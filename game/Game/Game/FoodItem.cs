using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class FoodItem : Item
    {

        public int FoodPower { get; set; } = 0;
        public int Toxication { get; set; } = 0;

        public override string View()
        {
            return String.Format("{0,-25}  {1,20}", Name, "\tLvl:" + Level + "\tWt:" + Weight + "\tFP:" + FoodPower + "\tTOX:" + Toxication);
        }

    }
}
