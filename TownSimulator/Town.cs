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
            Villagers = new Dictionary<int, Villager>();

            GenerateTown();
        }

        private void GenerateTown()
        {
            GenerateBuildings();
            GenerateVillagers();
        }

        private void GenerateVillagers()
        {
            Random rand = new Random();

            int nbWoodcutter = rand.Next(1, 4);
            for (int i = 0; i < nbWoodcutter; i++)
            {
                TileMap.PlaceGameObjectRandomly(new Woodcutter(this));
            }

            //Place a Carrier for each LumberMill there is in the Town
            foreach (Building b in Buildings)
            {
                if (b is LumberMill)
                {
                    TileMap.PlaceGameObjectRandomly(
                          new Carrier(
                              this,
                              (LumberMill)b)
                          );
                }
            }
        }

        private void GenerateBuildings()
        {
            Random rand = new Random();

            int nbLM = rand.Next(1, 3);
            for (int i = 0; i < nbLM; i++)
            {
                LumberMill lumberMill = new LumberMill(this);
                TileMap.PlaceGameObjectRandomly(lumberMill);
                TileMap.PlaceGameObjectRandomly(new Carrier( this,  lumberMill) );
            }

            int nbHouse = rand.Next(0, 3);
            for (int i = 0; i < nbHouse; i++)
            {
                TileMap.PlaceGameObjectRandomly(new House(this));
            }

            int nbBuildSite = rand.Next(1, 2);
            for (int i = 0; i < nbBuildSite; i++)
            {
                TileMap.PlaceGameObjectRandomly(new ConstructionSite(this));
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

        public void AddBuilding(Building building)
        {
            Buildings.Add(building);
        }
    }
}
