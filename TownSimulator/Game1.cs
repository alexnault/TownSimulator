#region Using Statements
using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using TileEngine;
#endregion

namespace TownSimulator
{
 
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Camera camera;
        

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
        }
         
        protected override void Initialize()
        {

            TextureManager.Initialize();
            camera = new Camera();


            int w = 10;
            int h = 10;
            int[,] textIndexes = new int[w, h];

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    textIndexes[i, j] = 0;

            TileMap.Initialize(textIndexes);

            GameObject obj = new GameObject()
            {
                ObjectSprite = new Sprite(1)
            };

                       
            TileMap.Tiles[0, 0].Objects.Add(obj);


            //////////////////////////////////////////
            ///Testing

            GameObject[] solidObjects = new GameObject[20];
            for (int i = 0; i < 20; i++)
            {
                solidObjects[i] = new GameObject()
                {
                    IsSolid = true,
                    ObjectSprite = new Sprite(2)
                };
            }

            for (int y = 0; y < h - 1; y++)
            {
                Tile t = TileMap.Tiles[1, y];
                solidObjects[y].Position = new Vector2(1 * Engine.TileWidth, y * Engine.TileHeight);
                t.Objects.Add(solidObjects[y]);
            }
            
            Point destPoint = new Point(7, 5);

            obj.path = Pathfinding.DoAStar(Engine.ConvertCellToPosition(destPoint), obj.Position);
            ////////////////////////////////////////////////////


            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureManager.Add(Content.Load<Texture2D>("Tiles/grass"), 0);
            TextureManager.Add(Content.Load<Texture2D>("Tiles/rock"), 1);
            TextureManager.Add(Content.Load<Texture2D>("Tiles/wood"), 2);

        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {   
            InputHelper.Update();

            UpdateCameraMovement();
            //UpdateObjectMovement(gameTime);

            TileMap.Update(gameTime);            
           
            
            base.Update(gameTime);
        }

        //private void UpdateObjectMovement(GameTime gameTime)
        //{

        //    Point motion = Point.Zero;
        //    int speed = 1;

        //    if (InputHelper.IsKeyDown(Keys.W))
        //        motion.Y--;
        //    if (InputHelper.IsKeyDown(Keys.S))
        //        motion.Y++;
        //    if (InputHelper.IsKeyDown(Keys.A))
        //        motion.X--;
        //    if (InputHelper.IsKeyDown(Keys.D))
        //        motion.X++;


        //    obj.Position = new Point(obj.Position.X + motion.X * speed, obj.Position.Y + motion.Y * speed);


        //    obj.Update(gameTime);

        //}

        private void UpdateCameraMovement()
        {
            Vector2 motion = Vector2.Zero;

            if (InputHelper.IsKeyDown(Keys.Up))
                motion.Y--;
            if (InputHelper.IsKeyDown(Keys.Down))
                motion.Y++;
            if (InputHelper.IsKeyDown(Keys.Left))
                motion.X--;
            if (InputHelper.IsKeyDown(Keys.Right))
                motion.X++;

            camera.Position += motion * camera.Speed;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            TileMap.Draw(spriteBatch, camera);

            //spriteBatch.Begin(
            //   SpriteSortMode.Texture,
            //   BlendState.AlphaBlend,
            //   null,
            //   null,
            //   null,
            //   null,
            //   camera.TransformMatrix);

            ////obj.Draw(spriteBatch);

            //spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
