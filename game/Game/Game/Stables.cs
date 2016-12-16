using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Stables : BuildingLocation
    {
        public Creature StabledMount { get; set; }//Stable slot
        List<Creature> Mounts = new List<Creature>();

        protected override void Build()
        {
            base.Build();
            Name = Owner.Name + " Stables";
            TextColor = ConsoleColor.DarkYellow;
            BackColor = ConsoleColor.Gray;
            //Generate Horses
            for (int i = 0; i < 7; i++)
            {
                Mounts.Add(new Character()
                {
                    Name = "Horse" + (i + 1).ToString(),
                    Strength = DieRoller.roll(5, 6),
                    Dexterity = DieRoller.roll(5, 6),
                    Constitution = DieRoller.roll(5, 6),
                    Level = DieRoller.roll(1, 3)
                });
            }

            Inventory.AddItem(new Item() { Name = "Saddle", Weight = 10,  Cost = 30, Type = "MOUNT" });           

            _InventoryCommands.Add(ConsoleKey.B, new InvCommandPair() { Name = "Buy Item", Command = _Buy });

            _MenuCommands.Add(ConsoleKey.V, new MenuCommandPair() { Name = "View Items", Command = _InventoryController });
            _MenuCommands.Add(ConsoleKey.B, new MenuCommandPair() { Name = "Buy Horse", Command = _BuyMount });
            _MenuCommands.Add(ConsoleKey.S, new MenuCommandPair() { Name = "Stable Mount", Command = _StableMount });
            _MenuCommands.Add(ConsoleKey.R, new MenuCommandPair() { Name = "Retrieve Mount", Command = _GetMount });
        }

        public override bool Exit()
        {
            Console.WriteLine("Fastest Mounts Around! Spread the word!"); return true;
        }

        protected override void _ViewInventory()
        {
            int n = 1;
            foreach (EquipItem i in Inventory.Contents) //display inventory
            {
                string str = String.Format("{0,-25}{1,3}  {2,20}", i.Name, i.Type, "\tDam:" + i.Base + "+" + i.Power + "\tDur:" + i.Durability);
                Console.WriteLine(n.ToString() + ") " + str);
                n += 1;
            }
        }
        
        protected override void _ShowBuildingStats()
        {//display relevant player stats
            string str = String.Format("{0,-36}{1,15}  ", Owner.Name, "Money:" + Owner.Money + "\t");
            Console.ForegroundColor = TextColor; Console.BackgroundColor = BackColor;
            Console.Write(str);

            //Write Stabled Status
            if (StabledMount == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0,40}  ", "No Mount Stabled."); }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("{0,40}  ", StabledMount.Name+" stabled");
            }

            Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black;
        }

        protected virtual bool _BuyMount()
        {
            Console.Clear();
            int c = 1;
            List<int> costs = new List<int>(); //stores costs as we calculate them for easy retrieval later.
            foreach (Character Mount in Mounts)
            {
                int cost = (Mount.Strength + Mount.Dexterity + Mount.Constitution) * Mount.Level * 2;
                costs.Add(cost);
                string x = String.Format("{0,-35}  {1,20}", c + ") " + Mount.Name, "\tLvl:" + Mount.Level + "\tStr:" + Mount.Strength + "\tDex:" + Mount.Dexterity + "\tCon:" + Mount.Constitution + "\tCost:" + cost);
                Console.WriteLine(x);
                c++;
            }
            while (true)
            {
                Console.WriteLine("\nWhich horse would you like to buy?:");
                ConsoleKeyInfo k = Console.ReadKey(); Console.CursorLeft = 0; //get user input
                if (k.Key == ConsoleKey.Escape) { return false; }
                int choice;
                if (Int32.TryParse(char.ToUpper(k.KeyChar).ToString(), out choice))
                {
                    if (Player.Money < costs[choice - 1])
                    {
                        Console.WriteLine("Not enough money!"); Console.ReadKey(); continue;
                    }
                    if (Player.Mount != null)
                    {
                        Console.WriteLine("\nYou currently have a Horse. Dismiss current Horse (Y/N)?: ");
                        ConsoleKeyInfo k2 = Console.ReadKey(); Console.CursorLeft = 0; //get user input
                        if (k2.Key == ConsoleKey.N) { return false; }
                    }

                    Player.Mount = Mounts[choice - 1];
                    Mounts.RemoveAt(choice - 1);
                    Player.Money -= costs[choice - 1];
                    costs.RemoveAt(choice - 1);
                    return true;
                }
            }
        }
        
        protected virtual bool _StableMount()//can ony have one mount stabled at a time
        {
            if (Player.Money < 30)
            {
                Console.WriteLine("Not enough money! Need 30gp to stable a horse");
                Console.ReadKey();
                return false;
            }
            if (Player.Mount == null)
            {
                Console.WriteLine("You do not currently have a mount to stable.");
                Console.ReadKey();
                return false;
            }
            while (true)
            {
                Console.WriteLine("\nWould you like to stable your horse?- 30gp  (Y/N):");
                ConsoleKeyInfo k = Console.ReadKey(); Console.CursorLeft = 0; //get user input
                if (k.Key == ConsoleKey.Escape || k.Key == ConsoleKey.N) { return false; }
                if (k.Key == ConsoleKey.Y)
                {
                    if (StabledMount != null) {
                        Console.WriteLine("\nYou currently have a mount stabled. Replace it?  (Y/N):");
                        ConsoleKeyInfo k2 = Console.ReadKey(); Console.CursorLeft = 0; //get user input
                        if (k2.Key == ConsoleKey.Escape || k.Key == ConsoleKey.N) { return false; }
                    }


                    Player.Money -= 30;
                    StabledMount = Player.Mount;
                    Player.Mount = null;
                    Console.WriteLine("We'll take good care of her!"); Console.ReadKey();
                    return true;
                }
            }
        }

        protected virtual bool _GetMount()
        {
            
            if (StabledMount == null)
            {
                Console.WriteLine("You do not currently have a mount stabled to retrieve.");
                Console.ReadKey();
                return false;
            }


            Creature temp = new Creature();
            temp = StabledMount;
            StabledMount = Player.Mount;
            Player.Mount = temp;

            Console.WriteLine("Mount retrieved from stables"); Console.ReadKey();
            return true;
             
            }

        protected override bool _SelectPlayerItem(out Item i)
        {
            int n = 1;

            foreach (Item item in Player.Inventory.Contents) //display inventory
            {
                Console.WriteLine(n.ToString() + ") " + item.Name + "\tDurability: " + item.Durability);
                n += 1;
            }
            if (n == 1) { Console.Write("Inventory Empty!"); Console.ReadKey(); i = new Item(); return false; }
            try
            { //get input, try to parse into Int. Try to assign i to the selected inventory item
                ConsoleKeyInfo k = Console.ReadKey(); Console.CursorLeft = 0; //get user input
                int choice = Int32.Parse(char.ToUpper(k.KeyChar).ToString());
                i = Player.Inventory.Contents[choice - 1];
                return true;
            }
            catch { i = new Item(); return false; }//something didn't work. 
        }
    }
}
