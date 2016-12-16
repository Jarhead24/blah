using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// do people use javascript object format this way in C#
// the reason i ask is VS keeps auto formatting it 
// placing the opening bracket on a newline by itself
// been doing a lot of Javascript in my freetime
// so i'm just used to doing objects like this

namespace Game
{
    class WizardTower : BuildingLocation
    {
        // objectify the effects
        public EffectParameter F = new EffectParameter() {
            ID = "WIZ_1",
            Name = "Scorching Flame",
            Description = "Causes burn damage for 5000 milliseconds", // just a description, no implementation
            Duration = 100, // set to same amount as cost to scale among different enchantments
            Power = 2,    // set to floor(10% of the required rep)
          //Range =   // left Range at default as I don't know what it is
            Type = "FIRE", 
            Color = ConsoleColor.Red
        };
        public EffectParameter I = new EffectParameter() {
            ID = "WIZ_1",
            Name = "Glaciating Mistral",
            Description = "A cold, dry, northerly wind that causes enemies to become frozen for 4200 milliseconds.",
            Duration = 200, // does duration mean how many times the enchanted item can be used before enchantment fades?
            Power = 4,
            Type = "ICE",
            Color = ConsoleColor.Blue
        };
        public EffectParameter L = new EffectParameter() {
            ID = "WIZ_1",
            Name = "Exsanguinating Stinger",
            Description = "Establish a connection to drain your enemies life force and pump it into yourself",
            Duration = 500,
            Power = 5,
            Type = "LEECH",
            Color = ConsoleColor.DarkGreen
        };

        // list for farewell messages
        public List<string> Farewells = new List<string>();

        // Command list for Enchantment Menu
        protected SortedDictionary<ConsoleKey, ServiceValuePair> _EnchantCommands = new SortedDictionary<ConsoleKey, ServiceValuePair>();
        
        

        protected override void Build()
        {
            base.Build();
            // changed the reputation
            // it will go up a little bit after purchasing one enchantment
            // then you will be able to view the second enchantment
            _Reputation = 44;


            // Initialize class variable for the name of this building
            Name = Owner.Name + "'s Wizard Tower";

            // Change text color to Magenta
            TextColor = ConsoleColor.Magenta;

            // Add enchant behavior for Enchanment Effects to EnchantCommand
            _EnchantCommands.Add(ConsoleKey.F, new ServiceValuePair() { // FIRE
                Name = F.Name,
                Cost = 100,
                RepThreshold = 25,
                Effect = F
            });

            _EnchantCommands.Add(ConsoleKey.I, new ServiceValuePair() { // ICE
                Name = I.Name,
                Cost = 200,
                RepThreshold = 45,
                Effect = I
            });

            _EnchantCommands.Add(ConsoleKey.L, new ServiceValuePair() { // Leech
                Name = L.Name,
                Cost = 500,
                RepThreshold = 75,
                Effect = L
            });
            
            // menu command to select an item from players inventory to be enchanted
            _MenuCommands.Add(ConsoleKey.M, new MenuCommandPair() {
                Name = "Select Item to Enchant",
                Command = _ChooseItem4enchantingPorpoises
            });

            Farewells.Add("Have a swell day!");
            Farewells.Add("Please never return!");
            Farewells.Add("Enchantments are for weaklings!");
            Farewells.Add("Thanks for stopping in, smoothskin.");
            Farewells.Add("Big dummy, read a book.");
            Farewells.Add("Dr. Beverly Crusher for Surgeon General!");
       
            
        }

        // our main diddy for selecting an item and enchanting it kind of like _InventoryConroller
        protected bool _ChooseItem4enchantingPorpoises()
        {
            // FIRST prompt user to select an item to enchant
            Item i; // placeholder Item dataype variable
            while (true)
            {
                Console.Clear();
                _ShowPlayerStats();
                _ShowBuildingStats();
                Console.WriteLine("\n" + Player.Name + "'s items:\n");
                if (_SelectPlayerItem(out i) == true) // if SelectPlayerItem returns true, we can continue
                {
                    if (_EnchantmentCommandment(i)) { return true; } // if returns true here, we are done acting on the item
                }
                else { break; } // if SelectPlayerItem returns false, break out of the loop and start over at Building's main menu screen
            }
            return false;
        }
        // Command screen for selecting an enchantment
        protected virtual bool _EnchantmentCommandment(Item i) // i is the item to be imbued with magical paprikas
        {
            while (true)
            {
                Console.Clear();
                _ShowPlayerStats();
                _ShowBuildingStats();

                // Display the item player selected to be enchanted
                Console.Write("\nYou selected your <");
                Console.ForegroundColor = ConsoleColor.Blue; Console.Write(i.Name);
                Console.ForegroundColor = ConsoleColor.White; Console.Write(">\t Street Value:" + i.Cost + "\n\n");

                // Display commands from the _EnchantCommands Dictionary
                // there arent really any enchant commands
                foreach (KeyValuePair<ConsoleKey, ServiceValuePair> Enchantment in _EnchantCommands)
                {
                    // check the player's reputation first. some magic REQUIRES REMARKABLE RAPPORT
                    if (_Reputation >= Enchantment.Value.RepThreshold)
                    { Console.WriteLine("{0}>\t {1}", Enchantment.Key, Enchantment.Value.Name); }
                }

                // just gonna shove this in here since it is technically
                // already in the buildings InvCommand dictionary by default
                ConsoleKey theKey = ConsoleKey.Escape; 
                InvCommandPair theExit = _InventoryCommands[theKey];

                Console.WriteLine("{0}>\t {1}", theKey, "Exit");

                // get user input
                ConsoleKeyInfo k = Console.ReadKey(); Console.CursorLeft = 0;

                // jacked in exit capability
                if (_InventoryCommands.ContainsKey(k.Key))
                { if (_InventoryCommands[k.Key].Command(i)) { return true; } }//exit successfully


                // if enchantment in _EnchantCommands dictionary, apply it to weapon
                // these aren't really commands. should still return true so player can exit the inventory
                // our 'command' is the _Enchant method, which will return true when finished

                if (_EnchantCommands.ContainsKey(k.Key))
                {
                    
                    if (_Buy(i, _EnchantCommands[k.Key])) { return true; }
                }
                Console.ReadKey();
            }
        }
        protected virtual bool _Buy(Item i, ServiceValuePair k) // i is the item required new magicks, k is the key to the Dictionary _EC
        {
            // check if player can afford the enchantment
            if(k.Cost>Player.Money)
            { Console.WriteLine("Not enough money. Master Wayne has drank the family's wine collection."); Console.ReadKey(); return false; }

            // check to make sure the item is not already enchanted
           // if(i.Effects.Count > 0) // not sure what you meant by checking the ID
                                    // get and out of range exception error this probably wouldnt work long term
                                    // as the weapon may be able to have other types of effects applied to it
                                    // I feel like this check should be done elsewhere. sooner in fact.

            // check for an enchantment from this building
            foreach (EffectParameter E in i.Effects)
            {
                if(E.ID == "WIZ_1")
                { Console.WriteLine("This item is already enchanted, " + Player.Name + ". Choose a different item to enchant."); Console.ReadKey(); return false; }
            }

            
            

            string oldName = i.Name;
            int oldPower = i.Power;

            if (_Enchant(i, k.Effect)) // if the item is successfully enchanted
            {
                Player.Money -= k.Cost; // subtract the cost of the enchantment from your wallet
                // maybe clear a new screen here?
                Console.WriteLine("\nYour {0} is successfully enchanted. With the {1} enchantment.\nNow it is called the {2}.",oldName, i.Effects[0].Name, i.Name);
                Console.WriteLine("Previous Power: " + oldPower);
                Console.WriteLine("New Power:\t" + i.Power);
                Console.WriteLine("\nThanks for the {0} HUMAN DOLLHAIRS",k.Cost);
                Console.ReadKey();

                return true; // return turn because we are done with actions

                ////(** Enchantments can't be consumed////////////////
                // > items are enchanted by great beings            //
                // > the being will use his or her innate           //
                // > magical abilities to imbue magic               //
                // > into any item you possess.                     //
                // > they can do this almost indefinitely           //
                // > some enchantments are more taxing              //
                // > ie. a ship is harder to build than toy mouse   //
                // > that's why some cost more than others          //
                //////////////////////////////////////////////////////
            }
            return true;
        }
        // this is similar _takeitem, which actually affects the players inventory this is where we show some confirmation to the player asswell
        protected virtual bool _Enchant(Item i, EffectParameter lotus ) // i is the Item to be magicified, lotus is the magic to be applied 
        {

            // we can do SO MUCH with lotus!!! objects are the best

            
            Console.WriteLine("You want to add the {0} enchantment to your {1}.\n", lotus.Name, i.Name);
                                                            Console.Write("It is a ");
            Console.ForegroundColor = lotus.Color;          Console.Write(lotus.Type);
            Console.ForegroundColor = ConsoleColor.White;   Console.WriteLine(" enchantment.");
            Console.WriteLine("Description:\n"+lotus.Description);

            Console.ReadKey();
            try
            { // print error messages if necessary

                // check if item is already enchanted

                // Player.AddItem is the star in the _TakeItem method
                // the meat and potatoes of this _Enchant method is as follows
                // if i wrote this code from scratch myself, i would make a method in the item class for "ADD EFFECT"

                i.Effects.Add(lotus); // all that work to just do this one line of code

                i.Name = lotus.Type +" "+ i.Name;
                i.Power += lotus.Power;
                // thought itd be good to increase reputation on enchanting since we are giving this enchanter money
                _Reputation += (lotus.Duration / 100) + 1;


                return true;
            }
            catch { return false; }
        }
        // override this method to show player's reputation with this store
        protected override void _ShowBuildingStats()
        {   // come back and fix this. alignment is a bit off
            // and the reputation text color is affecting the entire line
         
            string str = String.Format("{0,-35}{1,18}{2,47}", Owner.Name, "Repuation:"+_ReputationStatus().Item2,"");
            Console.ForegroundColor = _ReputationStatus().Item1; Console.BackgroundColor = BackColor;
            Console.Write(str);
            Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black;
        }
        // overide the Exit method to show our random farewell
        public override bool Exit()
        {
            int goodbyeNum = Farewells.Count;
            int goodbyeRnd = DieRoller.roll(goodbyeNum);

            Console.WriteLine(Farewells[goodbyeRnd]);

            return true;
        }
    }
}