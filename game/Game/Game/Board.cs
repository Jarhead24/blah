using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Board
    {
        
        public Tiles[,] board;
        public static int height;
        public static int width;
        public static int numberOfRooms;
        public static readonly Tiles WALL_TILE = new Tiles { Symbol = "#", Color = ConsoleColor.Green };
        public static readonly Tiles FLOOR_TILE = new Tiles { Symbol = ".", Color = ConsoleColor.DarkGreen };
        public static readonly Tiles HALL_TILE = new Tiles { Symbol = ".", Color = ConsoleColor.DarkMagenta };
        public static readonly Tiles STAIR_TILE = new Tiles { Symbol = ">", Color = ConsoleColor.Red };

        private static Board instance = null;
        private Player player = Player.Instance;
        private Random rand = null;
        private List<Room> roomsCreated = new List<Room>();

        /// <summary>
        /// Board Construtor
        /// </summary>
        public Board()
        {
            rand = new Random(Environment.TickCount);
        }

        /// <summary>
        /// Makes/Returns board instance
        /// </summary>
        public static Board Instance
        {
            // Singleton
            get
            {
                if(instance == null)
                {
                    instance = new Board();
                }
                return instance;
            }
        }

        /// <summary>
        /// Calls the need functions to create the board
        /// </summary>
        /// <param name="h">height of the board</param>
        /// <param name="w">width of the board</param>
        /// <param name="numRooms">number of rooms on the board</param>
        public void createBoard(int h, int w, int numRooms)
        {
            height = h;
            width = w;
            numberOfRooms = numRooms;
            board = new Tiles[h, w];
            // Make everything a #
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    board[y, x] = WALL_TILE;
                }
            }
            for (int i = 0; i < numberOfRooms; i++)
            {
                createRoom();
            }
            
            int randStairsLoc = rand.Next(0, roomsCreated.Count);
            int randPlayerLoc = rand.Next(0, roomsCreated.Count);
            drawStairs(roomsCreated[randStairsLoc]);
            drawPlayer(roomsCreated[randPlayerLoc]);
            // Link rooms last so rooms only try and avoid drawing over other rooms (don't try and avoid drawing over hallways instead)
            linkRooms();
        }

        /// <summary>
        /// Creates a random valid room on the board.
        /// </summary>
        private void createRoom()
        {
            Room newRoom = new Room();
            drawRoom(newRoom);
            roomsCreated.Add(newRoom);
        }

        /// <summary>
        /// Draws the initial player location
        /// </summary>
        /// <param name="room">Random room to put player in</param>
        private void drawPlayer(Room room)
        {
            Player player = Player.Instance;     
            int randomY = rand.Next(room.yCenter - room.distanceUp, room.yCenter + room.distanceUp + 1);
            int randomX = rand.Next(room.xCenter - room.distanceRight, room.xCenter + room.distanceRight + 1);
            player.xLocation = randomX;
            player.yLocation = randomY;
            player.CurrentTileOn = board[randomY, randomX];
            board[randomY, randomX] = player.PlayerTile;
        }

        /// <summary>
        /// Links rooms with hallways
        /// </summary>
        private void linkRooms()
        {
            // Iterating backwards through rooms created. Don't want to run on the last room because nothing to connect to.
            for (int i = roomsCreated.Count - 1; i > 0; i--)
            {
                Room startRoom = roomsCreated[i];
                Room destinationRoom = roomsCreated[i - 1];
                // Random choose to go left/right first or up/down
                
                int randomDirection = rand.Next(0, 2);
                // Right/Left first
                if (randomDirection == 0)
                {
                    for (int j = Math.Min(destinationRoom.xCenter, startRoom.xCenter); j <= Math.Max(destinationRoom.xCenter, startRoom.xCenter); j++)
                    {
                        if (board[startRoom.yCenter, j].Symbol == WALL_TILE.Symbol)
                        {
                            board[startRoom.yCenter, j] = HALL_TILE;
                        }
                    }
                    for (int j = Math.Min(destinationRoom.yCenter, startRoom.yCenter); j <= Math.Max(destinationRoom.yCenter, startRoom.yCenter); j++)
                    {
                        if (board[j, destinationRoom.xCenter].Symbol == WALL_TILE.Symbol)
                        {
                            board[j, destinationRoom.xCenter] = HALL_TILE;
                        }
                    }
                }
                // Up/Down first
                else
                {
                    for (int j = Math.Min(destinationRoom.yCenter, startRoom.yCenter); j <= Math.Max(destinationRoom.yCenter, startRoom.yCenter); j++)
                    {
                        if (board[j, startRoom.xCenter].Symbol == WALL_TILE.Symbol)
                        {
                            board[j, startRoom.xCenter] = HALL_TILE;
                        }
                    }
                    for (int j = Math.Min(destinationRoom.xCenter, startRoom.xCenter); j <= Math.Max(destinationRoom.xCenter, startRoom.xCenter); j++)
                    {
                        if (board[destinationRoom.yCenter, j].Symbol == WALL_TILE.Symbol)
                        {
                            board[destinationRoom.yCenter, j] = HALL_TILE;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Draws the stairs in a random location in the given room
        /// </summary>
        /// <param name="room">room to draw stairs in</param>
        private void drawStairs(Room room)
        {
            int randomY = rand.Next(room.yCenter - room.distanceUp, room.yCenter + room.distanceUp + 1);
            int randomX = rand.Next(room.xCenter - room.distanceRight, room.xCenter + room.distanceRight + 1);
            board[randomY, randomX] = STAIR_TILE;
        }

        /// <summary>
        /// Draws the given room on the board
        /// </summary>
        /// <param name="room"></param>
        private void drawRoom(Room room)
        {
            for (int y = room.yCenter - room.distanceUp; y <= room.yCenter + room.distanceUp; y++)
            {
                for (int x = room.xCenter - room.distanceRight; x <= room.xCenter + room.distanceRight; x++)
                {
                    board[y, x] = FLOOR_TILE;
                }
            }
        }

        /// <summary>
        /// Displays the board on screen.
        /// </summary>
        public void showBoard()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.ForegroundColor = board[y, x].Color;
                    Console.Write(board[y, x].Symbol);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Handles moving and redrawing the player
        /// </summary>
        /// <param name="currX"></param>
        /// <param name="currY"></param>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        /// <param name="player"></param>
        public LocationInformation movePlayer(int currX, int currY, int destX, int destY)
        {
            Tiles prevTile = player.CurrentTileOn;
            Tiles destTile = board[destY, destX];
            LocationInformation locInfo = new LocationInformation(currX, currY, player.CurrentTileOn);

            //Check if destination is valid
            if (board[destY, destX].Symbol != WALL_TILE.Symbol)
            {
                board[destY, destX] = player.PlayerTile;
                board[currY, currX] = player.CurrentTileOn;
                Console.SetCursorPosition(currX, currY);
                Console.ForegroundColor = prevTile.Color;
                Console.Write(prevTile.Symbol);
                Console.SetCursorPosition(destX, destY);
                Console.ForegroundColor = player.PlayerTile.Color;
                Console.Write(player.PlayerTile.Symbol);
                locInfo.changeLocationInformation(destX, destY, destTile);
            }
            Console.SetCursorPosition(width, height);
            return locInfo;
        }
    }
}
