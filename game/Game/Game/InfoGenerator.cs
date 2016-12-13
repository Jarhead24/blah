using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class InfoGenerator
    {
        public string nameGenerator()
        {
            string[] nameFirst = { "Ragnok", "Sebastion", "Ash", "Brak", "Harry", "Scruffy" };
            string[] nameLast = { " Hampton", " Catchem", " Potato", " Zyhper", " Phantomhive", " McJenkins" };
            string[] nameTitle = { " The Betrayer", " The Destroyer", " Master", " Hero", " The Servant", " The Cannon Fodder" };

            string firstn = nameFirst[DieRoller.roll(nameFirst.Length) - 1];
            string lastn = nameLast[DieRoller.roll(nameLast.Length) - 1];
            string sire = nameTitle[DieRoller.roll(nameTitle.Length) - 1];

            return firstn + lastn + " " + sire;
        }

        public string backgroundGenerator()
        {
            string[] history = {"Your ancestors were great rulers however their actions have cursed you to die by disenterry at some point.... probably should lay off the fiber",
                "Your were born of an assassain clan that was destroyed your the only soul that knows your own name.", "Your mother was a hamster and your father smelled of elder berries!",
            "Your family has always been selected to keep the town packed with fresh village idiots, best of luck to ya.", "Your past is soaked in blood and powdered with the ashes of enemies", "Some men just want to watch the world burn...",
                "Congratulations you were someone's personal bit- i mean slave!","Born withoput a soul you trudge through life a souless husk of being",
                "Even as a child you've decided your purpose in life is to be a thorn in everyones side","Congrats your the sucker that cleans the dungeons after torture sessions, you have no idea how you got talked into it, hope you can hold your breath" };

            return history[DieRoller.roll(history.Length) - 1];
        }

        public string jobGenerator()
        {
            string[] job = { "Assassin", "Bard", "Warmonger", "Ninja", "Hoodie Ninja", "The Bitch", "The Slave", "The Ruler", "The Black Knight", "The Betrayer",
            "Soulweaver","MEDIC!!!","The Janitor","One Hell of a Butler", "Demigod","Samurai","La Bomba", "The Merchant","The Travler", "The Choosen One"};

            return job[DieRoller.roll(job.Length) - 1];
        }
    }
}
