using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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
                    
                    Tiles[x, y] = new Tile(textureIndex);
                }
            }

        }

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
            spriteBatch.Begin(
               SpriteSortMode.BackToFront,
               BlendState.AlphaBlend,
               null,
               null,
               null,
               null,
               camera.TransformMatrix);

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
                            Color.White);
                    }

                    //Draws the game objects within the Tile object
                    Tiles[x, y].Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }

    }
}
