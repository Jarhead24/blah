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
        public static int height;
        public static int width;
        public static int numberOfRooms;
        private tiles[,] board;
        public readonly tiles WALL_TILE = new tiles { Symbol = "#", Color = ConsoleColor.Green };
        public readonly tiles FLOOR_TILE = new tiles { Symbol = ".", Color = ConsoleColor.DarkGreen };
        public readonly tiles STAIR_TILE = new tiles { Symbol = ">", Color = ConsoleColor.Red };

        List<RoomInfo> roomsCreated = new List<RoomInfo>();

        public Board(int h, int w, int numRooms)
        {
            height = h;
            width = w;
            numberOfRooms = numRooms;
            board = new tiles[h, w];
            boardCreator();
        }
        private void boardCreator()
        {
            board = new tiles[height, width];
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
            int r = rand.Next(1, roomsCreated.Count - 1); // it will actually get a random number between start and end - 1
            drawStairs(roomsCreated[r]);
            // Link rooms last so rooms only try and avoid drawing over other rooms (don't try and avoid drawing over hallways instead)
            linkRooms();
        }
        private class RoomInfo
        {
            public int xCenter { get; set; }
            public int yCenter { get; set; }
            public int distanceUp { get; set; }
            public int distanceRight { get; set; }
        }
        private RoomInfo defineCenterOfRoom(RoomInfo room)
        {
            Random rand = new Random();
            bool isCenterValid = false;
            int randHeight = 0;
            int randWidth = 0;
            long startTime = Environment.TickCount;
            // -1 because index. -2 to keep away from border. +1 because of the way rand works
            while (!isCenterValid && Environment.TickCount - startTime < 5000)
            {
                randHeight = rand.Next(2, (height - 1) - 2 + 1);
                randWidth = rand.Next(2, (width - 1) - 2 + 1);
                isCenterValid = checkForValidCenter(randWidth, randHeight);
            }
            if (Environment.TickCount - startTime >= 5000)
            {
                Console.WriteLine("Could not make " + numberOfRooms + " rooms. Try making a bigger board or creating less rooms.");
                Console.ReadLine();
                Environment.Exit(0);
            }
            room.yCenter = randHeight;
            room.xCenter = randWidth;
            return room;
        }
        private bool checkForValidCenter(int x, int y)
        {
            bool isValid = true;
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (board[y + j, x + k].Symbol != "#" )
                    {
                        isValid = false;
                    }
                }
            }

            return isValid;
        }
        private int getValidWidthOfRoom(RoomInfo room)
        {
            int xCenter = room.xCenter;
            int maxRight = width - xCenter - 2;
            int maxLeft = xCenter - 1;
            // Check max to the right
            for (int i = xCenter + 2; i < width; i++)
            {
                if (board[room.yCenter, i].Symbol != "#")
                {
                    maxRight = Math.Min(maxRight, Math.Abs(xCenter - (i - 1)));
                    break;
                }
            }
            // Check max to the left
            for (int i = xCenter - 2; i > 0; i--)
            {
                if (board[room.yCenter, i].Symbol != "#")
                {
                    maxLeft = Math.Min(maxLeft, Math.Abs(xCenter - (i + 1)));
                    break;
                }
            }
            return Math.Min(maxLeft, maxRight);
        }
        private int getValidHeightOfRoom(RoomInfo room)
        {
            int yCenter = room.yCenter;
            int maxUp = yCenter - 1;
            int maxDown = height - yCenter - 2;
            // Check max down
            for (int i = yCenter + 2; i < height; i++)
            {
                if (board[i, room.xCenter].Symbol != "#")
                {
                    maxDown = Math.Min(maxDown, Math.Abs(yCenter - (i - 1)));
                    break;
                }
            }
            // Check max up
            for (int i = yCenter - 2; i > 0; i--)
            {
                if (board[i, room.xCenter].Symbol != "#")
                {
                    maxUp = Math.Min(maxUp, Math.Abs(yCenter - (i + 1)));
                    break;
                }
            }
            return Math.Min(maxUp, maxDown);
        }
        private void createRoom()
        {
            RoomInfo newRoom = new RoomInfo();
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
        private void linkRooms()
        {
            // Iterating backwards through rooms created. Don't want to run on the last room because nothing to connect to.
            for (int i = roomsCreated.Count - 1; i > 0; i--)
            {
                RoomInfo startRoom = roomsCreated[i];
                RoomInfo destinationRoom = roomsCreated[i - 1];
                // Random choose to go left/right first or up/down
                Random rand = new Random();
                int randomDirection = rand.Next(0, 2);
                // Right/Left first
                if (randomDirection == 0)
                {
                    for (int j = Math.Min(destinationRoom.xCenter, startRoom.xCenter); j <= Math.Max(destinationRoom.xCenter, startRoom.xCenter); j++)
                    {
                        if (board[startRoom.yCenter, j].Symbol == "#")
                        {
                            board[startRoom.yCenter, j] = FLOOR_TILE;
                        }
                    }
                    for (int j = Math.Min(destinationRoom.yCenter, startRoom.yCenter); j <= Math.Max(destinationRoom.yCenter, startRoom.yCenter); j++)
                    {
                        if (board[j, destinationRoom.xCenter].Symbol == "#")
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
                        if (board[j, startRoom.xCenter].Symbol == "#")
                        {
                            board[j, startRoom.xCenter] = FLOOR_TILE;
                        }
                    }
                    for (int j = Math.Min(destinationRoom.xCenter, startRoom.xCenter); j <= Math.Max(destinationRoom.xCenter, startRoom.xCenter); j++)
                    {
                        if (board[destinationRoom.yCenter, j].Symbol == "#")
                        {
                            board[destinationRoom.yCenter, j] = FLOOR_TILE;
                        }
                    }

                }
            }
        }
        private void drawStairs(RoomInfo room)
        {
            Random rand = new Random();
            board[rand.Next(room.yCenter - room.distanceUp, room.yCenter + room.distanceUp + 1), rand.Next(room.xCenter - room.distanceRight, room.xCenter + room.distanceRight + 1)] = STAIR_TILE;

        }
        private void drawRoom(RoomInfo room)
        {
            for (int y = room.yCenter - room.distanceUp; y <= room.yCenter + room.distanceUp; y++)
            {
                for (int x = room.xCenter - room.distanceRight; x <= room.xCenter + room.distanceRight; x++)
                {
                    board[y, x] = FLOOR_TILE;
                }
            }
        }
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
    }
}
