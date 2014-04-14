using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;

namespace TownSimulator.Buildings
{
    class ConstructionSite : Building
    {
        public const int NB_REQUIRED_WOOD = 10;

        public int WoodCount { get; private set; }

        private Semaphore _mutex;

        public ConstructionSite(Town town) : base(town)
        {
            IsSolid = true;
            WoodCount = 0;

            _mutex = new Semaphore(1, 1);

            ObjectSprite = new Sprite(12, Position.X, Position.Y, 96, 96);
        }

        public int AddWood(int amount)
        {
            _mutex.WaitOne();
            int amountNeeded = NB_REQUIRED_WOOD - WoodCount;
            int amountDropped = Math.Min(amountNeeded, amount);
            WoodCount += amountDropped;
            _mutex.Release();
            return amountDropped;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {            
            DrawingUtils.DrawMessage(
                WoodCount.ToString(),
                new Vector2(Position.X * Engine.TileWidth, Position.Y * Engine.TileWidth + YDrawOffset),
                Color.GreenYellow,
                false);

            base.Draw(spriteBatch);
        }
    }
}
