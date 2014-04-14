using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

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

        public static Tile GetTile (Point p)
        {
            return Tiles[p.X, p.Y];
        }

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


        public static void LoadFromXML(string filePath)
        {
            
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            // setup the tile array
            XmlNode tiles = doc.GetElementsByTagName("Tiles")[0];
            int w = int.Parse(tiles.Attributes["Width"].Value);
            int h = int.Parse(tiles.Attributes["Height"].Value);

            Tiles = new Tile[w, h];

            // Add the tiles
            XmlNodeList tileList = doc.GetElementsByTagName("Tile");
            foreach(XmlNode node in tileList)
            {
                Tile t = Tile.LoadTileFromXml(node);

                Tiles[t.Position.X, t.Position.Y] = t;
            }






            //int currentRow = 0;

            //using(XmlReader reader = XmlReader.Create(filePath))
            //{
            //    reader.MoveToContent();
            //    while(reader.Read())
            //    {
            //        if(reader.IsStartElement())
            //        {
            //            switch(reader.Name.ToUpper())
            //            {
            //                case "TILEMAP":
            //                    break;

            //                case "TILES":
            //                    string width = reader.GetAttribute("Width");
            //                    string height = reader.GetAttribute("Height");

            //                    int w = int.Parse(width);
            //                    int h = int.Parse(height);

            //                    Tiles = new Tile[w, h];

            //                    break;

                                
            //                case "ROW":

            //                    string y = reader.GetAttribute("Y");
            //                    currentRow = int.Parse(y);

            //                    break;

            //                case "TILE":

            //                    string tID = reader.GetAttribute("TextureID");
            //                    int x = int.Parse(reader.GetAttribute("X"));
                                
            //                    //Tiles[x, currentRow] = new Tile(int.Parse(tID), x, currentRow);
            //                    Tiles[x, currentRow] = new Tile(1, x, currentRow);


            //                    break;

            //                case "GameObject":


            //                    break;
            //            }


            //        }
            //    }
            //}
        }


        public static void SaveToXML(string filePath)
        {
            XmlDocument doc = new XmlDocument();

            doc.AppendChild(doc.CreateNode(XmlNodeType.XmlDeclaration, "", ""));

            XmlElement tileMap = doc.CreateElement("TileMap");            

            XmlElement tiles = doc.CreateElement("Tiles");
            tiles.SetAttribute("Height", Height.ToString());
            tiles.SetAttribute("Width", Width.ToString());

            for (int y = 0; y < Height; y++)
            {
                XmlElement row = doc.CreateElement("Row");
                row.SetAttribute("Y", y.ToString());

                for (int x = 0; x < Width; x++)
                {
                    row.AppendChild(Tiles[x, y].GetTileXml(doc));
                }
                tiles.AppendChild(row);
            }
            
            tileMap.AppendChild(tiles);
            doc.AppendChild(tileMap);

            
            doc.Save(filePath);

            //using (XmlWriter writer = XmlWriter.Create(filePath))
            //{
            //    writer.WriteStartDocument();

            //    writer.WriteStartElement("TileMap");
            //    writer.WriteStartElement("Tiles");
            //    writer.WriteAttributeString("Height", Height.ToString());
            //    writer.WriteAttributeString("Width", Width.ToString());


            //    for (int y = 0; y < Height; y++)
            //    {
            //        writer.WriteStartElement("Row");
            //        writer.WriteAttributeString("Y", y.ToString());

            //        for (int x = 0; x < Width; x++)
            //        {
            //            Tile t = Tiles[x, y];

            //            writer.WriteStartElement("Tile"); 
                        
            //            writer.WriteAttributeString("TextureID", t.GroundTextureID.ToString());
            //            //writer.WriteAttributeString("Y", y.ToString());
            //            writer.WriteAttributeString("X", x.ToString());


            //            //TODO ADD the game objects, do a save in the Tile object?
            //            //foreach(GameObject go in )

            //            //Tile end
            //            writer.WriteEndElement();
            //        }
            //        //Row end
            //        writer.WriteEndElement();
            //    }
            //    //Tiles End
            //    writer.WriteEndElement();

            //    //TileMap End
            //    writer.WriteEndElement();

            //    //writer.Flush();
            //    writer.WriteEndDocument();
            //}
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

        
        public static List<Tile> GetTileArea(Point bottomCenter, Size size)
        {
            return GetTileArea(bottomCenter, size.Width, size.Height);
        }

        /// <summary>
        /// Get the tiles from the bottom center going outward.
        /// </summary>
        /// <param name="bottomCenter"></param>
        /// <param name="objWidth">Number of tile wide.</param>
        /// <param name="objHeight">Number of tile high.</param>
        /// <returns></returns>
        public static List<Tile> GetTileArea(Point bottomCenter, int objWidth, int objHeight)
        {
            List<Tile> tiles = new List<Tile>();

            for (int x = -(objWidth / 2); x <= (objWidth / 2); x++)
            {
                for (int y = -(objHeight - 1); y <= 0; y++)
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

        public static void PlaceGameObjectRandomly(GameObject obj)
        {
            Random rand = new Random();
            int xPos = 0;
            int yPos = 0;
            List<Tile> tiles;
            Size size = obj.GetTileSize();
            do
            {
                xPos = rand.Next(Width - 1);
                yPos = rand.Next(Height - 1);

                
                tiles = GetTileArea(new Point(xPos, yPos), size);

            } while (tiles == null || !tiles.All(t => !t.IsSolid));

#if DEBUG

            Tiles[xPos, yPos].GroundTextureID = 1;

            Console.WriteLine(
                string.Format("{0} built on [{1} ; {2}]. {3} wide, {4} high.",
                obj.GetType(),
                xPos,
                yPos,
                size.Width,
                size.Height));

#endif

           

            Tiles[xPos, yPos].AddObject(obj);
        }


        //public static T FindClosest<T>(Point origin, int maxDistance = 50) where T : GameObject
        //{
        //    return Find<T>(origin, 1, maxDistance);
        //}

        //// Original idea from
        //// Could be optimized or completly remaded
        //// http://stackoverflow.com/questions/3330181/algorithm-for-finding-nearest-object-on-2d-grid
        //public static T Find<T>(Point origin, int position, int maxDistance = 50) where T : GameObject
        //{
        //    if (position <= 0 || maxDistance <= 0)
        //        throw new ArgumentException();

        //    int minX = 0;
        //    int minY = 0;
        //    int maxX = Width - 1;
        //    int maxY = Height - 1;

        //    int count = 0;

        //    origin.X = (int)MathHelper.Clamp(origin.X, minX, maxX);
        //    origin.Y = (int)MathHelper.Clamp(origin.Y, minY, maxY);
        //    T originT = Tiles[origin.X, origin.Y].GetFirstObject<T>();
        //    if (originT != null) return originT;

        //    for (int d = 1; d < maxDistance; d++)
        //    {
        //        for (int i = 0; i < d + 1; i++)
        //        {
        //            Point point1 = new Point(origin.X - d + i, origin.Y - i);
        //            point1.X = (int)MathHelper.Clamp(point1.X, minX, maxX);
        //            point1.Y = (int)MathHelper.Clamp(point1.Y, minY, maxY);
        //            T t1 = Tiles[point1.X, point1.Y].GetFirstObject<T>();
        //            if (t1 != null)
        //            {
        //                count++;
        //                if (position == count) return t1;
        //            }

        //            Point point2 = new Point(origin.X + d - i, origin.Y + i);
        //            point2.X = (int)MathHelper.Clamp(point2.X, minX, maxX);
        //            point2.Y = (int)MathHelper.Clamp(point2.Y, minY, maxY);
        //            T t2 = Tiles[point2.X, point2.Y].GetFirstObject<T>();
        //            if (t2 != null)
        //            {
        //                count++;
        //                if (position == count) return t2;
        //            }
        //        }

        //        for (int i = 1; i < d; i++)
        //        {
        //            Point point1 = new Point(origin.X - i, origin.Y + d - i);
        //            point1.X = (int)MathHelper.Clamp(point1.X, minX, maxX);
        //            point1.Y = (int)MathHelper.Clamp(point1.Y, minY, maxY);
        //            T t1 = Tiles[point1.X, point1.Y].GetFirstObject<T>();
        //            if (t1 != null)
        //            {
        //                count++;
        //                if (position == count) return t1;
        //            }

        //            Point point2 = new Point(origin.X + d - i, origin.Y - i);
        //            point2.X = (int)MathHelper.Clamp(point2.X, minX, maxX);
        //            point2.Y = (int)MathHelper.Clamp(point2.Y, minY, maxY);
        //            T t2 = Tiles[point2.X, point2.Y].GetFirstObject<T>();
        //            if (t2 != null)
        //            {
        //                count++;
        //                if (position == count) return t2;
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
}
