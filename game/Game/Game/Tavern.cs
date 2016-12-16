using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Tavern : BuildingLocation
    {
        List<Character> Occupants = new List<Character>();

        protected override void Build()
        {
            base.Build();
            Name = CreatureBuilder.GetName() + "'s Tavern";
            TextColor = ConsoleColor.DarkCyan;
            BackColor = ConsoleColor.Gray;
            List<Item> tempInv = new List<Item>
            {
                new FoodItem() { Name = "Grog", Weight = 1, Cost = 3, Rarity = 1},
                new FoodItem() { Name = "Apple", Weight = 1, Cost = 1, FoodPower = 2, Rarity = 1 },
                new Item() { Name = "Bar Keeps's Last Call", Weight = 5, Cost = 500, Power = 9000, Type = "Tavern Owner's favorite house cleaning club", Rarity = 10 },
                new Item() { Name = "Sturdy Chair", Weight = 3, Cost = 50, Power = 30, Type = "Just Savage....", Rarity = 5 },
                new FoodItem() { Name = "Beer", Weight = 1, Cost = 10, FoodPower = -5, Rarity = 2 },
                new FoodItem() { Name = "Vodka", Weight = 1, Cost = 15, FoodPower = 2, Rarity = 2 },
                new FoodItem() { Name = "Wine", Weight = 1, Cost = 20, FoodPower = 5, Rarity = 3 },
                new FoodItem() { Name = "Cheese Wheel", Weight = 2, Cost = 25, FoodPower = 10, Rarity = 4 },
                new Item() { Name = "Bar Keep's Beard", Weight = 0, Cost = 100, Power = 0, Type = "It looks warm...", Rarity = 6 },
                new FoodItem() { Name = "Mutton", Weight = 4, Cost = 30, FoodPower = 30, Rarity = 5 },
                new FoodItem() { Name = "Bread", Weight = 2, Cost = 8, FoodPower = 12, Rarity = 2 },
                new FoodItem() { Name = "Rum", Weight = 1, Cost = 50, FoodPower = 17, Rarity = 4 },
                new FoodItem() { Name = "Beef", Weight = 2, Cost = 20, FoodPower = 20, Rarity = 1 },
                new FoodItem() { Name = "Pork", Weight = 3, Cost = 16, FoodPower = 25, Rarity = 2 },
                new FoodItem() { Name = "Special Potatoes", Weight = 5, Cost = 55, FoodPower = 55, Rarity = 9 },
                new FoodItem() { Name = "Onions", Weight = 1, Cost = 2, FoodPower = 3, Rarity = 1 },
                new FoodItem() { Name = "Stale Bread", Weight = 1, Cost = 3, FoodPower = 4, Rarity = 1 },
                new FoodItem() { Name = "Tomatoes", Weight = 1, Cost = 5, FoodPower = 3, Rarity = 1 },
                new FoodItem() { Name = "Beans", Weight = 1, Cost = 2, FoodPower = 1, Rarity = 1 },
                new FoodItem() { Name = "Eggs", Weight = 1, Cost = 20, FoodPower = 60, Rarity = 8 }
            };
            //Inventory.AddItem()
            var items = from I in tempInv where I.Rarity <= (DieRoller.roll(6) + PlayerCharacter.Instance.Luck) orderby I.Rarity descending select I;
            int itemCounter = 0;
            foreach (Item i in items)
            {
                if (itemCounter < 8)
                {
                    Inventory.AddItem(i);
                    itemCounter++;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Occupants.Add(new Character()
                {
                    Name = CreatureBuilder.GetName(),
                    Level = DieRoller.roll(20),
                });
            }

            _MenuCommands.Add(ConsoleKey.O, new MenuCommandPair() { Name = "View Occupants", Command = _ViewOccupants });
            _MenuCommands.Add(ConsoleKey.V, new MenuCommandPair() { Name = "View Items", Command = _InventoryController });
            _InventoryCommands.Add(ConsoleKey.B, new InvCommandPair() { Name = "Buy Item", Command = _Buy });
            _InventoryCommands.Add(ConsoleKey.N, new InvCommandPair() { Name = "Eat/Drink Item", Command = _Consume });
        }
        protected virtual bool _Consume(Item c)
        {
            if (c.Cost > Player.Money)
            { Console.WriteLine("Go GRIND ye broke hobo!"); Console.ReadKey(); return false; }
            Player.Money -= c.Cost;
            try {
                FoodItem nom = (FoodItem)c;
                Player.Consume(nom);
                Item item = Inventory.Contents.SingleOrDefault(x => x.ID == c.ID);
                if (item != null)
                    Inventory.Contents.Remove(item);
                return true;
            }
            catch { Console.WriteLine("Thats not possible... you might need more pylons"); Console.ReadKey(); return false; }
        }
        protected virtual bool _ViewOccupants()
        {
            foreach(Character Occupant in Occupants)
            {
                Console.WriteLine("Name: " + Occupant.Name + "\tLvl:" + Occupant.Level);
            }
            Console.ReadKey();
            return true;
        }
        public override bool Exit()
        {
            Console.WriteLine("Maybe next time you'll learn tohold your liqour hahaha!"); return true;
        }
        public override bool Enter(PlayerCharacter p)
        {
            return base.Enter(p);
        }
        protected override void _ShowBuildingStats()
        {//display relevant building stats. Override if you need different stats to display at top.
            string str = String.Format("{0,-35}{1,15} {2,48} ", Owner.Name, "Occupants:" + Occupants.Count, "");
            Console.ForegroundColor = TextColor; Console.BackgroundColor = BackColor;
            Console.Write(str);
            Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
