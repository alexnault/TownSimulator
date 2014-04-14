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

        Town town;

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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            camera = new Camera();
            GenerateMap();
            town = new Town(1);
			//string path = "test.xml";

            GodMode.ShowCommands();

            //TileMap.SaveToXML(path);
            //TileMap.LoadFromXML(path);
            TextureManager.Initialize();
            DrawingUtils.Initialize(spriteBatch, camera);

            base.Initialize();
        }
        
        private void GenerateMap()
        {
            //Static generation of the tile map
            int w = 30;
            int h = 30;
            int[,] textIndexes = new int[w, h];

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    textIndexes[i, j] = 0;

            TileMap.Initialize(textIndexes);


            //GameObject[] solidObjects = new GameObject[h - 1];
            //for (int i = 0; i < h - 1; i++)
            //{
            //    solidObjects[i] = new GameObject()
            //    {
            //        IsSolid = true,
            //        ObjectSprite = new Sprite(6)
            //    };
            //}

            //for (int y = 0; y < h - 1; y++)
            //{
            //    Tile t = TileMap.Tiles[1, y];
            //   // solidObjects[y].Position = new Point(1, y);
            //    t.AddObject(solidObjects[y]);
            //}

            //TileMap.Tiles[7, 8].AddObject( new Items.WoodPile() );
            
            //GameObject.PlaceBigObjectCentered(new Buildings.LumberMill(), 
            
            TileMap.Tiles[5, 1].AddObject(new Scenery.Tree());
            TileMap.Tiles[7, 3].AddObject(new Scenery.Tree());
            TileMap.Tiles[2, 7].AddObject(new Scenery.Tree());
            TileMap.Tiles[7, 12].AddObject(new Scenery.Tree());
            TileMap.Tiles[3, 18].AddObject(new Scenery.Tree());
            TileMap.Tiles[9, 15].AddObject(new Scenery.Tree());
        }


        protected override void LoadContent()
        {
            

            TextureManager.Add(Content.Load<Texture2D>("Tiles/grass_small"), 0);
            TextureManager.Add(Content.Load<Texture2D>("Tiles/rock"), 1);
            TextureManager.Add(Content.Load<Texture2D>("Tiles/wood_small"), 2);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/lumberjack_sheet_small"), 3);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/woodpile"), 4);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/trees.png"), 5);      
            TextureManager.Add(Content.Load<Texture2D>("Sprites/rock"), 6);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/house1"), 7);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/lumbermill"), 8);
            TextureManager.Add(Content.Load<Texture2D>("texture1px"), 10);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/carrier_sheet_small"), 11);
            

            DrawingUtils.Font = Content.Load<SpriteFont>("Fonts/font1");
              
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {   
            InputHelper.Update(camera);

            GodMode.Update(gameTime, camera, town);
            UpdateCameraMovement();
            //UpdateObjectMovement(gameTime);

            TileMap.Update(gameTime);
            town.Update(gameTime);
            
            base.Update(gameTime);
        }
        
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

            spriteBatch.Begin(
               SpriteSortMode.BackToFront,
               BlendState.NonPremultiplied,
               null,
               null,
               null,
               null,
               camera.TransformMatrix);

            TileMap.Draw(spriteBatch, camera);
            town.Draw(spriteBatch);
            GodMode.Draw();
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
