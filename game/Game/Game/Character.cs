using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Character : Creature
    {

        public Dictionary<string, int> Skills = new Dictionary<string, int>();

        public Character() { Inventory.Size = 9; }
    }
}
