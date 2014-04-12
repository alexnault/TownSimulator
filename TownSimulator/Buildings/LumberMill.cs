using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;

namespace TownSimulator.Buildings
{
    public class LumberMill : Building
    {
        public int WoodCount { get; private set; }

        private Semaphore _mutex;

        public LumberMill() : base()
        {
            IsSolid = true;
            WoodCount = 0;

            _mutex = new Semaphore(1, 1);

            ObjectSprite = new Sprite(8, Position.X, Position.Y, 95, 94);
        }

        public void StoreWood()
        {
            _mutex.WaitOne(); // FCFS
            WoodCount++;
            _mutex.Release();
        }
    }
}
