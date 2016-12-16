using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{

    class Room
    {
        private Random rand = new Random(Environment.TickCount);
        private int height = Board.height;
        private int width = Board.width;
        private Tiles[,] boardLayout;
        public int xCenter { get; set; }
        public int yCenter { get; set; }
        public int distanceUp { get; set; }
        public int distanceRight { get; set; }

        /// <summary>
        /// Room constructor. Will automatically make a random valid room according to the current board.
        /// </summary>
        public Room(Tiles[,] bL)
        {
            boardLayout = bL;
            defineCenterOfRoom();
            int roomHeight = 0;
            int roomWidth = 0;
            // randomly decides height or width priority for room creation
            int hOrWPriority = rand.Next(0, 2);
            if(hOrWPriority == 0)
            {
                roomHeight = rand.Next(1, getValidHeightOfRoom(1));
                roomWidth = rand.Next(1, getValidWidthOfRoom(roomHeight));
            }
            else
            {
                roomWidth = rand.Next(1, getValidWidthOfRoom(1));
                roomHeight = rand.Next(1, getValidHeightOfRoom(roomWidth));
            }
            this.distanceUp = roomHeight;
            this.distanceRight = roomWidth;
        }

        /// <summary>
        /// Finds a valid spot for the center of a new room.
        /// </summary>
        private void defineCenterOfRoom()
        {
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
            this.yCenter = randHeight;
            this.xCenter = randWidth;
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
            for (int j = -2; j <= 2; j++)
            {
                for (int k = -2; k <= 2; k++)
                {
                    if (boardLayout[y + j, x + k].Symbol != Board.WALL_TILE.Symbol)
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
        /// <param name="roomHeight">height of room</param>
        /// <returns>maxiumum room width</returns>
        private int getValidWidthOfRoom(int roomHeight)
        {
            int maxRight = width - xCenter - 2;
            int maxLeft = xCenter - 1;
            // Check max to the right
            for (int i = xCenter + 2; i < width; i++)
            {
                for (int j = -roomHeight; j <= roomHeight; j++)
                {
                    if (boardLayout[j + yCenter, i].Symbol != Board.WALL_TILE.Symbol)
                    {
                        maxRight = Math.Min(maxRight, Math.Abs(xCenter - (i - 1)));
                        break;
                    }
                }
            }
            // Check max to the left
            for (int i = xCenter - 2; i > 0; i--)
            {
                for (int j = -roomHeight; j <= roomHeight; j++)
                {
                    if (boardLayout[j + yCenter, i].Symbol != Board.WALL_TILE.Symbol)
                    {
                        maxLeft = Math.Min(maxLeft, Math.Abs(xCenter - (i + 1)));
                        break;
                    }
                }
            }
            return Math.Min(maxLeft, maxRight);
        }

        /// <summary>
        /// Gets the maxium height of a room
        /// </summary>
        /// <param name="roomWidth">current width of room</param>
        /// <returns>maximum room height</returns>
        private int getValidHeightOfRoom(int roomWidth)
        {
            int maxUp = yCenter - 1;
            int maxDown = height - yCenter - 2;
            // Check max down
            for (int i = yCenter + 2; i < height; i++)
            {
                for (int j = -roomWidth; j <= roomWidth; j++)
                {
                    if (boardLayout[i, j + xCenter].Symbol != Board.WALL_TILE.Symbol)
                    {
                        maxDown = Math.Min(maxDown, Math.Abs(yCenter - (i - 1)));
                        break;
                    }
                }
            }
            // Check max up
            for (int i = yCenter - 2; i > 0; i--)
            {
                for (int j = -roomWidth; j <= roomWidth; j++)
                {
                    if (boardLayout[i, j + xCenter].Symbol != Board.WALL_TILE.Symbol)
                    {
                        maxUp = Math.Min(maxUp, Math.Abs(yCenter - (i + 1)));
                        break;
                    }
                }
            }
            return Math.Min(maxUp, maxDown);
        }
    }
}
