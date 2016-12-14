using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class GameInfoTracker
    {
        private static GameInfoTracker instance = null;
        public int dungeonLevel { get; set; }
        public int score { get; set; }
        public bool currentLevelComplete { get; set; }

        /// <summary>
        /// GameInfoTracker constructor
        /// </summary>
        public GameInfoTracker()
        {
            this.dungeonLevel = 1;
            this.score = 0;
            this.currentLevelComplete = false;
        }

        /// <summary>
        /// Makes GameInfoTracker a singleton
        /// </summary>
        public static GameInfoTracker Instance
        {
            // Singleton
            get
            {
                if (instance == null)
                {
                    instance = new GameInfoTracker();
                }
                return instance;
            }
        }

        /// <summary>
        /// Increases score and dungeon level when a level is completed
        /// </summary>
        public void levelComplete()
        {
            this.score = this.score + this.dungeonLevel;
            this.dungeonLevel++;
        }
    }
}
