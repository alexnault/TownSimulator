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
            int spriteType = _rand.Next(0, 4);
            switch (spriteType)
            {
                case(0):
                    ObjectSprite.Width = 64;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -16;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(256, 0, ObjectSprite.Width, ObjectSprite.Height);
                    break;
                case(1):
                    ObjectSprite.Width = 64;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -16;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(320, 0, ObjectSprite.Width, ObjectSprite.Height);
                    break;
                case (2):
                    ObjectSprite.Width = 96;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -32;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(0, 128, ObjectSprite.Width, ObjectSprite.Height);
                    break;
                case (3):
                    ObjectSprite.Width = 96;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -32;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(96, 128, ObjectSprite.Width, ObjectSprite.Height);
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
