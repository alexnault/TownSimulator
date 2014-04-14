using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;
using TownSimulator.Buildings;
using TownSimulator.Villagers;

namespace TownSimulator
{
    public class Town
    {
        public Dictionary<int, Villager> Villagers { get; private set; }
        //public Villager[] Villagers { get; private set; }
        public int NbVillagers { get; private set; }

        public Semaphore AITurn;

        public List<Building> Buildings { get; set; }

        public Town(uint initNbVillagers)
        {
            NbVillagers = 0;

            AITurn = new Semaphore(1, 1);

            Buildings = new List<Building>();
            TileMap.Tiles[7, 8].AddObject(new LumberMill(this));
            TileMap.Tiles[10, 18].AddObject(new House(this));
            TileMap.Tiles[20, 5].AddObject(new ConstructionSite(this));

            //Villagers = new Villager[nbVillagers];
            Villagers = new Dictionary<int, Villager>();
            for (int i = 0; i < initNbVillagers; i++)
            {
                // Automaticly added to this town
                TileMap.Tiles[0, 0].AddObject(new Woodcutter("Bob", "Gratton", this));
                //Villagers[i] = new Woodcutter("Bob", "Gratton", this);
            }

            TileMap.Tiles[0, 0].AddObject(
                new Carrier(
                    "Bob", "Gratton",
                    this,
                    (LumberMill)Buildings.FirstOrDefault(x => x.GetType() == typeof(LumberMill))
                )
            );
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

        public void AddBuilding(Building building)
        {
            Buildings.Add(building);
        }
    }
}
