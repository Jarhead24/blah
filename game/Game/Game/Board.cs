using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Board : ILocation
    {
        
        public Tiles[,] board;
        public static int height = 30;
        public static int width = 100;
        public static int numberOfRooms = 2;
        public static readonly Tiles WALL_TILE = new Tiles { Symbol = "#", Color = ConsoleColor.Green };
        public static readonly Tiles FLOOR_TILE = new Tiles { Symbol = ".", Color = ConsoleColor.DarkGreen };
        public static readonly Tiles HALL_TILE = new Tiles { Symbol = ".", Color = ConsoleColor.DarkMagenta };
        public static readonly Tiles STAIR_TILE = new Tiles { Symbol = ">", Color = ConsoleColor.Red };
        public string Name { get; set; }
        public ConsoleColor TextColor { get; set; }
        public ConsoleColor BackColor { get; set; }

        private static Board instance = null;
        private PlayerCharacter player;
        private Random rand = null;
        private List<Room> roomsCreated = new List<Room>();

        /// <summary>
        /// Board Construtor
        /// </summary>
        public Board()
        {
            Name = "The Endless Dungeon";
            TextColor = ConsoleColor.Red;
            BackColor = ConsoleColor.Black;
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
            this.board = new Tiles[h, w];
            // Make everything a #
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    this.board[y, x] = WALL_TILE;
                }
            }
            for (int i = 0; i < numberOfRooms; i++)
            {
                createRoom();
            }
            
            int randStairsLoc = rand.Next(0, roomsCreated.Count);
            int randPlayerLoc = rand.Next(0, roomsCreated.Count);

            //To make sure the stairs and the player aren't placed in the same room
            if (roomsCreated.Count > 1)
            {
                while (randPlayerLoc == randStairsLoc)
                {
                    randPlayerLoc = rand.Next(0, roomsCreated.Count);
                }
            }
            drawStairs(roomsCreated[randStairsLoc]);
            drawPlayer(roomsCreated[randPlayerLoc]);
            // Link rooms last so rooms only try and avoid drawing over other rooms (don't try and avoid drawing over hallways instead)
            linkRooms();
        }

        /// <summary>
        /// Regenerates the dungeon with the already enter parameters
        /// </summary>
        public void regenerateBoard(int numRooms)
        {
            board = new Tiles[height, width];
            roomsCreated.Clear();
            // Make everything a #
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    board[y, x] = WALL_TILE;
                }
            }
            for (int i = 0; i < numRooms; i++)
            {
                createRoom();
            }

            int randStairsLoc = rand.Next(0, roomsCreated.Count);
            int randPlayerLoc = rand.Next(0, roomsCreated.Count);

            //To make sure the stairs and the player aren't placed in the same room
            if (roomsCreated.Count > 1)
            {
                while (randPlayerLoc == randStairsLoc)
                {
                    randPlayerLoc = rand.Next(0, roomsCreated.Count);
                }
            }
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
            Room newRoom = new Room(board);
            drawRoom(newRoom);
            roomsCreated.Add(newRoom);
        }

        /// <summary>
        /// Draws the initial player location
        /// </summary>
        /// <param name="room">Random room to put player in</param>
        private void drawPlayer(Room room)
        {
            int randomY = rand.Next(room.yCenter - room.distanceUp, room.yCenter + room.distanceUp + 1);
            int randomX = rand.Next(room.xCenter - room.distanceRight, room.xCenter + room.distanceRight + 1);
            player.xLocation = randomX;
            player.yLocation = randomY;
            player.CurrentTileOn = board[randomY, randomX];
            board[randomY, randomX] = PlayerCharacter.PLAYER_TILE;
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
            Console.BackgroundColor = ConsoleColor.Black;
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
        /// Handles moving and redrawing the player and deciding if the level is complete
        /// </summary>
        /// <param name="currX">Current x location</param>
        /// <param name="currY">Current y location</param>
        /// <param name="destX">Destination x location</param>
        /// <param name="destY">Destination y location</param>
        public LocationInformation movePlayer(int currX, int currY, int destX, int destY)
        {
            Tiles prevTile = player.CurrentTileOn;
            Tiles destTile = board[destY, destX];
            LocationInformation locInfo = new LocationInformation(currX, currY, player.CurrentTileOn);

            //Check if destination is valid
            if (board[destY, destX].Symbol != WALL_TILE.Symbol)
            {
                this.board[destY, destX] = PlayerCharacter.PLAYER_TILE;
                this.board[currY, currX] = player.CurrentTileOn;
                Console.SetCursorPosition(currX, currY);
                Console.ForegroundColor = prevTile.Color;
                Console.Write(prevTile.Symbol);
                Console.SetCursorPosition(destX, destY);
                Console.ForegroundColor = PlayerCharacter.PLAYER_TILE.Color;
                Console.Write(PlayerCharacter.PLAYER_TILE.Symbol);
                locInfo.changeLocationInformation(destX, destY, destTile);
            }
            Console.SetCursorPosition(width, height);
            if (locInfo.tileOn.Symbol == STAIR_TILE.Symbol)
            {
                GameInfoTracker.Instance.currentLevelComplete = true;
            }
            return locInfo;
        }



        public bool Exit()
        {
            return true;
        }

        public bool Enter(PlayerCharacter p)
        {
            player = p;
            ConsoleKeyInfo keyInfo;
            Console.Clear();
            if(this.board == null)
            {
                this.createBoard(30, 100, 2);
            }
            this.showBoard();
            drawInitialGUI(p);
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        moveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        moveDown();
                        break;
                    case ConsoleKey.RightArrow:
                        moveRight();
                        break;
                    case ConsoleKey.LeftArrow:
                        moveLeft();
                        break;
                    default:
                        break;
                }
                if (GameInfoTracker.Instance.currentLevelComplete)
                {
                    GameInfoTracker.Instance.levelComplete();
                    p.Money += GameInfoTracker.Instance.score;
                    GameInfoTracker.Instance.currentLevelComplete = false;
                    regenerateDungeon();
                }
            }
            return true;
        }

        /// <summary>
        /// Move player up one space
        /// </summary>
        public void moveUp()
        {
            LocationInformation newLoc = movePlayer(player.xLocation, player.yLocation, player.xLocation, player.yLocation - 1);
            player.CurrentTileOn = newLoc.tileOn;
            player.xLocation = newLoc.xLocation;
            player.yLocation = newLoc.yLocation;
        }

        /// <summary>
        /// Move player down one space
        /// </summary>
        public void moveDown()
        {
            LocationInformation newLoc = movePlayer(player.xLocation, player.yLocation, player.xLocation, player.yLocation + 1);
            player.CurrentTileOn = newLoc.tileOn;
            player.xLocation = newLoc.xLocation;
            player.yLocation = newLoc.yLocation;
        }

        /// <summary>
        /// Move player right one space
        /// </summary>
        public void moveRight()
        {
            LocationInformation newLoc = movePlayer(player.xLocation, player.yLocation, player.xLocation + 1, player.yLocation);
            player.CurrentTileOn = newLoc.tileOn;
            player.xLocation = newLoc.xLocation;
            player.yLocation = newLoc.yLocation;
        }

        /// <summary>
        /// Move player left one space
        /// </summary>
        public void moveLeft()
        {
            LocationInformation newLoc = movePlayer(player.xLocation, player.yLocation, player.xLocation - 1, player.yLocation);
            player.CurrentTileOn = newLoc.tileOn;
            player.xLocation = newLoc.xLocation;
            player.yLocation = newLoc.yLocation;
        }

        /// <summary>
        /// Generates a new random dungeon
        /// </summary>
        private void regenerateDungeon()
        {
            Console.Clear();
            regenerateBoard(Math.Min(10,GameInfoTracker.Instance.dungeonLevel+1));
            showBoard();
            drawInitialGUI(player);
        }

        /// <summary>
        /// Draws the initial GUI
        /// </summary>
        private void drawInitialGUI(PlayerCharacter p)
        {
            GUI.Instance.displayGameInfo(p);
        }
    }
}
