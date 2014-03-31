using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;
using TownSimulator.Villagers;

namespace TownSimulator.Scenery
{
    class Tree : GameObject
    {
        
        //public bool SomeoneIsCuttingIt;

        public Woodcutter Slayer;
        public Semaphore _mutex;

        private static Random _rand = new Random();

        public Tree()
        {
            IsSolid = true;
            _mutex = new Semaphore(1, 1);
            //SomeoneIsCuttingIt = false;

            ObjectSprite = new Sprite(5);
            int spriteType = _rand.Next(0, 2);
            switch (spriteType)
            {
                case(0):
                    ObjectSprite.Width = 64;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -16;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(384, 0, 64, 128);
                    break;
                case(1):
                    ObjectSprite.Width = 64;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -16;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(448, 0, 64, 128);
                    break;
            }
        }

        public void Consort(Woodcutter slayer)
        {
            _mutex.WaitOne(); // FCFS
            if (Slayer == null)
                Slayer = slayer;
            _mutex.Release();
        }
    }
}
