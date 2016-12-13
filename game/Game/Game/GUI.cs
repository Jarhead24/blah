using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class GUI
    {
        private static GUI instance = null;
        Player player = Player.Instance;
        Board board = Board.Instance;

        /// <summary>
        /// Makes GUI a singleton
        /// </summary>
        public static GUI Instance
        {
            // Singleton
            get
            {
                if (instance == null)
                {
                    instance = new GUI();
                }
                return instance;
            }
        }

        /// <summary>
        /// Displays the player info at the bottom
        /// </summary>
        public void displayPlayerInfo()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(splitLongLines("Name: " + player.name));
            Console.WriteLine(splitLongLines("Level " + player.level));
            Console.WriteLine(splitLongLines("Health: " + player.health));
            Console.WriteLine(splitLongLines("Attack: " + player.attack));
            Console.WriteLine(splitLongLines("Defense: " + player.defense));
            Console.WriteLine(splitLongLines("Job: " + player.occupation));
            Console.WriteLine(splitLongLines("Background: " + player.background));
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Splits long line to be multiple lines at the max width of the board
        /// </summary>
        /// <param name="line">Line to split</param>
        /// <returns>String with split lines.</returns>
        private string splitLongLines(string line)
        {
            StringBuilder newLine = new StringBuilder();
            int lengthLimit = Board.width;
            string[] lineToChars = line.Split(' ');

            string fullLine = "";
            foreach(string letter in lineToChars)
            {
                if((fullLine + letter).Length > lengthLimit)
                {
                    fullLine = fullLine.PadRight(lengthLimit);
                    newLine.AppendLine(fullLine);
                    fullLine = "";
                }
                fullLine += string.Format("{0} ", letter);
            }

            if (fullLine.Length > 0)
            {
                fullLine = fullLine.PadRight(lengthLimit);
                newLine.AppendLine(fullLine);
            }

            return newLine.ToString();
        }
    }
}
