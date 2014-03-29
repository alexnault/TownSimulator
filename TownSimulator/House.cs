﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace TownSimulator
{
    public class House : GameObject
    {


        public House()
            :base()
        {



        }


        public static void AddToTiles(House house, List<Tile> tiles)
        {
            Tile finalTile = GetLowerRightile(tiles);

            tiles.Remove(finalTile);

            foreach (Tile t in tiles)
            {
                t.AddObject(house);
            }
        }

        private static Tile GetLowerRightile(List<Tile> tiles)
        {
            Tile lrTile = null;
            int mostRight = -1;
            int mostDown = -1;

            foreach (Tile t in tiles)
            {
                if (t.Position.X > mostRight && t.Position.Y > mostDown)
                {
                    mostDown = t.Position.Y;
                    mostRight = t.Position.X;
                    
                    lrTile = t;
                }

            }

            return lrTile;
        }



    }
}