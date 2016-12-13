using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class DieRoller
    {
        public static int roll()
        {
            return StaticRandom.Instance.Next(1, 7);
        }
        public static int roll(int sides)
        {
            return StaticRandom.Instance.Next(1, sides + 1);
        }
        public static int roll(int numd, int sides)
        {
            int total = 0;
            for (int i = 0; i < numd; i++)
            {
                total += StaticRandom.Instance.Next(1, sides + 1);
            }
            return total;

        }
        public static int roll(int numd, int sides, int t)
        {
            int x = 0;

            for (int i = 0; i < numd; i++)
            {
                if (StaticRandom.Instance.Next(1, sides + 1) >= t)
                { x++; }
            }
            return x;
        }
    }
}