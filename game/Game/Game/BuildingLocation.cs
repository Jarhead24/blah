using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    //Provides the basic functionality for Building Classes. Derive your building classes from here
    //Only method that you cannot override is _InventoryController. This helps each location have the same flow for their inventory management

    class BuildingLocation : ILocation
    {
        public string Name { get; set; } //required from ILocation
        public int money { get; } //BuildingLocation money available
        public Character Owner; //Owner of this Location
        public PlayerCharacter Player; //reference to the player who is visiting. Keeps us from needing to pass player everywhere
        public Container Inventory = new Container() { Size = 9 };//Container for Location.
        protected int _Reputation = 50; //50 is average. 0 is max hostile. 100 is max ally
        public ConsoleColor TextColor { get; set; } = ConsoleColor.Green;//Text Color for location
        public ConsoleColor BackColor { get; set; } = ConsoleColor.DarkGray;//back color for location

        public bool Leave;//class flag for leaving the location. Controls main while loop in Enter()

        //Commands lists for Inventory and Menu (Menu is usually displayed in Enter() Method )
        //InvCommandPair and MenuCommandPair are defined in Misc.cs  They are structs for holding some extra parameters in the dictionary values
        protected SortedDictionary<ConsoleKey, InvCommandPair> _InventoryCommands = new SortedDictionary<ConsoleKey, InvCommandPair>();
        protected SortedDictionary<ConsoleKey, MenuCommandPair> _MenuCommands = new SortedDictionary<ConsoleKey, MenuCommandPair>();

        public BuildingLocation()
        {
            //constructor calls Build by default. If you needed to do other stuff at construct time, override Build()
            Build();
        }

        protected virtual void Build()//initializes the class. Override this if you need extra commands, properties, etc
        {
            //Add Owner
            Owner = new Character() { Name = CreatureBuilder.GetName(), Money = 1000 };
            //Add default _InventoryController to MenuCommands            
            _MenuCommands.Add(ConsoleKey.Escape, new MenuCommandPair() { Name = "Exit", Command = () => Leave = true });

            //Add default examine and exit behavior for Inventory Items to InventoryCommands
            _InventoryCommands.Add(ConsoleKey.E, new InvCommandPair() { Name = "Examine", Command = _Examine });
            _InventoryCommands.Add(ConsoleKey.Escape, new InvCommandPair() { Name = "Exit", Command = (Item i) => true });

            //Add this in your derived classes to turn on Inventory Items for the location
            //_MenuCommands.Add(ConsoleKey.V, new MenuCommandPair() { Name = "View Items", Command = _InventoryController });

            //Add this in your derived classes to turn on buying functionality
            //_InventoryCommands.Add(ConsoleKey.B, new InvCommandPair() { Name = "Buy Item", Command = _Buy });     
        }

        //required for ILocation. Essentially, the starting point for the Class
        public virtual bool Enter(PlayerCharacter p)
        {
            Player = p;//capture current player when they enter
            Leave = false;//class flag for leaving the location
            while (Leave == false)
            {
                Console.Clear();
                _ShowPlayerStats();
                _ShowBuildingStats();
                Console.WriteLine("\nWelcome to " + Name);
                Console.WriteLine("What would you like to do?\n\n");

                //Display commands in MenuCommands Dictionary
                foreach (KeyValuePair<ConsoleKey, MenuCommandPair> Command in _MenuCommands)
                {
                    Console.WriteLine("{0})\t\t{1}", Command.Key, Command.Value.Name);
                }
                ConsoleKeyInfo k = Console.ReadKey(); Console.CursorLeft = 0;

                //if command in MenuCommand Dictionary, then call it. Commands which exit the shop should set Leave = true
                if (_MenuCommands.ContainsKey(k.Key))
                {
                    _MenuCommands[k.Key].Command();

                }
            }
            Exit(); Console.ReadKey();
            return true;
        }

        //required for ILocation. Exit the location. End point for the Class. 
        public virtual bool Exit()
        {
            Console.WriteLine("Please come again!");
            return true;
        }

        //main controller for when player enters inventory (View Items command). Must be turned on (see constructor comments)
        protected bool _InventoryController()
        {
            Item i;
            while (true)
            {
                Console.Clear();
                _ShowPlayerStats();
                _ShowBuildingStats();
                Console.WriteLine("\n" + Name + " items:\n");
                _ViewInventory();
                if (_SelectItem(out i) == true)//if SelectItem returns true, safe to continue. 
                {
                    if (_InventoryCommand(i)) { return true; } //if return true, we are done with actions                      
                }
                else { break; }//if SelectItem returns false, break and start over
            }
            return false;//controls nothing
        }

        //Inventory methods used by _InventoryController. Can override any of them if needed!
        protected virtual void _ShowPlayerStats()//Cyan is default
        {//display relevant player stats. Override if you need different stats to display at top.
            string str = String.Format("{0,-35}{1,15}  {2,20}", Player.Name, "Money:" + Player.Money, "HP:" + Player.HP + "/" + Player.MaxHP);
            Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine(str); Console.ForegroundColor = ConsoleColor.White;
        }
        protected virtual void _ShowBuildingStats()//Green is default
        {//display relevant building stats. Override if you need different stats to display at top.
            string str = String.Format("{0,-35}{1,15} {2,48} ", Owner.Name, "Money:" + Owner.Money, "");
            Console.ForegroundColor = TextColor; Console.BackgroundColor = BackColor;
            Console.Write(str);
            Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black;
        }
        protected virtual void _ViewInventory()
        {
            int n = 1;
            foreach (Item i in Inventory.Contents) //display inventory
            {
                Console.WriteLine(n.ToString() + ") " + i.Name);
                n += 1;
            }
        }
        protected virtual bool _SelectItem(out Item i)
        {//select Item from Building Contents
            try
            { //get input, try to parse into Int. Try to assign i to the selected inventory item
                ConsoleKeyInfo k = Console.ReadKey(); Console.CursorLeft = 0; //get user input
                int choice = Int32.Parse(char.ToUpper(k.KeyChar).ToString());
                i = Inventory.Contents[choice - 1];
                return true;
            }
            catch { i = new Item(); return false; }//something didn't work. 
        }
        protected virtual bool _InventoryCommand(Item i)
        {
            while (true)
            {
                Console.Clear();
                _ShowPlayerStats();
                _ShowBuildingStats();

                //Display currently viewed Item
                Console.Write("\nLooking at <");
                Console.ForegroundColor = ConsoleColor.Blue; Console.Write(i.Name);
                Console.ForegroundColor = ConsoleColor.White; Console.Write(">\t Cost:" + i.Cost + "\n\n");

                //Display commands in _InventoryCommands Dictionary
                foreach (KeyValuePair<ConsoleKey, InvCommandPair> Command in _InventoryCommands)
                { Console.WriteLine("{0})\t\t {1}", Command.Key, Command.Value.Name); }

                //get user input
                ConsoleKeyInfo k = Console.ReadKey(); Console.CursorLeft = 0;

                //if command in InventoryCommand Dictionary, then call it. Commands which exit the inventory should return true
                if (_InventoryCommands.ContainsKey(k.Key))
                { if (_InventoryCommands[k.Key].Command(i)) { return true; } }//exit successfully
                Console.ReadKey();
            }
        }

        protected virtual bool _SelectPlayerItem(out Item i)//Selects Item from Player. Needed for many tasks (repairing, enchanting, etc...)
        {
            int n = 1;
            foreach (Item item in Player.Inventory.Contents) //display inventory
            {
                Console.WriteLine(n.ToString() + ") " + item.Name);
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

        //building actions. Override if needed.
        protected virtual bool _Examine(Item i)
        {
            Console.WriteLine(i.View());
            return false;
        }
        protected virtual bool _TakeItem(Item i) //Take Item from Building Contents
        { //returns false if not a valid take. Returns true if Item  was taken       
            try
            {//Player.AddItem() does these checks, but doing them here gives a chance for error messages.
                if (i.Weight + Player.Weight > (Player.Strength * 10))
                { Console.WriteLine("Too heavy! Lighten your load some!"); Console.ReadKey(); return false; }

                if (Player.Inventory.Contents.Count >= Player.Inventory.Size)
                { Console.WriteLine("Your pack is full!"); Console.ReadKey(); return false; }

                Player.AddItem(i);

                //remove item from Location Inventory by ID (using LINQ!)
                Item item = Inventory.Contents.SingleOrDefault(x => x.ID == i.ID);
                if (item != null)
                    Inventory.Contents.Remove(item);
                return true; //return true because we are done with action (item is consumed)
            }
            catch { return false; }
        }
        protected virtual bool _Buy(Item i)//Buy Item from Building. Must be enabled in the command menu (see comment in constructor)
        { //returns false if not a valid buy. Returns true if Item is valid for purchase

            if (i.Cost > Player.Money)
            { Console.WriteLine("Not enough Money!"); Console.ReadKey(); return false; }

            if (_TakeItem(i))
            {
                Player.Money -= i.Cost; //subtract cost of item 
                return true; //return true because we are done with action (item is consumed)
            }
            return true;
        }

        //Used for Reputation display in some buildings. Not shown by default. 
        //override _ShowBuildingStats if you need it to display.
        protected virtual Tuple<ConsoleColor, string> _ReputationStatus()
        {
            if (_Reputation <= 10) { return new Tuple<ConsoleColor, string>(ConsoleColor.DarkRed, "HATE"); }
            else if (_Reputation <= 25) { return new Tuple<ConsoleColor, string>(ConsoleColor.Red, "Hostile"); }
            else if (_Reputation <= 40) { return new Tuple<ConsoleColor, string>(ConsoleColor.DarkBlue, "Wary"); }
            else if (_Reputation <= 60) { return new Tuple<ConsoleColor, string>(ConsoleColor.Yellow, "Neutral"); }
            else if (_Reputation <= 75) { return new Tuple<ConsoleColor, string>(ConsoleColor.DarkGreen, "Like"); }
            else if (_Reputation <= 90) { return new Tuple<ConsoleColor, string>(ConsoleColor.Green, "Ally"); }
            else { return new Tuple<ConsoleColor, string>(ConsoleColor.Blue, "LOVE"); }
        }
    }

}