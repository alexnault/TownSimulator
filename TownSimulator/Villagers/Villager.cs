using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TownSimulator.Items;

namespace TownSimulator.Villagers
{
    abstract class Villager : TileEngine.GameObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public uint Age { get; private set; }

        public uint Hunger { get; private set; }
        public uint Thirst { get; private set; }
        public uint Satisfaction { get { return 100 - ((Hunger + Thirst) / 2); } }

        public Thread thread { get; private set; }

        public Item ItemHolding { get; set; }

        protected Town HomeTown { get; set; }
        
        public Villager(string firstname, string lastname, Town hometown)
        {
            FirstName = firstname;
            LastName = lastname;
            Age = (uint) new Random().Next(18, 50);

            Hunger = 0;
            Thirst = 0;

            HomeTown = hometown;

            thread = new Thread(new ThreadStart(Start));
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();
        }

        virtual protected void Start()
        {
            Run();
        }

        abstract protected void Run();
    }
}
