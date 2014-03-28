using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TownSimulator.Villagers;

namespace TownSimulator
{
    class Town
    {
        public TileEngine.TileMap Map { get; private set; }
        public Villager[] Villagers { get; private set; }

        protected uint NbVillagers;

        public Town(uint nbVillagers, TileEngine.TileMap map)
        {
            Map = map;
            NbVillagers = nbVillagers;

            Villagers = new Villager[nbVillagers];
            for (int i = 0; i < nbVillagers; i++)
            {
                Villagers[i] = new Woodcutter("Bob", "Gratton", this);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < NbVillagers; i++)
            {
                Villagers[i].Draw(spriteBatch);
            }
            // TODO draw items, buildings, etc.
        }
    }
}
