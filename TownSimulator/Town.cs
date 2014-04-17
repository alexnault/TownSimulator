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
    class Town
    {
        public Dictionary<int, Villager> Villagers { get; private set; }
        //public Villager[] Villagers { get; private set; }
        public int NbVillagers { get; private set; }

        public Semaphore AITurn;

        public List<Building> Buildings { get; set; }

        public Town(bool randomlyGenerated)
        {
            NbVillagers = 0;

            AITurn = new Semaphore(1, 1);

            Buildings = new List<Building>();
            Villagers = new Dictionary<int, Villager>();

            GenerateTown(randomlyGenerated);
        }

        private void GenerateTown(bool random)
        {
            GenerateBuildings(random);
            GenerateVillagers(random);
        }

        

        private void GenerateBuildings(bool random)
        {

            if (random)
            {
                Random rand = new Random();

                int nbLM = 1;// rand.Next(1, 3);
                for (int i = 0; i < nbLM; i++)
                {
                    LumberMill lm = new LumberMill(this);
                    TileMap.PlaceGameObjectRandomly(lm);
                    TileMap.PlaceGameObjectRandomly(new Carrier(this, lm));
                }

                int nbHouse = rand.Next(0, 3);
                for (int i = 0; i < nbHouse; i++)
                {
                    TileMap.PlaceGameObjectRandomly(new House(this));
                }

                //int nbBuildSite = rand.Next(1, 2);
                //for (int i = 0; i < nbBuildSite; i++)
                //{
                //    TileMap.PlaceGameObjectRandomly(new ConstructionSite(this));
                //}
            }
            else
            {
                TileMap.Tiles[18, 10].AddObject(new LumberMill(this, false));
                TileMap.Tiles[15, 10].AddObject(new House(this, false));
                TileMap.Tiles[21, 10].AddObject(new House(this, false));
                TileMap.Tiles[16, 14].AddObject(new House(this, false));
                TileMap.Tiles[22, 14].AddObject(new House(this, false));
                TileMap.Tiles[29, 10].AddObject(new House(this, true));
                //TileMap.Tiles[29, 10].AddObject(new ConstructionSite(this));

                // TODO refactor to Rock object
                TileMap.Tiles[23, 10].AddObject(new GameObject() { ObjectSprite = new Sprite(6), IsSolid = true });
                TileMap.Tiles[24, 10].AddObject(new GameObject() { ObjectSprite = new Sprite(6), IsSolid = true });
                TileMap.Tiles[26, 10].AddObject(new GameObject() { ObjectSprite = new Sprite(6), IsSolid = true });
                TileMap.Tiles[27, 10].AddObject(new GameObject() { ObjectSprite = new Sprite(6), IsSolid = true });
            }
        }

        private void GenerateVillagers(bool random)
        {
            if (random)
            {
                Random rand = new Random();

                int nbWoodcutter = rand.Next(1, 4);
                for (int i = 0; i < nbWoodcutter; i++)
                {
                    TileMap.PlaceGameObjectRandomly(new Woodcutter(this));
                }

                ////Place a Carrier for each LumberMill there is in the Town
                //foreach (Building b in Buildings)
                //{
                //    if (b is LumberMill)
                //    {
                //        TileMap.PlaceGameObjectRandomly(
                //            new Carrier(this, (LumberMill)b));
                //    }
                //}
            }
            else
            {
                for(int i = 0; i < 5; i++)
                {
                    TileMap.Tiles[0, 0].AddObject(new Woodcutter(this));
                }
                
                TileMap.Tiles[0, 0].AddObject(
                    new Carrier(
                        this,
                        (LumberMill)Buildings.FirstOrDefault(x => x.GetType() == typeof(LumberMill)))
                        );
                TileMap.Tiles[0, 0].AddObject(new Builder(this));
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
