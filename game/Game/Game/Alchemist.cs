using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Alchemist : BuildingLocation
    {
        private SortedDictionary<ConsoleKey, ServiceValuePair> Potions = new SortedDictionary<ConsoleKey, ServiceValuePair>();
        ConsoleColor TColor;
        ConsoleColor BColor;
        protected override void Build()
        {
            base.Build();
            Name = Owner.Name + " the Chemist";
            TColor = ConsoleColor.DarkBlue;
            BColor = ConsoleColor.DarkGray;

            _MenuCommands.Add(ConsoleKey.V, new MenuCommandPair() { Name = "View Items", Command = _InventoryController });
            _MenuCommands.Add(ConsoleKey.P, new MenuCommandPair() { Name = "Brew Potion", Command = _Brew });


            _InventoryCommands.Add(ConsoleKey.B, new InvCommandPair() { Name = "Buy Item", Command = _Buy });

            ServiceValuePair potionDict;
            potionDict = new ServiceValuePair()
            {
                Name = "Minor Health Potion",
                RepThreshold = 25,
                Cost = 10,
                Effect = new EffectParameter() { Type = "HEAL", Power = 10, Description = "Heals the drinker 10HP" }
            };
            Potions.Add(ConsoleKey.D1, potionDict);

            potionDict = new ServiceValuePair()
            {
                Name = "Strength Potion",
                RepThreshold = 45,
                Cost = 200,
                Effect = new EffectParameter() { Type = "STR", Power = 10, Description = "Adds 10 STR points to the Drinker" }
            };
            Potions.Add(ConsoleKey.D2, potionDict);

            potionDict = new ServiceValuePair()
            {
                Name = "Constitution Potion",
                RepThreshold = 75,
                Cost = 500,
                Effect = new EffectParameter() { Type = "CON", Power = 10, Description = "Adds 10 CON points to the Drinker" }
            };
            Potions.Add(ConsoleKey.D3, potionDict);

            potionDict = new ServiceValuePair()
            {
                Name = "Major Health Potion",
                RepThreshold = 100,
                Cost = 20,
                Effect = new EffectParameter() { Type = "HEAL", Power = 25, Description = "Heals the drinker 25HP" }
            };
            Potions.Add(ConsoleKey.D4, potionDict);

            potionDict = new ServiceValuePair()
            {
                Name = "Giant Strength Potion",
                RepThreshold = 100,
                Cost = 300,
                Effect = new EffectParameter() { Type = "STR", Power = 20, Description = "Adds 20 STR points to the Drinker" }
            };
            Potions.Add(ConsoleKey.D5, potionDict);

            potionDict = new ServiceValuePair()
            {
                Name = "Oxen Constitution Potion",
                RepThreshold = 100,
                Cost = 750,
                Effect = new EffectParameter() { Type = "CON", Power = 20, Description = "Adds 20 CON points to the Drinker" }
            };
            Potions.Add(ConsoleKey.D6, potionDict);

        }

        protected override void _ShowBuildingStats()
        {
            string str = String.Format("{0,-36}{1,15}  ", Owner.Name, "Money:" + Owner.Money + "\t");
            Console.ForegroundColor = TColor; Console.BackgroundColor = BColor;
            Console.Write(str);

            Tuple<ConsoleColor, string> display = _ReputationStatus();
            Console.ForegroundColor = display.Item1; Console.Write(String.Format("{0,40}  ", display.Item2));

            Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black;
        }

        protected virtual bool _Brew()
        {
            while (true)
            {
                Console.Clear();
                _ShowPlayerStats();
                _ShowBuildingStats();
                Console.WriteLine("\nWelcome to " + Name + "'s Alchemy Shoppe\n");
                foreach (KeyValuePair<ConsoleKey, ServiceValuePair> Potion in Potions)
                {
                    if (Potion.Value.RepThreshold < _Reputation)
                        Console.WriteLine("{0,-30} {1,17} ", Potion.Key + ") " + Potion.Value.Name, "Price:" + Potion.Value.Cost);
                }
                ConsoleKeyInfo kchoice = Console.ReadKey(); Console.CursorLeft = 0;
                if (kchoice.Key == ConsoleKey.Escape) { return true; }
                if (Potions.ContainsKey(kchoice.Key))
                {
                    if (Potions[kchoice.Key].Cost > Player.Money) { continue; }

                    Item i = new Item() { Name = Potions[kchoice.Key].Name, Weight = 2, Identified = true };
                    i.Effects.Add(Potions[kchoice.Key].Effect);

                    if (Player.AddItem(i)) { Player.Money -= Potions[kchoice.Key].Cost; return true; }
                    return false;
                }
            }
        }

        public override bool Exit()
        {
            Console.WriteLine("Come back when I get more herbs"); return true;
        }
    }
}
