using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Player
    {
        public Tiles PlayerTile { get; }
        public int xLocation { get; set; }
        public int yLocation { get; set; }
        public Tiles CurrentTileOn { get; set; }
        public int health { get; set; }
        public int level { get; set; }
        public int attack { get; set; }
        public int defense { get; set; }
        public string name { get; set; }
        public string background { get; set; }
        public string occupation { get; set; }
        private static Player instance = null;

        /// <summary>
        /// Player constructor. Automatically randomizes all player info. 
        /// </summary>
        public Player()
        {
            PlayerTile = new Tiles { Symbol = "@", Color = ConsoleColor.Cyan };
            InfoGenerator infoGen = new InfoGenerator();
            this.name = infoGen.nameGenerator();
            this.background = infoGen.backgroundGenerator();
            this.occupation = infoGen.jobGenerator();
            this.level = DieRoller.roll(6, 3);
            this.health = DieRoller.roll(6, 3) + DieRoller.roll(4, this.level);
            this.attack = DieRoller.roll(6, 3) + DieRoller.roll(4, this.level);
            this.defense = DieRoller.roll(6, 3) + DieRoller.roll(4, this.level);
        }

        /// <summary>
        /// Makes Player a singleton
        /// </summary>
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

        /// <summary>
        /// Move player up one space
        /// </summary>
        public void moveUp()
        {
            LocationInformation newLoc = Board.Instance.movePlayer(xLocation, yLocation, xLocation, yLocation - 1);
            this.CurrentTileOn = newLoc.tileOn;
            this.xLocation = newLoc.xLocation;
            this.yLocation = newLoc.yLocation;
        }

        /// <summary>
        /// Move player down one space
        /// </summary>
        public void moveDown()
        {
            LocationInformation newLoc = Board.Instance.movePlayer(xLocation, yLocation, xLocation, yLocation + 1);
            this.CurrentTileOn = newLoc.tileOn;
            this.xLocation = newLoc.xLocation;
            this.yLocation = newLoc.yLocation;
        }

        /// <summary>
        /// Move player right one space
        /// </summary>
        public void moveRight()
        {
            LocationInformation newLoc = Board.Instance.movePlayer(xLocation, yLocation, xLocation + 1, yLocation);
            this.CurrentTileOn = newLoc.tileOn;
            this.xLocation = newLoc.xLocation;
            this.yLocation = newLoc.yLocation;
        }

        /// <summary>
        /// Move player left one space
        /// </summary>
        public void moveLeft()
        {
            LocationInformation newLoc = Board.Instance.movePlayer(xLocation, yLocation, xLocation - 1, yLocation);
            this.CurrentTileOn = newLoc.tileOn;
            this.xLocation = newLoc.xLocation;
            this.yLocation = newLoc.yLocation;
        }
    };
}
