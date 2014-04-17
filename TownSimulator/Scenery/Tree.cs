using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public Woodcutter Slayer { get; private set; }
        public int Health { get; set; }

        private Semaphore _mutex;
        private static Random _rand = new Random();

        private float _windDirection;
        private float _acceleration;
        private const float MAX_ANGLE = 0.045f;
        private TimeSpan lastGameTime;
        private TimeSpan windTime = TimeSpan.FromMilliseconds(50);
        private float _windStrength;

        public Tree()
        {
            IsSolid = true;
            Health = 100;
            _windDirection = _rand.Next(0, 2);
            _acceleration = 0;
            _windStrength = 1;

            _mutex = new Semaphore(1, 1);

            ObjectSprite = new Sprite(5) { Origin = new Vector2(48, 125) };


            int spriteType = _rand.Next(0, 5);
            switch (spriteType)
            {
                case (0):
                    ObjectSprite.Width = 64;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -16;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(257, 0, ObjectSprite.Width, ObjectSprite.Height);
                    break;
                case (1):
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
                    ObjectSprite.TexturePortion = new Rectangle(0, 129, ObjectSprite.Width, ObjectSprite.Height);
                    break;
                case (3):
                    ObjectSprite.Width = 94;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -32;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(97, 129, ObjectSprite.Width, ObjectSprite.Height);
                    break;
                case (4):
                    ObjectSprite.Width = 96;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -32;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(384, 256, ObjectSprite.Width, ObjectSprite.Height);
                    break;
            }
        }

        public bool Consort(Woodcutter slayer)
        {
            bool isSlayer = false;

            _mutex.WaitOne(); // FCFS
            if (Slayer == null)
            {
                Slayer = slayer;
                isSlayer = true;
            }
            _mutex.Release();

            return isSlayer;
        }

        public bool Deconsort()
        {
            bool released = false;

            _mutex.WaitOne();
            if (Slayer != null)
            {
                Slayer = null;
                released = true;
            }
            _mutex.Release();

            return released;
        }

        public override void Update(GameTime gameTime)
        {
            if (Slayer != null)
            {                
                if (Health == 0)
                    Slayer.Warn(EnvironmentEvent.TreeCutted);
                else
                    Health--;
            }

            CalculateWind(gameTime);
            base.Update(gameTime);
        }

        private void CalculateWind(GameTime gameTime)
        {
            if (lastGameTime > windTime)
            {
                float factor = (MAX_ANGLE - Math.Abs(ObjectSprite.Rotation));

                _acceleration = factor * (_windStrength) / 16;

                if (ObjectSprite.Rotation >= MAX_ANGLE - 0.01)
                {
                    _windDirection = 1;
                    _windStrength = _rand.Next(1, 4);
                }
                else if (ObjectSprite.Rotation <= -MAX_ANGLE + 0.01)
                {
                    _windDirection = 0;
                    _windStrength = _rand.Next(1, 4);
                }

                if (_windDirection == 0)
                    ObjectSprite.Rotation += _acceleration;
                else
                    ObjectSprite.Rotation -= _acceleration;

                MathHelper.Clamp(ObjectSprite.Rotation, -MAX_ANGLE, MAX_ANGLE);

                lastGameTime = TimeSpan.FromMilliseconds(0);
            }

            lastGameTime += gameTime.ElapsedGameTime;
        }
    }
}
