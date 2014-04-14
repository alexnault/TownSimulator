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

        private float _angle;
        private float _windDirection;
        private float _acceleration;
        private const float MAX_ANGLE = 0.045f;
        private TimeSpan lastGameTime;
        private TimeSpan windTime = TimeSpan.FromMilliseconds(50);

        public Tree()
        {
            IsSolid = true;
            Health = 100;
            _angle = 0;
            _windDirection = _rand.Next(0, 2);
            _acceleration = 0;

            _mutex = new Semaphore(1, 1);

            ObjectSprite = new Sprite(5);
            int spriteType = _rand.Next(0, 5);
            switch (spriteType)
            {
                case (0):
                    ObjectSprite.Width = 64;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -16;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(256, 0, ObjectSprite.Width, ObjectSprite.Height);
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
                    ObjectSprite.TexturePortion = new Rectangle(0, 128, ObjectSprite.Width, ObjectSprite.Height);
                    break;
                case (3):
                    ObjectSprite.Width = 96;
                    ObjectSprite.Height = 128;
                    XDrawOffset = -32;
                    YDrawOffset = -96;
                    ObjectSprite.TexturePortion = new Rectangle(96, 128, ObjectSprite.Width, ObjectSprite.Height);
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

        public void Consort(Woodcutter slayer)
        {
            _mutex.WaitOne(); // FCFS
            if (Slayer == null)
                Slayer = slayer;
            _mutex.Release();
        }

        public override void Update(GameTime gameTime)
        {
            if (Slayer != null)
            {
                Health--;
                if (Health == 0)
                {
                    Slayer.Warn(EnvironmentEvent.TreeCutted);
                }
            }

            CalculateWind(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ObjectSprite != null)
            {
                Vector2 posPixels = new Vector2(Position.X * Engine.TileWidth, Position.Y * Engine.TileHeight);

                DrawingUtils.DrawRectangle(new Rectangle((int)posPixels.X, (int)posPixels.Y, Engine.TileWidth, Engine.TileHeight), Color.Blue);
                ObjectSprite.Draw(spriteBatch, (int)posPixels.X + XDrawOffset, (int)posPixels.Y + YDrawOffset, _angle, 48, 128);

            }
        }

        private void CalculateWind(GameTime gameTime)
        {
            if (lastGameTime > windTime)
            {
                _acceleration = (MAX_ANGLE - Math.Abs(_angle)) / 16;

                if (_angle >= MAX_ANGLE - 0.01)
                    _windDirection = 1;
                else if (_angle <= -MAX_ANGLE + 0.01)
                    _windDirection = 0;

                if (_windDirection == 0)
                    _angle += _acceleration;
                else
                    _angle -= _acceleration;

                MathHelper.Clamp(_angle, -MAX_ANGLE, MAX_ANGLE);

                lastGameTime = TimeSpan.FromMilliseconds(0);
            }

            lastGameTime += gameTime.ElapsedGameTime;
        }
    }
}
