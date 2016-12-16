using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Creature
    {
        public string ID = Guid.NewGuid().ToString(); //random ID
        public string Name { get; set; } = ""; //Creature's Name
        public string Race { get; set; } = "Human"; //Race of Creature      
        public int MaxHP => Constitution * 5 + Level; //Max Hit Points
        public int HP { get; set; } //Current Hit Points
        public int Level { get; set; } = 0; //Current Creature Level
        public int Strength { get; set; } = 1; //Current Strength
        public int Dexterity { get; set; } = 1; //Current Dexterity
        public int Constitution { get; set; } = 1;//Current Constitution
        public int Luck { get; set; } = 1;//Current Luck
        public int Hunger { get; set; } = 100; //Current Hunger. 100 is Full
        public int Weight { get; set; } = 0; //Current Carry Weight
        public Container Inventory { get; set; } = new Container(); //Creature Inventory
        public int Money { get; set; } = 0;
        public Dictionary<string, int> BaseStats = new Dictionary<string, int>();//dictionary for storing base stats
        public List<EffectParameter> Effects = new List<EffectParameter>();//List for storing effects current effecting player

        public Creature()
        {
            HP = MaxHP;
            BaseStats["Strength"] = Strength;
            BaseStats["Dexterity"] = Dexterity;
            BaseStats["Constitution"] = Constitution;
            BaseStats["Luck"] = Luck;
        }
        protected virtual bool RefreshStats()
        {
            Strength = BaseStats["Strength"];
            Dexterity = BaseStats["Dexterity"];
            Constitution = BaseStats["Constitution"];
            Luck = BaseStats["Luck"];
            foreach (EffectParameter Effect in Effects)
            {
                //apply effect (NOT IMPLEMENTED)
            }
            return true;
        }
        public virtual bool Consume(FoodItem i)
        {

            if (Hunger + i.FoodPower >= 100) { Hunger = 100; }
            else { Hunger += i.FoodPower; }
            foreach (EffectParameter Effect in i.Effects)
            {
                Effects.Add(Effect);
            }
            return true;
        }
        public bool AddItem(Item i)
        {
            if (Inventory.AddItem(i))
            {
                Weight += Inventory.Weight;
                return true;
            }
            else { return false; }

        }

        //remove item at position n and return it
        public bool RemoveItem(out Item i, int position = 0, string ID = "")
        {

            if (Inventory.RemoveItem(out i, position, ID)) { Weight -= Inventory.Weight; return true; }
            else { return false; }


        }

        public void ChangeHP(int value)
        {
            if (HP + value > MaxHP) { HP = MaxHP; }
            else { HP += value; }
        }

        public virtual bool DisplayInventory()
        {
            int i = 0;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Name + "'s Inventory:\n");
            for (i = 0; i < Inventory.Contents.Count; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Inventory.ViewItem(i));
                if (Inventory.Contents[i].Identified == true)
                {
                    foreach (EffectParameter E in Inventory.Contents[i].Effects)
                    {
                        string str = String.Format("\t{0}  {1,35}", E.Type, "\tPow:" + E.Power + "\tRng:" + E.Range + "\tDur:" + E.Duration);
                        Console.ForegroundColor = E.Color;
                        Console.WriteLine(str);
                        Console.WriteLine("\t" + E.Description);
                    }
                }
                Console.WriteLine();
            }
            if (i == 0) { Console.WriteLine("<empty>"); }
            Console.ReadKey();
            return true;
        }

        public virtual bool DisplayStatSheet()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Name + "\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(String.Format("{0,-25}{1,20}", "Money: ", Money));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(String.Format("{0,-25}{1,20}", "Level: ", Level));
            Console.WriteLine(String.Format("{0,-25}{1,20}", "HP: ", HP + "/" + MaxHP));
            Console.WriteLine(String.Format("{0,-25}{1,20}", "Strength: ", Strength));
            Console.WriteLine(String.Format("{0,-25}{1,20}", "Dexterity: ", Dexterity));
            Console.WriteLine(String.Format("{0,-25}{1,20}", "Constitution: ", Constitution));
            Console.WriteLine(String.Format("{0,-25}{1,20}", "Weight: ", Weight + "/" + (Strength * 20)));
            Console.WriteLine(String.Format("{0,-25}{1,20}", "Hunger: ", Hunger));

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\nEffects:");
            foreach (EffectParameter E in Effects)
            {
                string str = String.Format("{0,-25}  {1,20}", E.Type, "\tPow:" + E.Power + "\tRng:" + E.Range + "\tDur:" + E.Duration);
                Console.WriteLine(str);
            }


            Console.ReadKey();
            return true;
        }


    }
}
