using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    //Interface for locations (including the dungeon).
    interface ILocation
    {
        string Name { get; }
        ConsoleColor TextColor { get; }
        ConsoleColor BackColor { get; }
        bool Exit();
        bool Enter(PlayerCharacter player);

    }
}
