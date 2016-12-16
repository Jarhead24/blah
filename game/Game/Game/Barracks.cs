using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Barracks : BuildingLocation
    {
        List<Character> Hirelings = new List<Character>();
        protected override void Build()
        {
            base.Build();
            Name = "Barracks";

            //Hirelings
            for (int i = 0; i < 7; i++)
            {
                Hirelings.Add(new Character()
                {
                    Name = CreatureBuilder.GetName(),
                    Strength = DieRoller.roll(3, 6),
                    Dexterity = DieRoller.roll(3, 6),
                    Constitution = DieRoller.roll(3, 6),
                    Level = DieRoller.roll(1, 3)
                });
            }

            //Add menu commands
            _MenuCommands.Add(ConsoleKey.H, new MenuCommandPair() { Name = "Hire Hireling", Command = _HireHireling });
            _MenuCommands.Add(ConsoleKey.D, new MenuCommandPair() { Name = "Donate Item", Command = _DonateItem });


        }
        public override bool Exit()
        {
            Console.WriteLine("Rock The Casbah!"); return true;

        }
        protected virtual bool _HireHireling()
        {
            Console.Clear();
            int c = 1;
            List<int> costs = new List<int>();
            foreach (Character Hireling in Hirelings)
            {
                int cost = (Hireling.Strength + Hireling.Dexterity + Hireling.Constitution) * Hireling.Level * 5;
                costs.Add(cost);
                string x = String.Format("{0,-35}  {1,20}",c + ") " + Hireling.Name, "\tLvl:" + Hireling.Level + "\tStr:" + Hireling.Strength + "\tDex:" + Hireling.Dexterity + "\tCon:" + Hireling.Constitution + "\tCost:" + cost);
                Console.WriteLine(x);
                c++;
            }
            while (true)
            {
                Console.WriteLine("\nWho shall you hire?:");
                ConsoleKeyInfo k = Console.ReadKey(); Console.CursorLeft = 0; 
                if (k.Key == ConsoleKey.Escape) { return false; }
                int choice;
                if (Int32.TryParse(char.ToUpper(k.KeyChar).ToString(), out choice))
                {
                    if (Player.Money < costs[choice - 1])
                    {
                        Console.WriteLine("Sorry, We dont work for peanuts!"); Console.ReadKey(); continue;
                    }
                    if (Player.Hireling != null)
                    {
                       Console.WriteLine("\nYou have a hireling at the moment. Do you wish to release current Hireling (Y/N)?: ");
                        ConsoleKeyInfo k2 = Console.ReadKey(); Console.CursorLeft = 0; 
                        if (k2.Key == ConsoleKey.N) { return false; }
                    }

                    Player.Hireling = Hirelings[choice - 1];
                    Hirelings.RemoveAt(choice - 1);
                    Player.Money -= costs[choice - 1];
                    costs.RemoveAt(choice - 1);
                    return true;

                }
            }
        }
        protected virtual bool _DonateItem()
        {
            Item i;

            if (_SelectPlayerItem(out i))
            {

                try
                {
                    Item item = (EquipItem)i;
                    int value = item.Level * 2;
                    _Reputation += value;
                    Console.WriteLine("We will punish your foes.");
                    Player.RemoveItem(out i, ID: item.ID);
                    Console.ReadKey();
                    return true;
                }
                catch
                {
                    Console.WriteLine("That is unusable, what do you take us for!?");
                    Console.ReadKey();
                    return true;
                }
            }
            return false;}

        protected override void _ShowBuildingStats()
        {//display relevant player stats
            string str = String.Format(Owner.Name, "Money:" + Owner.Money + "\t");
            Console.ForegroundColor = TextColor; Console.BackgroundColor = BackColor;
            Console.Write(str);

            //Repuation
            Tuple<ConsoleColor, string> display = _ReputationStatus();
            Console.ForegroundColor = display.Item1; Console.Write(String.Format("                             " + display.Item2));

            Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
