using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Sage : BuildingLocation
    {
        protected override void Build()
        {
            base.Build();
            Name = Owner.Name + " the Sage";
            TextColor = ConsoleColor.Blue;

            //Items, Identify, and Scribe Items menu
            _MenuCommands.Add(ConsoleKey.I, new MenuCommandPair() { Name = "Identify an item", Command = _Identify });
            _MenuCommands.Add(ConsoleKey.W, new MenuCommandPair() { Name = "Weapons", Command = _Weapons });
            _MenuCommands.Add(ConsoleKey.S, new MenuCommandPair() { Name = "Scribe Materials", Command = _Scribe });
            _InventoryCommands.Add(ConsoleKey.B, new InvCommandPair() { Name = "Buy", Command = _Buy });



        }
        public override bool Exit()
        {
            int randExit = StaticRandom.Instance.Next(0, 5);
            switch (randExit)
            {
                case 0: Console.WriteLine("Goodbye traveler!"); return true;
                case 1: Console.WriteLine("Follow the yellow brick road!"); return true;
                case 2: Console.WriteLine("May your days be merry and bright!"); return true;
                case 3: Console.WriteLine("Don't eat toilet mints!"); return true;
                case 4: Console.WriteLine("You'll never win!"); return true;
            }
            return true;
            
        }
        protected virtual bool _Weapons()
        {
            int b = 0;
            while (b != 1)
            {
                Console.Clear();

                Console.WriteLine("Shop of " + Name);
                Console.WriteLine("Chose a weapon to purchase.\n\n");
                Console.WriteLine("Escape) to exit.\n1) Gnarly Staff 500gp \n2) Arcane Wand 1500gp \n3) Demon Staff 2000gp \n4) Jebus Wand 5000gp \n5) Shiny Staff 5000gp");

                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape) { return true; }

                if (key.Key == ConsoleKey.D1) //Option 1 choice
                {
                    if (Player.Money < 500) { Console.WriteLine("You need more money."); Console.ReadLine(); }
                    else
                    {
                        bool missile = Player.AddItem((new Item() { Name = "Gnarly Staff", Weight = 5, Durability = 10, Identified = true, Effects = new List<EffectParameter> { new EffectParameter() { Type = "STAFF", Power = 5, Description = "Californians really like this weapon." } } }));
                        Console.Write("\nYou purchased a Gnarly Staff. \nPress enter to continue. "); Console.ReadLine();
                        Player.Money -= 500;
                    }
                }

                if (key.Key == ConsoleKey.D2) //Option 2 choice
                {
                    if (Player.Money < 1500) { Console.WriteLine("You need more money."); Console.ReadLine(); }
                    else
                    {
                        bool missile = Player.AddItem((new Item() { Name = "Arcane Wand", Weight = 3, Durability = 15, Identified = true, Effects = new List<EffectParameter> { new EffectParameter() { Type = "WAND", Power = 7, Description = "This is possibly the most boring wand." } } }));
                        Console.Write("\nYou purchased the Arcane Wand. \nPress enter to continue. "); Console.ReadLine();
                        Player.Money -= 1500;
                    }
                }

                if (key.Key == ConsoleKey.D3) //Option 3 choice
                {
                    if (Player.Money < 2000) { Console.WriteLine("You need more money."); Console.ReadLine(); }
                    else
                    {
                        bool missile = Player.AddItem((new Item() { Name = "Demon Staff", Weight = 6, Durability = 15, Identified = true, Effects = new List<EffectParameter> { new EffectParameter() { Type = "STAFF", Power = 9, Description = "When this is equipped in Jamaica you're just the man." } } }));
                        Console.Write("\nYou purchased the Demon Staff. \nPress enter to continue. "); Console.ReadLine();
                        Player.Money -= 2000;
                    }
                }

                if (key.Key == ConsoleKey.D4) //Option 4 choice
                {
                    if (Player.Money < 5000) { Console.WriteLine("You need more money."); Console.ReadLine(); }
                    else
                    {
                        bool missile = Player.AddItem((new Item() { Name = "Jebus Wand", Weight = 5, Durability = 45, Identified = true, Effects = new List<EffectParameter> { new EffectParameter() { Type = "WAND", Power = 12, Description = "This wand hits like a church bus." } } }));
                        Console.Write("\nYou purchased the Jebus Wand. \nPress enter to continue. "); Console.ReadLine();
                        Player.Money -= 5000;
                    }
                }

                if (key.Key == ConsoleKey.D5) //Option 5 choice
                {
                    if (Player.Money < 5000) { Console.WriteLine("You need more money."); Console.ReadLine(); }
                    else
                    {
                        bool missile = Player.AddItem((new Item() { Name = "Shiny Staff", Weight = 8, Durability = 30, Identified = true, Effects = new List<EffectParameter> { new EffectParameter() { Type = "STAFF", Power = 15, Description = "This staff will blind your foes." } } }));
                        Console.Write("\nYou purchased the Shiny Staff. \nPress enter to continue. "); Console.ReadLine();
                        Player.Money -= 5000;
                    }
                }
            }
            return false;
        }
        protected virtual bool _Scribe()
        {
            int b = 0;
            while (b != 1)
            {
                Console.Clear();

                Console.WriteLine("Shop of " + Name);
                Console.WriteLine("Chose a scroll to purchase.\n\n");
                Console.WriteLine("Escape) to exit.\n1) Magic Missile 50gp \n2) Town Portal 100gp \n3) Teleport 50gp \n4) Identify 30gp");

                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Escape) { return true; }
                if (key.Key == ConsoleKey.D1) //Option 1 choice
                {
                    if (Player.Money < 50) { Console.WriteLine("You need more money."); Console.ReadLine(); }
                    else
                    {
                        bool missile = Player.AddItem((new Item() { Name = "Magic Missile Scroll", Weight = 1, Durability = 1, Identified = true, Effects = new List<EffectParameter> { new EffectParameter() { Type = "MMIS", Power = 2, Duration = 1, Description = "Shoots a magic missile." } } }));
                        Console.Write("\nYou purchased a Magic Missile Scroll. \nPress enter to continue. "); Console.ReadLine();
                        Player.Money -= 50;
                    }
                }

                if (key.Key == ConsoleKey.D2) //Option 2 choice
                {
                    if (Player.Money < 100) { Console.WriteLine("You need more money."); Console.ReadLine(); }
                    else
                    {
                        bool town = Player.AddItem((new Item() { Name = "Town Portal", Weight = 1, Durability = 1, Identified = true, Effects = new List<EffectParameter> { new EffectParameter() { Type = "TOWN", Power = 1, Duration = 1, Description = "Teleports you back to town." } } }));
                        Console.Write("\nYou purchased a Town Portal. \nPress enter to continue. "); Console.ReadLine();
                        Player.Money -= 100;
                    }
                }

                if (key.Key == ConsoleKey.D3) //Option 3 choice
                {
                    if (Player.Money < 50) { Console.WriteLine("You need more money."); Console.ReadLine(); }
                    else
                    {
                        bool teleport = Player.AddItem((new Item() { Name = "Teleport", Weight = 1, Durability = 1, Identified = true, Effects = new List<EffectParameter> { new EffectParameter() { Type = "TELE", Power = 3, Duration = 1, Description = "Teleports you to a random spot on the map." } } }));
                        Console.Write("\nYou purchased Teleport. \nPress enter to continue. "); Console.ReadLine();
                        Player.Money -= 50;
                    }
                }

                if (key.Key == ConsoleKey.D4) //Option 4 choice
                {
                    if (Player.Money < 30) { Console.WriteLine("You need more money."); Console.ReadLine(); }
                    else
                    {
                        bool identify = Player.AddItem((new Item() { Name = "Identify", Weight = 1, Durability = 1, Identified = true, Effects = new List<EffectParameter> { new EffectParameter() { Type = "IDEN", Power = 5, Duration = 1, Description = "Identify an item." } } }));
                        Console.Write("\nYou purchased Teleport. \nPress enter to continue. "); Console.ReadLine();
                        Player.Money -= 30;
                    }
                }
            }
            return false;
        }

        protected virtual bool _Identify()
        {
            Item ident;
            Console.Clear();
            Console.WriteLine("Identify an item: ");
            if (_SelectPlayerItem(out ident))
            {
                if (ident.Effects.Count > 0)
                {
                    Console.Clear();
                    Console.WriteLine(ident.View());
                    foreach (EffectParameter E in ident.Effects)
                    {
                        Console.WriteLine("\nPower: " + E.Power
                            + "\tRange: " + E.Range
                            + "\tDuration: " + E.Duration
                            + "\nDescription: " + E.Description
                            + "\nType: " + E.Type + " "
                            );
                    }
                    ident.Identified = true;
                }
                else Console.WriteLine("\nThis item does not have any magic effects");
                Console.WriteLine("\nPress enter to continue.");
                Console.ReadKey();

            }

            return true;
        }
    }
}