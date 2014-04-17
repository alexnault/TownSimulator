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

        private bool _inConstruction;
        public bool InConstruction
        {
            get { return _inConstruction; }
            set
            {
                if (value)
                    ObjectSprite.TexturePortion = new Rectangle(0, 0, ObjectSprite.Width, ObjectSprite.Height);
                else
                    ObjectSprite.TexturePortion = new Rectangle(ObjectSprite.Width, 0, ObjectSprite.Width, ObjectSprite.Height);
                _inConstruction = value;
            }
        }
        
        public Builder Foreman { get; private set; }

        public abstract int NB_REQUIRED_WOOD { get; }
        public abstract int NB_REQUIRED_STONE { get; }
        public bool ResourcesSpent { get; set; }
	
        //protected TimeSpan BuildTime { get; set; } // TODO revert to this
        //protected TimeSpan LastGameTime { get; private set; }
        protected int Progress;

        protected int WoodCount { get; set; }
        protected int StoneCount { get; set; }

        protected Semaphore _mutex { get; set; }

        public Building(Town town, Sprite sprite, bool inConstruction = true)
            :base()
        {
            town.AddBuilding(this);

            _mutex = new Semaphore(1, 1);

            IsSolid = true;
            IsBig = true;
            ObjectSprite = sprite;
            InConstruction = inConstruction;
            ResourcesSpent = !inConstruction;
            Progress = 0;
            //LastGameTime = new TimeSpan();
            //BuildTime = new TimeSpan(0, 0, 5);
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

        public bool NeedWood()
        {
            return (InConstruction && !ResourcesSpent && WoodCount < NB_REQUIRED_WOOD);
        }
        public bool NeedStone()
        {
            return (InConstruction && !ResourcesSpent && StoneCount < NB_REQUIRED_STONE);
        } 

        public override void Update(GameTime gameTime)
        {
            if (InConstruction)
            {
                if (Foreman != null)
                {
                    if (ResourcesSpent)
                    {
                        Progress++;
                        //LastGameTime += gameTime.ElapsedGameTime;
                        //if (LastGameTime > BuildTime)
                        if (Progress == 1000)
                        {
                            InConstruction = false;
                            Foreman.Warn(EnvironmentEvent.BuildingBuilt);
                        }
                    }
                    else
                    {
                        if (WoodCount >= NB_REQUIRED_WOOD && StoneCount >= NB_REQUIRED_STONE)
                        {
                            WoodCount -= NB_REQUIRED_WOOD;
                            StoneCount -= NB_REQUIRED_STONE;
                            ResourcesSpent = true;
                        }
                    }
                }
            }
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
