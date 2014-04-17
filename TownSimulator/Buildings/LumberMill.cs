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
        public override int NB_REQUIRED_WOOD { get { return 20; } }
        public override int NB_REQUIRED_STONE { get { return 0; } }

        Carrier Worker;

        public LumberMill(Town town, bool inConstruction = true) : base(
            town,
            new Sprite(8, 0, 0, 96, 96),
            inConstruction)
        {
            IsSolid = true;
            WoodCount = 0;
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

        public int StoreWood(int amount)
        {
            _mutex.WaitOne(); // FCFS
            WoodCount += amount; // Currently store all wood 
            if (Worker != null)
                Worker.Warn(EnvironmentEvent.WoodStored);
            _mutex.Release();
            return amount;
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
                WoodCount.ToString() + "/" + "inf.", 
                new Vector2(Position.X * Engine.TileWidth, Position.Y * Engine.TileWidth + YDrawOffset), 
                Color.GreenYellow,
                false);

            base.Draw(spriteBatch);
        }
    }
}
