using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Blacksmith : BuildingLocation
    {

        protected override void Build()
        {
            base.Build();

            Name = Owner.Name + "'s Smithy";
            TextColor = ConsoleColor.Red;


            Inventory.AddItem(new EquipItem() { Name = "CHAIN", Weight = 6, Base = 4, Cost = 10, Type = "CHAIN" });
            Inventory.AddItem(new EquipItem() { Name = "Long Sword", Weight = 6, Base = 4, Cost = 10, Type = "SWORD" });
            Inventory.AddItem(new EquipItem() { Name = "Great Axe", Weight = 4, Base = 3, Cost = 7, Type = "AXE", Durability = 40 });
            Inventory.AddItem(new EquipItem() { Name = "Dagger", Weight = 1, Base = 3, Cost = 15, Type = "KNIFE", Power = 1 });
            Inventory.AddItem(new EquipItem() { Name = "Bell", Weight = 1, Cost = 3, Type = "BELL" });
            //Inventory.AddItem(new EquipItem() { Name = "Sickle", Weight = 6, Base = 4, Cost = 3, Type = "KNIFE", Durability = 20 });
            //Inventory.AddItem(new EquipItem() { Name = "Shield", Weight = 9,  Cost = 16, Type = "SHIELD" });



            _InventoryCommands.Add(ConsoleKey.B, new InvCommandPair() { Name = "Buy Item", Command = _Buy });

            _MenuCommands.Add(ConsoleKey.V, new MenuCommandPair() { Name = "View Items", Command = _InventoryController });
            _MenuCommands.Add(ConsoleKey.R, new MenuCommandPair() { Name = "Repair Item", Command = _Repair });
            _MenuCommands.Add(ConsoleKey.S, new MenuCommandPair() { Name = "Sharpen Item", Command = _Sharpen });

        }

        public override bool Exit()
        {
            
            string[] Exit =
                { "Goodbye!", "See you later!", "Generic exit message!" ,"I can't think of anything clever to say here."};
            
            int exitList = StaticRandom.Instance.Next(0, Exit.Length);
            string exitMessage = Exit[exitList];
            Console.WriteLine(exitMessage);
           return bool.Parse(exitMessage); //i don't know how to fix this 
            //Console.WriteLine("Goodbye Message!");
            

        }

        protected override void _ViewInventory()
        {
            int n = 1;
            foreach (EquipItem i in Inventory.Contents) 
            {
                string str = String.Format("{0,-25}{1,3}  {2,20}", i.Name, i.Type, "\tDam:" + i.Base + "+" + i.Power + "\tDur:" + i.Durability);
                Console.WriteLine(n.ToString() + ") " + str);
                n += 1;
            }
        }
        protected override void _ShowPlayerStats()
        {
            string str = String.Format("{0,-35}{1,15}  {2,20}", Player.Name, "Money:" + Player.Money, "Weight:" + Player.Weight + "/" + (Player.Strength * 10));
            Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine(str); Console.ForegroundColor = ConsoleColor.White;
        }

        protected virtual bool _Repair()
        { 
            Item i;
            if (_SelectPlayerItem(out i) == true)
            {
                int choice;
                while (true)
                {
                    Console.WriteLine("Repairs cost 2gp per point, enter an amount to repair.");
                    if (Int32.TryParse(Console.ReadLine(), out choice))
                    {
                        if (choice <= 0) { return true; }
                        if (choice + i.Durability > 100) { choice = 100 - i.Durability; } 
                        if (choice * 2 > Player.Money) { Console.WriteLine("INSUFFICIENT FUNDS!"); Console.ReadKey(); continue; }
                    }
                    else
                    {
                        Console.WriteLine("Select number keys only."); continue;
                    }
                    break;
                }
                Console.WriteLine("Just like new.");
                i.Durability += choice;
                Player.Money -= (choice * 2);
                Console.ReadKey();
            }
            return false;
        }

        protected virtual bool _Sharpen()
        {
            Item i;

            if (Player.Money < 20)
            {
                Console.WriteLine("INSUFFICIENT FUNDS!");
                Console.ReadKey();
                return false;
            }
            while (true)
            {
                Console.WriteLine("\nSelect item to Sharpen (20gp):");
                if (_SelectPlayerItem(out i))
                {
                    if (i.Type != "SWORD" && i.Type != "AXE")
                    {
                        Console.WriteLine("You can only sharpen SWORDS and AXES. Make another selection! "); Console.ReadKey(); break;
                    }

                    EffectParameter E2 = i.Effects.FirstOrDefault(x => x.ID == "SHARPEN");
                    if (E2 == null)
                    {
                        i.Effects.Add(new EffectParameter() { ID = "SHARPEN", Type = "PHYS", Power = 1, Description = "Weapon Sharpeneed" });

                        Player.Money -= 20;
                        Console.WriteLine("Sharp enough to kill a man."); Console.ReadKey();
                        return true;
                    }
                    Console.WriteLine("You Sharpen again."); Console.ReadKey(); return true;
                }
            }
            return false;
        }


        protected override bool _SelectPlayerItem(out Item i)
        {
            int n = 1;

            foreach (Item item in Player.Inventory.Contents)
            {
                Console.WriteLine(n.ToString() + ") " + item.Name + "\tDurability: " + item.Durability);
                n += 1;
            }
            if (n == 1) { Console.Write("Inventory Empty!"); Console.ReadKey(); i = new Item(); return false; }
            try
            { 
                ConsoleKeyInfo k = Console.ReadKey(); Console.CursorLeft = 0;
                int choice = Int32.Parse(char.ToUpper(k.KeyChar).ToString());
                i = Player.Inventory.Contents[choice - 1];
                return true;
            }
            catch { i = new Item(); return false; } 
        }
    }
}

