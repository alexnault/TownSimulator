using Microsoft.Xna.Framework;
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
        public Dictionary<int, Villager> Villagers { get; private set; }
        //public Villager[] Villagers { get; private set; }
        public int NbVillagers { get; private set; }

        public Semaphore AITurn;

        public Town(uint initNbVillagers)
        {
            NbVillagers = 0;

            AITurn = new Semaphore(1, 1);
            
            //Villagers = new Villager[nbVillagers];
            Villagers = new Dictionary<int, Villager>();
            for (int i = 0; i < initNbVillagers; i++)
            {
                // Automaticly added to this town
                new Woodcutter("Bob", "Gratton", this);
                //Villagers[i] = new Woodcutter("Bob", "Gratton", this);
            }
        }

        public void Update(GameTime gametime)
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

        public void AddVillager(Villager villager)
        {
            Villagers.Add(NbVillagers, villager);
            NbVillagers++;
        }
    }
}
