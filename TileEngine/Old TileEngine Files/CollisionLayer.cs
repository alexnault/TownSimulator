using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace TileEngine
{
    public class CollisionLayer
    {

        int[,] map;

        public int Width
        {
            get { return map.GetLength(1); }
        }
        public int Height
        {
            get { return map.GetLength(0); }
        }
        
        public CollisionLayer(int height, int width)
        {
            map = new int[height, width];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)               
                    map[y, x] = -1;
        }


        public void Save(string filename, string[] textureNames)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine("[Layout]");

                for (int y = 0; y < Height; y++)
                {
                    string line = string.Empty;

                    for (int x = 0; x < Width; x++)
                    {
                        line += map[y, x].ToString() + " ";

                    }
                    writer.WriteLine(line);
                }
            }
        }



        public static CollisionLayer FromFile(string filename)
        {
            CollisionLayer CollisionLayer;
            List<List<int>> tempLayout = new List<List<int>>(); //Le layout

            //Using veut dire qu'on peut juste l'utiliser à l'intérieur des bracket
            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingLayout = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim(); //Enlève les espaces

                    if (string.IsNullOrEmpty(line))  //Si la ligne est vide
                        continue;   //Retourne au début du while

                    if (line.Contains("[Layout]"))    //Si la ligne a lu le Tag [Textures]
                    {
                        readingLayout = true;
                    }
                    else if (readingLayout)
                    {
                        List<int> row = new List<int>();
                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            if (!string.IsNullOrEmpty(c))   //Au cas ou il y ait des espaces de trop
                                row.Add(int.Parse(c));  //Ajoute le string en Int
                        }
                        tempLayout.Add(row);
                    }
                }
            }
            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            CollisionLayer = new CollisionLayer(height, width);      //Height and Width sont interchangé, car sinon sa plante, no idea why



            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    CollisionLayer.SetCellIndex(x, y, tempLayout[y][x]);
                }
            }
            return CollisionLayer;
        }
                        
        public int GetCellIndex(int x, int y)
        {
            return map[y, x];
        }

        public int GetCellIndex(Vector2 cell)
        {
            return map[(int)cell.Y, (int)cell.X];
        }

        public int GetCellIndex(Point point)
        {
            return map[point.Y, point.X];
        }

        //Sert a setter une texture dans une cell
        public void SetCellIndex(int x, int y, int cellIndex)
        {
            map[y, x] = cellIndex;      //Met l'index de la texture au bon endroit dans l'array 
        }
        public void SetCellIndex(Point point, int cellIndex)
        {
            map[point.Y, point.X] = cellIndex;
        }

        public void ReplaceIndex(int existingIndex, int newIndex)
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (map[y, x] == existingIndex)
                        map[y, x] = newIndex;
        }

        public void RemoveIndex(int existingIndex)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (map[y, x] == existingIndex)
                        map[y, x] = -1;
                    else if (map[y, x] > existingIndex)
                        map[y, x]--;
                }
            }
        }
    }
}
