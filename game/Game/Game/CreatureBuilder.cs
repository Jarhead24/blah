using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class CreatureBuilder
    { //NOT IMPLEMENTED - Only using GetName() function currently.
      //Class will be used to Build the different creatures in the game.
        public static Creature Build(string type)
        {
            switch (type)
            {
                case "HUMAN":
                    return new Character();
                case "PLAYER":
                    return new PlayerCharacter();
                default:
                    return new Character();
            }
        }
        public static string GetName()
        {//This method is copy/paste from old project. Needs rewritten.
            string[] RoyalTitles = new string[]{"King", "Duke", "Earl", "Mayor", "Master", "Lord","Chief","Captain","Colonel","Lieutenant","Sergeant",
                       "Major","Hero","Champion","Father", "Boss", "Prince","Princess"};

            string[] Names = new string[]{"Pew-Pew","Paincake","Slime","Mongrel","Blood","Grime", "Neckbeard", "Faceplant","Pickle","Cookie",
                 "Fuzzy","Furry","Hoax", "Bubba","Bucky","Goober","Glitch","Mohawk","Cowboy", "Stump", "Nookie","Plaid",
                 "Chuckle","Clam", "Cheese","Platypus","Bum","Dumpster","Mouthbreather", "Beerbelly","Frag","Crush",
                 "Crash","Kill","Moustache","Undercover","Gorilla","Crunch","Rainbow","Hungry","Sorry","Mistake","Boulder",
                 "Pillow","Curtain", "Kid", "Monkey", "Ooze", "Lonestar", "Funky", "Goth", "Emo","Death Metal","Rockstar",
                 "Cosmic", "Pirate", "Bomb","Slacker","Detective","Teddy Bear","Spud","Sludge","Squirm","Wiggle",
                 "Dance","Party","Pimple"};

            string[] Adj = new string[]{"monger","pudding","sausage","burger","creep","hax","jammer","spank","goblin","sleaze","crank","cramp","masher",
               "stash", "cake","boy","girl"};

            //turned off. Names were getting too long.
           /* string[] Ender = new string[]{"the Super Tasty", "the Delicious","the Frothy","the Gasy","the Over-Confident","the Freshest","the Rodeo Clown",
                 "the Best-At-Everything-Period","the Silent but Deadly","the Nerd","the Geek","the Hipster","the Grimest Reaper",
                 "the Mime","the Sad","the Clinically Depressed","the Stupid","the Mildly Curious","the Furious","the Dubious",
                 "the Unbearable","the Devastatingly Handsome","the Lonely","the Sexy","the Defective","the 99% waterproof",
                 "the Amazingly Drunk", "the Smooth", "the Mediocre Genius", "the Original Gangsta", "the Smoothest Criminal",
                 "the Hallucinating","Esquire", "Jr.","Sr.","the Really Angry", "of the Forest","the Lactose Intolerant","of the Meadow",
                 "the Incontinent","the Gluttonous","the Envious","the Jealous","the Lost Jackson"};
                 */

            string[] Roman = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

            string name = "";
            bool RoyalFlag = false;

            //Determine if Royal starter       

            int x = StaticRandom.Instance.Next(0, 6);
            if (x == 5)
            {
                name += RoyalTitles[StaticRandom.Instance.Next(0, RoyalTitles.Length - 1)] + " ";
                RoyalFlag = true;
            }
            //Add a single name
            name += Names[StaticRandom.Instance.Next(0, Names.Length - 1)];

            //Possible add another name
            x = StaticRandom.Instance.Next(1, 3);
            if (x > 1)
            {  name += " " + Names[StaticRandom.Instance.Next(0, Names.Length - 1)];  }
            //Append Adjective. Chance it will be a Royal one
            x = StaticRandom.Instance.Next(1, 5);
            if (x == 1 & RoyalFlag == false)
            {
                name += "-" + RoyalTitles[StaticRandom.Instance.Next(0, RoyalTitles.Length - 1)];
                RoyalFlag = true;
            }
            else if (x > 4)
            {  name += Adj[StaticRandom.Instance.Next(0, Adj.Length - 1)]; }
            //Generate Ender
            int enderRand = StaticRandom.Instance.Next(1, 6); // 5 does nothing
            if (enderRand == 6 & RoyalFlag == false)
            { //Possible Royal Ender
                name += "-" + RoyalTitles[StaticRandom.Instance.Next(0, RoyalTitles.Length - 1)];
            }
            //Possible Roman Ender
            x = StaticRandom.Instance.Next(1, 7);
            if (x == 7)
            {
                name += " ";
                while (true)
                {
                    x = StaticRandom.Instance.Next(1, 9);
                    name += Roman[x];
                    if (x < 9)
                    {  break;  }
                }
            }

            //turned off
            /*if (enderRand < 4)
            { //Ender     
                name += " " + Ender[StaticRandom.Instance.Next(0, Ender.Length - 1)];
            }*/

            //Add another name if it gets to this point and is still under 8
            if (name.Length < 12)
            {
                name += " " + Names[StaticRandom.Instance.Next(0, Names.Length - 1)];
            }
            return name;
        }
        
    }
}
