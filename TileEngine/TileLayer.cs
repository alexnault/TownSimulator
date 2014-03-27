using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace TileEngine
{
    public class TileLayer
    {


        List<Texture2D> tileTextures = new List<Texture2D>();
        int[,] map;
        float alpha = 1f;

        public float Alpha
        {
            get { return alpha; }
            set
            {
                alpha = MathHelper.Clamp(value, 0f, 1f);    //0f = minimum, 1f = maximum
            }
        }
        public int WidthInPixels
        {
            get { return Width * Engine.TileWidth; }
        }
        public int HeightInPixels
        {
            get { return Height * Engine.TileHeight; }
        }
        public int Width
        {
            get { return map.GetLength(1); }
        }
        public int Height
        {
            get { return map.GetLength(0); }
        }

        public TileLayer(int height, int width)
        {
            map = new int[height, width];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map[y, x] = -1;
        }


        public TileLayer(int[,] existingMap)
        {
            map = (int[,])existingMap.Clone();  //Clone la map envoyé en paramètre
        }

        public int IsUsingTexture(Texture2D texture)
        {
            if (tileTextures.Contains(texture))
                return tileTextures.IndexOf(texture);   //Renvois l'index de la texture

            return -1;      //Indique que l'on n'utilise pas la texture
        }


        public void Save(string filename, string[] textureNames)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                //Écriture du nom des textures
                writer.WriteLine("[Textures]");
                foreach (string t in textureNames)
                    writer.WriteLine(t);

                writer.WriteLine();

                writer.WriteLine("[Properties]");
                writer.WriteLine("Alpha = " + Alpha.ToString());

                writer.WriteLine();

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


        public static TileLayer FromFile(string filename, out string[] textureNameArray) //Pour ouvrir un fichier dans l'éditeur
        {
            TileLayer tileLayer;
            List<string> texturesNames = new List<string>();    //Les textures


            tileLayer = ProcessFile(filename, texturesNames);

            textureNameArray = texturesNames.ToArray();

            return tileLayer;
        }

        //Pour ouvrir un fichier dans le jeu
        public static TileLayer FromFile(ContentManager content, string filename)
        {
            TileLayer tileLayer;

            List<string> texturesNames = new List<string>();    //Les textures

            tileLayer = ProcessFile(filename, texturesNames);

            //Load les textures d'après le contenu dans l'array
            tileLayer.LoadTileTextures(content, texturesNames.ToArray());

            return tileLayer;
        }

        private static TileLayer ProcessFile(string filename, List<string> texturesNames)
        {
            TileLayer tileLayer;
            List<List<int>> tempLayout = new List<List<int>>(); //Le layout
            Dictionary<string, string> propertiesDict = new Dictionary<string, string>();

            //Using veut dire qu'on peut juste l'utiliser à l'intérieur des bracket
            using (StreamReader reader = new StreamReader(filename))
            {
                bool readingTextures = false;
                bool readingLayout = false;
                bool readingProperties = false;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim(); //Enlève les espaces

                    if (string.IsNullOrEmpty(line))  //Si la ligne est vide
                        continue;   //Retourne au début du while

                    if (line.Contains("[Textures]"))    //Si la ligne a lu le Tag [Textures]
                    {
                        readingTextures = true;
                        readingLayout = false;
                        readingProperties = false;
                    }
                    else if (line.Contains("[Layout]"))    //Si la ligne a lu le Tag [Textures]
                    {
                        readingTextures = false;
                        readingLayout = true;
                        readingProperties = false;
                    }
                    else if (line.Contains("[Properties]"))
                    {
                        readingProperties = true;
                        readingTextures = false;
                        readingLayout = false;
                    }
                    else if (readingTextures)
                        texturesNames.Add(line);
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
                    else if (readingProperties)
                    {
                        string[] pair = line.Split('=');
                        string key = pair[0].Trim();
                        string value = pair[1].Trim();

                        propertiesDict.Add(key, value);
                    }

                }
            }
            int width = tempLayout[0].Count;
            int height = tempLayout.Count;

            tileLayer = new TileLayer(height, width);      //Height and Width sont interchangé, car sinon sa plante, no idea why

            foreach (KeyValuePair<string, string> property in propertiesDict)
            {
                switch (property.Key)
                {
                    case "Alpha":
                        tileLayer.Alpha = float.Parse(property.Value);
                        break;
                }


            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tileLayer.SetCellIndex(x, y, tempLayout[y][x]);
                }
            }
            return tileLayer;
        }

        //params permet d'ajouter plein de paramètre du meme type ex. 3 texture names (string)
        public void LoadTileTextures(ContentManager Content, params string[] textureNames)
        {
            Texture2D texture;

            foreach (string textureName in textureNames)
            {
                texture = Content.Load<Texture2D>(textureName);
                tileTextures.Add(texture);
            }
        }

        public void AddTexture(Texture2D texture)
        {
            tileTextures.Add(texture);
        }

        public void RemoveTexture(Texture2D texture)
        {
            RemoveIndex(tileTextures.IndexOf(texture));  //Remplace tous les index de la texture à enlever par -1
            tileTextures.Remove(texture);
        }


        public int GetCellIndex(int x, int y)
        {
            return map[y, x];
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

        //Pour l'editor
        public void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Begin(
                SpriteSortMode.Texture,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                null,
                camera.TransformMatrix);


            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int textureIndex = map[y, x];   //y et x sont inversés intentionnelement

                    if (textureIndex == -1)
                        continue;   //Recommence le for

                    Texture2D texture = tileTextures[textureIndex];


                    spriteBatch.Draw(
                        texture,
                        new Rectangle(
                            x * Engine.TileWidth,
                            y * Engine.TileHeight,
                            Engine.TileWidth,
                            Engine.TileHeight),
                        new Color(new Vector4(1f, 1f, 1f, Alpha)));
                }

            }
            spriteBatch.End();

        }

        //Pour le jeu (TileMap)
        public void Draw(SpriteBatch spriteBatch, Camera camera, Point min, Point max)
        {
            spriteBatch.Begin(
               SpriteSortMode.Texture,
               BlendState.AlphaBlend,
               null,
               null,
               null,
               null,
               camera.TransformMatrix);

            min.X = (int)Math.Max(min.X, 0);
            min.Y = (int)Math.Max(min.Y, 0);
            max.X = (int)Math.Min(max.X, Width);
            max.Y = (int)Math.Min(max.Y, Height);


            for (int x = min.X; x < max.X; x++)
            {
                for (int y = min.Y; y < max.Y; y++)
                {
                    int textureIndex = map[y, x];   //y et x sont inversés intentionnelement

                    if (textureIndex == -1)
                        continue;   //Recommence le for

                    Texture2D texture = tileTextures[textureIndex];

                    spriteBatch.Draw(
                        texture,
                        new Rectangle(
                            x * Engine.TileWidth,
                            y * Engine.TileHeight,
                            Engine.TileWidth,
                            Engine.TileHeight),
                        new Color(new Vector4(1f, 1f, 1f, Alpha)));
                }

            }
            spriteBatch.End();

        }




    }
}
