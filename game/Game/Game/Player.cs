using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Player
    {
        private static Player instance = null;
        public tiles PlayerTile { get; }
        public int xLocation { get; set; }
        public int yLocation { get; set; }
        public tiles CurrentTileOn { get; set; }
        private Board board = Board.Instance;

        public Player()
        {
            PlayerTile = new tiles { Symbol = "@", Color = ConsoleColor.Cyan };
        }

        public static Player Instance
        {
            // Singleton
            get
            {
                if (instance == null)
                {
                    instance = new Player();
                }
                return instance;
            }
        }

        public void moveUp()
        {
            board.movePlayer(xLocation, yLocation, xLocation, yLocation - 1, this);
        }
        public void moveDown()
        {
            board.movePlayer(xLocation, yLocation, xLocation, yLocation + 1, this);
        }
        public void moveRight()
        {
            board.movePlayer(xLocation, yLocation, xLocation + 1, yLocation, this);
        }
        public void moveLeft()
        {
            board.movePlayer(xLocation, yLocation, xLocation - 1, yLocation, this);
        }
    };
}
