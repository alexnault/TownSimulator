using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TownSimulator.Villagers;

namespace TownSimulator
{
    public class Town
    {
        public Villager[] Villagers { get; private set; }

        protected uint NbVillagers;

        public Town(uint nbVillagers)
        {
            NbVillagers = nbVillagers;

            Villagers = new Villager[nbVillagers];
            for (int i = 0; i < nbVillagers; i++)
            {
                Villagers[i] = new Woodcutter("Bob", "Gratton", this);
            }
        }

        public void Update(Microsoft.Xna.Framework.GameTime gametime)
        {
            for (int i = 0; i < NbVillagers; i++)
            {
                Villagers[i].Update(gametime);
            }
            // TODO update items, buildings, etc.
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
