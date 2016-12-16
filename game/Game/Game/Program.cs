using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(110, 40);
            InfoGenerator infoGen = new InfoGenerator();
            var test = infoGen.nameGenerator();
            PlayerCharacter Player = new PlayerCharacter()
            {
                Name = infoGen.nameGenerator(),
                Money = 1000,
                Hunger = 50,
                Strength = DieRoller.roll(3, 6),
                Dexterity = DieRoller.roll(3, 6),
                Constitution = DieRoller.roll(3, 6),
                Luck = DieRoller.roll(1, 4)
            };

            Player.HP = DieRoller.roll(Player.Constitution);

            //add buildings to Towns
            LocationMap Town1 = new LocationMap() { Name = "Peasantville" };
            Town1.AddLocation(new Sage());
            Town1.AddLocation(new WizardTower());
            Town1.AddLocation(new Blacksmith());
            Town1.AddLocation(new Barracks());

            LocationMap Town2 = new LocationMap() { Name = "Poorburg" };
            Town2.AddLocation(new Church());
            Town2.AddLocation(new Tavern());
            Town2.AddLocation(new Stables());
            Town2.AddLocation(new Alchemist());
            Town2.AddLocation(new Board());

            //add Towns to Main Map
            LocationMap MainMap = new LocationMap() { Name = "Main Menu" };
            MainMap.AddLocation(Town1);
            MainMap.AddLocation(Town2);

            //function for entering locations
            MainMap.Enter(Player);

            Console.ReadKey();
        }
    }
}
