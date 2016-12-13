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

        public void displayPlayerInfo()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(("Name: " + player.name).PadRight(Board.width));
            Console.WriteLine(("Level " + player.level).PadRight(Board.width));
            Console.WriteLine(("Health: " + player.health).PadRight(Board.width));
            Console.WriteLine(("Attack: " + player.attack).PadRight(Board.width));
            Console.WriteLine(("Defense: " + player.defense).PadRight(Board.width));
            Console.WriteLine(("Job: " + player.occupation).PadRight(Board.width));
            Console.WriteLine(("Background: " + player.background).PadRight(Board.width));
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
