using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TownSimulator.Villagers
{
    class Woodcutter : Villager
    {

        public Woodcutter(string firstname, string lastname, Town hometown)
            : base(firstname, lastname, hometown)
        {
            Position = new Vector2(10, 10);
        }

        protected override void Run()
        {
            while (true) // Kill Villager
            {
                //Semaphore s = new Semaphore(0, 1);
                Thread.Sleep(1000);
                //s.WaitOne();
            }
        }
    }
}
