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
    class LumberMill : Building
    {
        public int WoodCount { get; private set; }

        private Semaphore _mutex;

        Carrier Worker;

        public LumberMill(Town town) : base(town)
        {
            IsSolid = true;
            WoodCount = 0;

            _mutex = new Semaphore(1, 1);

            ObjectSprite = new Sprite(8, Position.X, Position.Y, 95, 94);
        }

        public void Consort(Carrier worker)
        {
            _mutex.WaitOne(); // FCFS
            if (Worker == null)
                Worker = worker;
            _mutex.Release();
        }

        public int CheckWood() // Not sure if needed
        {
            _mutex.WaitOne();
            int woodCount = WoodCount;
            _mutex.Release();
            return woodCount;
        }

        public void StoreWood()
        {
            _mutex.WaitOne(); // FCFS
            WoodCount++;
            if (Worker != null)
                Worker.Warn(EnvironmentEvent.WoodStored);
            _mutex.Release();
        }

        public int DiscardWood(int amount)
        {
            _mutex.WaitOne();
            int woodDiscarded = Math.Min(WoodCount, amount);
            WoodCount -= woodDiscarded;
            _mutex.Release();
            return woodDiscarded;
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
