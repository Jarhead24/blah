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
            int inputWidth = 1;
            int inputHeight = 1;
            int numRooms = 0;
            int maxRooms = 99999999;
            ConsoleKeyInfo keyInfo;

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
                Console.WriteLine("Input number of rooms to attempt to create (Minimum of 1. Maximum of "+maxRooms+"): ");
                Int32.TryParse(Console.ReadLine(), out numRooms);
            }
            Console.SetWindowSize(inputWidth+10, inputHeight+5);
            Console.Clear();
            Board board = Board.Instance;
            Player player = Player.Instance;
            board.createBoard(inputHeight, inputWidth, numRooms);
            board.showBoard();
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        player.moveUp();
                        break;
                    case ConsoleKey.DownArrow:
                        player.moveDown();
                        break;
                    case ConsoleKey.RightArrow:
                        player.moveRight();
                        break;
                    case ConsoleKey.LeftArrow:
                        player.moveLeft();
                        break;
                    default:
                        break;
                }            
            }
            Environment.Exit(0);
        }

        private void gameLoop()
        {

        }
    }
}
