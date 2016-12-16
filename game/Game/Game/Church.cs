using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Church : BuildingLocation
    {
        private static string GenerateName()
        {
            string[] churchFirst =
            {
                "Church Of"
            };

            string[] churchLast =
            {
                "BridgeValley","Scientology","God","Christ","Satan"
            };


            int firstChurchIndex = StaticRandom.Instance.Next(0, churchFirst.Length);
            int lastChurchIndex = StaticRandom.Instance.Next(0, churchLast.Length);

            string name = churchFirst[firstChurchIndex] + " " + churchLast[lastChurchIndex];

            return name;
        }


        protected override void Build()
        {

            base.Build();
            Name = GenerateName();
            TextColor = ConsoleColor.White;

            Inventory.AddItem(new Item() { Name = "Holy Water" });
            Inventory.AddItem(new Item() { Name = "Bread" });
            Inventory.AddItem(new Item() { Name = "Wooden Cross" });
            Inventory.AddItem(new Item() { Name = "Bible" });
            Inventory.AddItem(new Item() { Name = "Jesus Bobblehead" });

            _MenuCommands.Add(ConsoleKey.V, new MenuCommandPair() { Name = "View Items", Command = _InventoryController });
            _MenuCommands.Add(ConsoleKey.G, new MenuCommandPair() { Name = "Give an offer?", Command = _GiveOffering });
            _MenuCommands.Add(ConsoleKey.H, new MenuCommandPair() { Name = "Would you like to be healed?", Command = _Heal });
            _MenuCommands.Add(ConsoleKey.A, new MenuCommandPair() { Name = "Bless something?", Command = _Bless });

            _InventoryCommands.Add(ConsoleKey.B, new InvCommandPair() { Name = "Buy Item", Command = _Buy });
        }

        protected override void _ShowBuildingStats()
        {
            string BuildingStats = String.Format(Owner.Name, "Money:" + Owner.Money);
            Console.Write(BuildingStats);


            Tuple<ConsoleColor, string> display = _ReputationStatus();

        }

        public override bool Exit()
        {
            Console.WriteLine("Please come again!");
            return true;
        }

        public virtual bool _GiveOffering()
        {
            while (true)
            {
                int donate;
                Console.WriteLine("How much will you be donating?");
                if (Int32.TryParse(Console.ReadLine(), out donate))
                {
                    if (donate <= 0)
                    {
                        break;
                    }
                    if (donate <= Player.Money)
                    {
                        Console.WriteLine("Thanks for donating!");
                        Player.Money -= donate;
                        Owner.Money += donate;
                        _Reputation += donate / 3;
                        break;
                    }
                }

            }
            return true;

        }
        protected virtual bool _Heal()
        {
            while (true)
            {
                int healNum;
                Console.WriteLine("How much would you like to heal?: ");
                if (Int32.TryParse(Console.ReadLine(), out healNum))
                {
                    if (healNum <= 0)
                    {
                        break;
                    }
                    else if (healNum <= Player.Money)
                    {
                        Player.Money -= healNum;
                        Owner.Money += healNum;
                        _Reputation -= (healNum / 5);
                        Player.HP += healNum;
                        Console.WriteLine("You have been healed!");
                        Console.ReadKey();
                        break;
                    }
                }
            }
            return true;

        }

        protected virtual bool _Bless()
        {
            if (Player.Money < 25)
            {
                Console.WriteLine("Must have more than 25g! Sorry!");
                Console.ReadKey();
                return false;
            }
            return true;

            //didn't finish in time, submitting what I have. 
        }
    }
}
