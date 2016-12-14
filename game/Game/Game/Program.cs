using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            generatePlayer();
            generateDungeon();
            drawInitialGUI();
            gameLoop();
            Environment.Exit(0);
        }

        /// <summary>
        /// Generates the starting player and their info
        /// </summary>
        private static void generatePlayer()
        {
            Player player = new Player();
        }
        
        /// <summary>
        /// Generates the initial dungeon with the player in it
        /// </summary>
        private static void generateDungeon()
        {
            int inputWidth = 1;
            int inputHeight = 1;
            int numRooms = 0;
            int maxRooms = 99999999;
            Board board = Board.Instance;

            while (inputWidth < 5)
            {
                Console.Clear();
                Console.WriteLine("Input dungeon width (Minimum of 5): ");
                Int32.TryParse(Console.ReadLine(), out inputWidth);
            }
            while (inputHeight < 5)
            {
                Console.Clear();
                Console.WriteLine("Input dungeon height (Minimum of 5): ");
                Int32.TryParse(Console.ReadLine(), out inputHeight);
            }
            maxRooms = (inputWidth - 2) * (inputHeight - 2) / 9;
            maxRooms = Math.Min(maxRooms, 10);
            while (numRooms < 1 || numRooms > maxRooms)
            {
                Console.Clear();
                Console.WriteLine("Input number of rooms to attempt to create (Minimum of 1. Maximum of " + maxRooms + "): ");
                Int32.TryParse(Console.ReadLine(), out numRooms);
            }
            Console.SetWindowSize(inputWidth + 5, inputHeight + 10);
            Console.Clear();
            board.createBoard(inputHeight, inputWidth, numRooms);
            board.showBoard();
        }

        /// <summary>
        /// Draws the initial GUI
        /// </summary>
        private static void drawInitialGUI()
        {
            GUI.Instance.displayGameInfo();
        }

        /// <summary>
        /// The main game loop.
        /// </summary>
        private static void gameLoop()
        {
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        Player.Instance.moveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        Player.Instance.moveDown();
                        break;
                    case ConsoleKey.RightArrow:
                        Player.Instance.moveRight();
                        break;
                    case ConsoleKey.LeftArrow:
                        Player.Instance.moveLeft();
                        break;
                    case ConsoleKey.Enter:
                        regenerateDungeon();
                        break;
                    default:
                        break;
                }
                if (GameInfoTracker.Instance.currentLevelComplete)
                {
                    GameInfoTracker.Instance.levelComplete();
                    GameInfoTracker.Instance.currentLevelComplete = false;
                    regenerateDungeon();
                }
            }
        }

        /// <summary>
        /// Generates a new random dungeon
        /// </summary>
        private static void regenerateDungeon()
        {
            Console.Clear();
            Board.Instance.regenerateBoard();
            Board.Instance.showBoard();
            drawInitialGUI();
        }
    }
}
