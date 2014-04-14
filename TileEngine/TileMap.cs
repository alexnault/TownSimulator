using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public static class TileMap
    {
        public static int Width 
        {
            get { return Tiles.GetLength(0); } 
        }
        public static int Height
        {
            get { return Tiles.GetLength(1); }
        }
        public static Tile[,] Tiles { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        ///<param name="layout">Index of the textures for each of the tiles.</param>    
        public static void Initialize(int[,] layout)
        {
            int width = layout.GetLength(0);
            int height = layout.GetLength(1);

            Tiles = new Tile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int textureIndex = layout[x, y];
                    
                    Tiles[x, y] = new Tile(textureIndex, x, y);
                }
            }

        }

        //public static void IntializeFromFile(string filePath)
        //{
        //    int[,] layout;

        //    //Load from file

        //    using(StreamReader reader = new StreamReader(filePath))
        //    {
        //        bool readingTextures = false;
        //        bool readingLayout = false;
        //        bool readingProperties = false;

        //        while (!reader.EndOfStream)
        //        {
        //            string line = reader.ReadLine().Trim(); //Enlève les espaces

        //            if (string.IsNullOrEmpty(line))  //Si la ligne est vide
        //                continue;   //Retourne au début du while

        //            if (line.Contains("[Textures]"))    //Si la ligne a lu le Tag [Textures]
        //            {
        //                readingTextures = true;
        //                readingLayout = false;
        //                readingProperties = false;
        //            }
        //            else if (line.Contains("[Layout]"))    //Si la ligne a lu le Tag [Textures]
        //            {
        //                readingTextures = false;
        //                readingLayout = true;
        //                readingProperties = false;
        //            }
        //            else if (line.Contains("[Properties]"))
        //            {
        //                readingProperties = true;
        //                readingTextures = false;
        //                readingLayout = false;
        //            }
        //            else if (readingTextures)
        //                texturesNames.Add(line);
        //            else if (readingLayout)
        //            {
        //                List<int> row = new List<int>();
        //                string[] cells = line.Split(' ');

        //                foreach (string c in cells)
        //                {
        //                    if (!string.IsNullOrEmpty(c))   //Au cas ou il y ait des espaces de trop
        //                        row.Add(int.Parse(c));  //Ajoute le string en Int
        //                }
        //                tempLayout.Add(row);
        //            }
        //            else if (readingProperties)
        //            {
        //                string[] pair = line.Split('=');
        //                string key = pair[0].Trim();
        //                string value = pair[1].Trim();

        //                propertiesDict.Add(key, value);
        //            }

        //        }
        //    }


        //    Initialize(layout);


        //}

        public static void Update(GameTime gameTime)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {   
                    Tiles[x, y].Update(gameTime);
                }
            }
        }

        public static void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            //spriteBatch.Begin(
            //   SpriteSortMode.BackToFront,
            //   BlendState.AlphaBlend,
            //   null,
            //   null,
            //   null,
            //   null,
            //   camera.TransformMatrix);

            for(int x = 0; x < Width ; x++)
            {
                for(int y = 0 ; y < Height; y++)
                {
                    Texture2D texture = TextureManager.Get(Tiles[x, y].GroundTextureID);
                    if (texture != null)
                    {
                        spriteBatch.Draw(
                            texture,
                            new Rectangle(x * Engine.TileWidth, y * Engine.TileHeight, Engine.TileWidth, Engine.TileHeight),
                            null,
                            Color.White,
                            0,
                            new Vector2(0,0),
                            SpriteEffects.None,
                            1);
                    }

                    //Draws the game objects within the Tile object
                    Tiles[x, y].Draw(spriteBatch);
                }
            }

            //spriteBatch.End();
        }

        


        /// <summary>
        /// Get the tiles from the bottom center going outward.
        /// </summary>
        /// <param name="bottomCenter"></param>
        /// <param name="width">Number of tile wide.</param>
        /// <param name="height">Number of tile high.</param>
        /// <returns></returns>
        public static List<Tile> GetTileArea(Point bottomCenter, int width, int height)
        {
            List<Tile> tiles = new List<Tile>();

            for (int x = -(width / 2); x <= (width / 2); x++)
            {
                for (int y = -(height - 1); y <= 0; y++)
                {
                    int posX = bottomCenter.X + x;
                    int posY = bottomCenter.Y + y;

                    if (posX < 0 ||
                        posY < 0 ||
                        posX >= Width ||
                        posY >= Height)
                    {
                        return null;
                    }

                    tiles.Add(Tiles[posX, posY]);
                }
            }
            return tiles;
        }


        public static T FindClosest<T>(Point origin, int maxDistance = 50) where T : GameObject
        {
            return Find<T>(origin, 1, maxDistance);
        }

        // Original idea from
        // Could be optimized or completly remaded
        // http://stackoverflow.com/questions/3330181/algorithm-for-finding-nearest-object-on-2d-grid
        public static T Find<T>(Point origin, int position, int maxDistance = 50) where T : GameObject
        {
            if (position <= 0 || maxDistance <= 0)
                throw new ArgumentException();

            int minX = 0;
            int minY = 0;
            int maxX = Width - 1;
            int maxY = Height - 1;

            int count = 0;

            origin.X = (int)MathHelper.Clamp(origin.X, minX, maxX);
            origin.Y = (int)MathHelper.Clamp(origin.Y, minY, maxY);
            T originT = Tiles[origin.X, origin.Y].GetFirstObject<T>();
            if (originT != null) return originT;

            for (int d = 1; d < maxDistance; d++)
            {
                for (int i = 0; i < d + 1; i++)
                {
                    Point point1 = new Point(origin.X - d + i, origin.Y - i);
                    point1.X = (int)MathHelper.Clamp(point1.X, minX, maxX);
                    point1.Y = (int)MathHelper.Clamp(point1.Y, minY, maxY);
                    T t1 = Tiles[point1.X, point1.Y].GetFirstObject<T>();
                    if (t1 != null)
                    {
                        count++;
                        if (position == count) return t1;
                    }

                    Point point2 = new Point(origin.X + d - i, origin.Y + i);
                    point2.X = (int)MathHelper.Clamp(point2.X, minX, maxX);
                    point2.Y = (int)MathHelper.Clamp(point2.Y, minY, maxY);
                    T t2 = Tiles[point2.X, point2.Y].GetFirstObject<T>();
                    if (t2 != null)
                    {
                        count++;
                        if (position == count) return t2;
                    }
                }

                for (int i = 1; i < d; i++)
                {
                    Point point1 = new Point(origin.X - i, origin.Y + d - i);
                    point1.X = (int)MathHelper.Clamp(point1.X, minX, maxX);
                    point1.Y = (int)MathHelper.Clamp(point1.Y, minY, maxY);
                    T t1 = Tiles[point1.X, point1.Y].GetFirstObject<T>();
                    if (t1 != null)
                    {
                        count++;
                        if (position == count) return t1;
                    }

                    Point point2 = new Point(origin.X + d - i, origin.Y - i);
                    point2.X = (int)MathHelper.Clamp(point2.X, minX, maxX);
                    point2.Y = (int)MathHelper.Clamp(point2.Y, minY, maxY);
                    T t2 = Tiles[point2.X, point2.Y].GetFirstObject<T>();
                    if (t2 != null)
                    {
                        count++;
                        if (position == count) return t2;
                    }
                }
            }
            return null;
        }
    }
}
