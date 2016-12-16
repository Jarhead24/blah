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
        PlayerCharacter player = PlayerCharacter.Instance;
        Board board = Board.Instance;

        private static readonly ConsoleColor guiBackgroundColor = ConsoleColor.DarkBlue;
        private static readonly ConsoleColor guiTextColor = ConsoleColor.Gray;

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
        /// Displays the player info at the bottom of dungeon
        /// </summary>
        public void displayGameInfo(PlayerCharacter p)
        {
            Console.BackgroundColor = guiBackgroundColor;
            Console.ForegroundColor = guiTextColor;
            Console.Write(splitLongLines("Name: " + p.Name));
            Console.Write(splitLongLines("Health: " + p.HP));
            Console.Write(splitLongLines("Dungeon Level: " + GameInfoTracker.Instance.dungeonLevel));
            Console.Write(splitLongLines("Score: " + GameInfoTracker.Instance.score));
            Console.Write(splitLongLines("Press Escape to Return to Town"));
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
