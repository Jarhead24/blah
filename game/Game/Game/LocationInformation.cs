using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class LocationInformation
    {
        public int xLocation { get; set; }
        public int yLocation { get; set; }
        public Tiles tileOn { get; set; }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public LocationInformation()
        {

        }

        /// <summary>
        /// Constructor to define Location Information
        /// </summary>
        /// <param name="x">xCoord</param>
        /// <param name="y">yCoord</param>
        /// <param name="tileOn">Tile at the location</param>
        public LocationInformation(int x, int y, Tiles tileOn)
        {
            this.xLocation = x;
            this.yLocation = y;
            this.tileOn = tileOn;
        }
        
        /// <summary>
        /// Method to easily change location information
        /// </summary>
        /// <param name="x">xCoord</param>
        /// <param name="y">yCoord</param>
        /// <param name="tileOn">Tile at the location</param>
        public void changeLocationInformation(int x, int y, Tiles tileOn)
        {
            this.xLocation = x;
            this.yLocation = y;
            this.tileOn = tileOn;
        }
    }
}
