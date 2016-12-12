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
        private static Board instance = null;

        public static int height;
        public static int width;
        public static int numberOfRooms;
        private tiles[,] board;
        public readonly tiles WALL_TILE = new tiles { Symbol = "#", Color = ConsoleColor.Green };
        public readonly tiles FLOOR_TILE = new tiles { Symbol = ".", Color = ConsoleColor.DarkGreen };
        public readonly tiles STAIR_TILE = new tiles { Symbol = ">", Color = ConsoleColor.Red };

        List<Room> roomsCreated = new List<Room>();

        public Board()
        {
        }

        /// <summary>
        /// Board Constructor
        /// </summary>
        /// <param name="h">board height</param>
        /// <param name="w">board width</param>
        /// <param name="numRooms">number of rooms on the board</param>
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
        public void createBoard(int h, int w, int numRooms)
        {
            height = h;
            width = w;
            numberOfRooms = numRooms;
            board = new tiles[h, w];
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
            Random rand = new Random();
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
            newRoom = defineCenterOfRoom(newRoom);
            Random rand = new Random();
            // How far out in each direction it goes. Not the actual height/width
            // -2 is because -1 to stay 1 away from edge and -1 because array ends at -1 width/height
            int roomHeight = rand.Next(1, getValidHeightOfRoom(newRoom));
            int roomWidth = rand.Next(1, getValidWidthOfRoom(newRoom));
            newRoom.distanceUp = roomHeight;
            newRoom.distanceRight = roomWidth;
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
            Random rand = new Random();
            int randomY = rand.Next(room.yCenter - room.distanceUp, room.yCenter + room.distanceUp + 1);
            int randomX = rand.Next(room.xCenter - room.distanceRight, room.xCenter + room.distanceRight + 1);
            player.xLocation = randomX;
            player.yLocation = randomY;
            player.CurrentTileOn = board[randomY, randomX];
            board[randomY, randomX] = player.PlayerTile;
        }

        /// <summary>
        /// Finds a valid spot for the center of a new room.
        /// </summary>
        /// <param name="room"></param>
        /// <returns>Returns a room now defined with a valid center.</returns>
        private Room defineCenterOfRoom(Room room)
        {
            Random rand = new Random();
            bool isCenterValid = false;
            int randHeight = 0;
            int randWidth = 0;
            long startTime = Environment.TickCount;
            // -1 because index. -2 to keep away from board border. +1 because of the way rand works
            while (!isCenterValid && Environment.TickCount - startTime < 5000)
            {
                randHeight = rand.Next(2, (height - 1) - 2 + 1);
                randWidth = rand.Next(2, (width - 1) - 2 + 1);
                isCenterValid = checkForValidCenter(randWidth, randHeight);
            }
            if (Environment.TickCount - startTime >= 5000)
            {
                Console.WriteLine("Ran out of space placing rooms. Try making a bigger board or creating less rooms.");
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            room.yCenter = randHeight;
            room.xCenter = randWidth;
            return room;
        }

        /// <summary>
        /// Checks if a given coordinate is a valid location for the center of a new room.
        /// </summary>
        /// <param name="x">x location</param>
        /// <param name="y">y location</param>
        /// <returns>true if it's a valid center</returns>
        private bool checkForValidCenter(int x, int y)
        {
            bool isValid = true;
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (board[y + j, x + k].Symbol != WALL_TILE.Symbol )
                    {
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Gets the maximum width of a room
        /// </summary>
        /// <param name="room">room with already defined center</param>
        /// <returns>maxiumum room width</returns>
        private int getValidWidthOfRoom(Room room)
        {
            int xCenter = room.xCenter;
            int maxRight = width - xCenter - 2;
            int maxLeft = xCenter - 1;
            // Check max to the right
            for (int i = xCenter + 2; i < width; i++)
            {
                if (board[room.yCenter, i].Symbol != WALL_TILE.Symbol)
                {
                    maxRight = Math.Min(maxRight, Math.Abs(xCenter - (i - 1)));
                    break;
                }
            }
            // Check max to the left
            for (int i = xCenter - 2; i > 0; i--)
            {
                if (board[room.yCenter, i].Symbol != WALL_TILE.Symbol)
                {
                    maxLeft = Math.Min(maxLeft, Math.Abs(xCenter - (i + 1)));
                    break;
                }
            }
            return Math.Min(maxLeft, maxRight);
        }

        /// <summary>
        /// Gets the maxium height of a room
        /// </summary>
        /// <param name="room">room with already defined center</param>
        /// <returns>maximum room height</returns>
        private int getValidHeightOfRoom(Room room)
        {
            int yCenter = room.yCenter;
            int maxUp = yCenter - 1;
            int maxDown = height - yCenter - 2;
            // Check max down
            for (int i = yCenter + 2; i < height; i++)
            {
                if (board[i, room.xCenter].Symbol != WALL_TILE.Symbol)
                {
                    maxDown = Math.Min(maxDown, Math.Abs(yCenter - (i - 1)));
                    break;
                }
            }
            // Check max up
            for (int i = yCenter - 2; i > 0; i--)
            {
                if (board[i, room.xCenter].Symbol != WALL_TILE.Symbol)
                {
                    maxUp = Math.Min(maxUp, Math.Abs(yCenter - (i + 1)));
                    break;
                }
            }
            return Math.Min(maxUp, maxDown);
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
                Random rand = new Random();
                int randomDirection = rand.Next(0, 2);
                // Right/Left first
                if (randomDirection == 0)
                {
                    for (int j = Math.Min(destinationRoom.xCenter, startRoom.xCenter); j <= Math.Max(destinationRoom.xCenter, startRoom.xCenter); j++)
                    {
                        if (board[startRoom.yCenter, j].Symbol == WALL_TILE.Symbol)
                        {
                            board[startRoom.yCenter, j] = FLOOR_TILE;
                        }
                    }
                    for (int j = Math.Min(destinationRoom.yCenter, startRoom.yCenter); j <= Math.Max(destinationRoom.yCenter, startRoom.yCenter); j++)
                    {
                        if (board[j, destinationRoom.xCenter].Symbol == WALL_TILE.Symbol)
                        {
                            board[j, destinationRoom.xCenter] = FLOOR_TILE;
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
                            board[j, startRoom.xCenter] = FLOOR_TILE;
                        }
                    }
                    for (int j = Math.Min(destinationRoom.xCenter, startRoom.xCenter); j <= Math.Max(destinationRoom.xCenter, startRoom.xCenter); j++)
                    {
                        if (board[destinationRoom.yCenter, j].Symbol == WALL_TILE.Symbol)
                        {
                            board[destinationRoom.yCenter, j] = FLOOR_TILE;
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
            Random rand = new Random();
            board[rand.Next(room.yCenter - room.distanceUp, room.yCenter + room.distanceUp + 1), rand.Next(room.xCenter - room.distanceRight, room.xCenter + room.distanceRight + 1)] = STAIR_TILE;

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
        public void movePlayer(int currX, int currY, int destX, int destY, Player player)
        {
            tiles prevTile = player.CurrentTileOn;
            tiles destTile = board[destY, destX];

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
                player.yLocation = destY;
                player.xLocation = destX;
                player.CurrentTileOn = destTile;
            }
            Console.SetCursorPosition(width, height);
        }
    }
}
