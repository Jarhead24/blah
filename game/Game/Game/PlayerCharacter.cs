using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class PlayerCharacter : Character
    {
        public static readonly Tiles PLAYER_TILE = new Tiles { Symbol = "@", Color = ConsoleColor.Cyan };
        public int xLocation { get; set; }
        public int yLocation { get; set; }
        public Tiles CurrentTileOn { get; set; }
        public int Inebriation { get; set; }//Player's state of drunkeness. 100 is max (death)
        public Character Hireling { get; set; }//Hireling of player     
        public Creature Mount { get; set; }//Horse of player 

        private static PlayerCharacter instance = null;

        /// <summary>
        /// Player constructor.
        /// </summary>
        public PlayerCharacter()
        {
        }

        /// <summary>
        /// Makes Player a singleton
        /// </summary>
        public static PlayerCharacter Instance
        {
            // Singleton 
            get
            {
                if (instance == null)
                {
                    instance = new PlayerCharacter();
                }
                return instance;
            }
        }

        /// <summary>
        /// Consumes food
        /// </summary>
        /// <param name="i">FoodItem</param>
        /// <returns>true when eaten</returns>
        public override bool Consume(FoodItem i)
        {
            if (i.Toxication + Inebriation < 0) { Inebriation = 0; }//handle going negative
            else { Inebriation += i.Toxication; }
            if (Hunger + i.FoodPower >= 100) { Hunger = 100; }
            else { Hunger += i.FoodPower; }
            foreach (EffectParameter Effect in i.Effects)
            {
                Effects.Add(Effect);
            }
            return true;
        }

        /// <summary>
        /// Adds a skill to the player
        /// </summary>
        /// <param name="skill">Name of the skill</param>
        /// <param name="value">Level to set skill to, or if already added then levels to add to skill.</param>
        public void AddSkill(string skill, int value)
        {
            if (Skills.ContainsKey(skill))
            {
                Skills[skill] += value;
            }
            else
            {
                Skills[skill] = value;
            }
        }

        /// <summary>
        /// Returns the level of a skill
        /// </summary>
        /// <param name="skill">Skill name to return</param>
        /// <returns>level of skill</returns>
        public int GetSkill(string skill)
        {
            if (Skills.ContainsKey(skill))
            {
                return Skills[skill];
            }
            else
            {
                return 0;
            }
        }

        public override bool DisplayStatSheet()
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


            //Display Character Effects
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n\nEffects:");
            foreach (EffectParameter E in Effects)
            {
                string str = String.Format("{0,-25}  {1,20}", E.Type, "\tPow:" + E.Power + "\tRng:" + E.Range + "\tDur:" + E.Duration);
                Console.WriteLine(str);
            }

            //Display Hireling
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n\nHireling:");
            if (Hireling == null)
            {
                Console.WriteLine("No one has been hired");
            }
            else { Console.WriteLine(String.Format("{0,-25}  {1,20}", Hireling.Name, "\tLvl:" + Hireling.Level + "\tStr:" + Hireling.Strength + "\tDex:" + Hireling.Dexterity + "\tCon:" + Hireling.Constitution)); }

            //Display Mount
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\nMount:");
            if (Mount == null)
            {
                Console.WriteLine("No mount has been purchased");
            }
            else
            {
                Console.WriteLine(String.Format("{0,-25}  {1,20}", Mount.Name, "\tLvl:" + Mount.Level + "\tStr:" + Mount.Strength + "\tDex:" + Mount.Dexterity + "\tCon:" + Mount.Constitution));
            }
            Console.ReadKey();
            return true;
        }
    };
}
