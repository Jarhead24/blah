using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class LocationMap : ILocation
    {
        public List<ILocation> Map = new List<ILocation>();
        public string Name { get; set; }
        private SortedDictionary<ConsoleKey, MenuCommandPair> _MapCommands = new SortedDictionary<ConsoleKey, MenuCommandPair>();
        public PlayerCharacter Player; //currently visiting player
        public ConsoleColor TextColor { get; set; } = ConsoleColor.Green;//Text Color for location
        public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;//back color for location



        public LocationMap()//Builds commands for main menu. Run once before using LocationSelector
        {
            _MapCommands.Add(ConsoleKey.Escape, new MenuCommandPair() { Name = "Exit", Command = () => false });
            _MapCommands.Add(ConsoleKey.I, new MenuCommandPair() { Name = "View Player Inventory", Command = ViewPlayerInventory });
            _MapCommands.Add(ConsoleKey.C, new MenuCommandPair() { Name = "View Player Character", Command = ViewPlayerCharacter });

        }

        public bool Exit()
        {
            return true;
        }

        public bool Enter(PlayerCharacter p)
        {
            Player = p;
            while (true)
            {
                int i = 1; //start location selector out on number 1
                int choice; //user input

                Console.Clear();

                //Player Status bar (Use ShowPlayerStats() inside of location)
                string str = String.Format("{0,-35}{1,15}  {2,20}", Player.Name, "Money:" + Player.Money, "HP:" + Player.HP + "/" + Player.MaxHP);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(str);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Location: " + Name + "\n");
                Console.ForegroundColor = ConsoleColor.White;

                //display Locations
                foreach (ILocation Location in Map)
                {
                    Console.Write(i.ToString() + ")\t\t");
                    Console.ForegroundColor = Location.TextColor;
                    Console.Write(Location.Name + "\n");
                    i += 1;
                    Console.ForegroundColor = ConsoleColor.White;
                }


                Console.WriteLine();

                //display Commands
                foreach (KeyValuePair<ConsoleKey, MenuCommandPair> Command in _MapCommands)
                {
                    Console.WriteLine("{0})\t\t{1}", Command.Key, Command.Value.Name);
                }

                ConsoleKeyInfo k = Console.ReadKey(); Console.CursorLeft = 0; //get user input

                if (Int32.TryParse(char.ToUpper(k.KeyChar).ToString(), out choice))
                {   //if choice is valid, enter location
                    if (choice <= Map.Count) { Map[choice - 1].Enter(Player); }
                    else { Console.WriteLine("Invalid Choice"); Console.ReadKey(); }
                }
                else
                {//Didn't parse
                    if (_MapCommands.ContainsKey(k.Key))
                    { //if command in MapCommand Dictionary, then call it. Commands which exit the shop should return false
                        if (_MapCommands[k.Key].Command() == false)
                        { return true; }//leave shop successfully 
                    }
                }

            }
        }


        public void AddLocation(ILocation Location) { Map.Add(Location); }
        private bool ViewPlayerCharacter()
        {
            Player.DisplayStatSheet(); return true;
        }
        private bool ViewPlayerInventory()
        {
            Player.DisplayInventory(); return true;
        }

    }
}
