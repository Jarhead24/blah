using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Item
    {
        public string ID { get; }
        public string Name { get; set; } = "";
        public int Level { get; set; } = 0;
        public int Weight { get; set; } = 0;
        public int Durability { get; set; } = 100;
        public int Cost { get; set; } = 0;
        public int Power { get; set; } = 0;
        public int Rarity { get; set; } = 0;
        public string Type { get; set; } = ""; //Item type 
        public List<EffectParameter> Effects = new List<EffectParameter>(); //holds effects of item
        public bool Identified { get; set; } = false;//Has item been identified (items must be identified to view item effects.)

        public Item()
        {//generate unique ID at Item creation
            ID = Guid.NewGuid().ToString();
        }
        public virtual string View()
        { //default Item view
            return String.Format("{0,-25}  {1,20}", Name, "\tLvl:" + Level + "\tWt:" + Weight + "\tDur:" + Durability);
        }
    }

}
