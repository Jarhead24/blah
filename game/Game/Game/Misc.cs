using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    //structure for the value part of the _InventoryCommands Dictionary
    public struct InvCommandPair
    { 
        public string Name;
        public Func<Item, bool> Command;//InventoryCommands take Item and return bool
    }

    //structure for the value part of the _MenuCommands Dictionary
    public struct MenuCommandPair
    { 
        public string Name;
        public Func<bool> Command;//MenuCommands return bool. Most of them act on Player, which already has a reference in the class, so does not need passed in.
    }

    //class for storing effects EFFECTS NOT IMPLEMENTED (but can be created and displayed)
    public class EffectParameter
    {
        public string ID = Guid.NewGuid().ToString(); 
        public string Name = "";//Name of effect (For example, "Fireball")
        public string Description = "";//Description of effect (For example, "A molten meteor erupts from your hand!")
        public int Duration { get; set; } = 1; //-1 is infinite
        public int Power { get; set; } = 1;//Overall Strength of the effect
        public int Range { get; set; } = 1;//Range of the effect
        public string Type { get; set; } = "NONE";//effect type. Used to look up effect in table and apply it (NOT IMPLEMENTED)
        public ConsoleColor Color { get; set; } = ConsoleColor.White;
    }

    //class for the value part of the Dictionary for Building Services (Wizard Tower Enchant, Alchemist Brew, Sage Scribe)
    public class ServiceValuePair
    { 
        public string Name;
        public EffectParameter Effect;//effect to be applied
        public int RepThreshold; //Reputation required to view 
        public int Cost = 1; //Base Cost

    }
}
