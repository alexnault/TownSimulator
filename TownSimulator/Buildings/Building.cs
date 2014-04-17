using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;
using TownSimulator.Villagers;

namespace TownSimulator.Buildings
{
    abstract class Building : GameObject
    {
        Town town;

        public bool InConstruction { get; private set; }
        public Builder Foreman { get; private set; }

        public abstract int NB_REQUIRED_WOOD { get; }
        public abstract int NB_REQUIRED_STONE { get; }

        protected int WoodCount { get; set; }
        protected int StoneCount { get; set; }

        protected Semaphore _mutex { get; set; }

        public Building(Town town, bool inConstruction = true)
            :base()
        {
            town.AddBuilding(this);

            _mutex = new Semaphore(1, 1);

            ObjectSprite = new Sprite(7, Position.X, Position.Y, 95, 94);
            IsSolid = true;
            IsBig = true;
            InConstruction = inConstruction;
        }
      
        public bool Consort(Builder foreman)
        {
            bool isForeman = false;

            _mutex.WaitOne();
            if (Foreman == null)
            {
                Foreman = foreman;
                isForeman = true;
            }
            _mutex.Release();

            return isForeman;
        }
        public bool Deconsort()
        {
            bool released = false;

            _mutex.WaitOne();
            if (Foreman != null)
            {
                Foreman = null;
                released = true;
            }
            _mutex.Release();

            return released;
        }

        // Refactor to store other ressources
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
            if (InConstruction)
            {
                DrawingUtils.DrawMessage(
                WoodCount.ToString(),
                new Vector2(Position.X * Engine.TileWidth, Position.Y * Engine.TileWidth + YDrawOffset),
                Color.GreenYellow,
                false);
            }

            base.Draw(spriteBatch);
        }
    }
}
