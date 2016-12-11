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
            while (numRooms < 1 || numRooms > maxRooms)
            {
                Console.Clear();
                Console.WriteLine("Input number of rooms to attempt to create (Minimum of 1. Maximum of "+maxRooms+"): ");
                Int32.TryParse(Console.ReadLine(), out numRooms);
            }
            Console.SetWindowSize(inputWidth, inputHeight);
            while (1 == 1) {
                Console.Clear();
                Board board = new Board(inputHeight, inputWidth, numRooms);
                board.showBoard();
                Console.ReadLine();
            }
        }
    }
}
