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
        
        TileMap tileMap;
        Camera camera;
        GameObject obj;

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
            tileMap = new TileMap();
            camera = new Camera();
            obj = new GameObject();
            obj.Position = new Point(5, 5);

            
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load un fichier de layer
            tileMap.Layers.Add(TileLayer.FromFile(Content, "Content/Layers/Layer.layer"));
            //Load un fichier de collision
            tileMap.CollisionLayer = CollisionLayer.FromFile("Content/Layers/Collision.collayer");


            Texture2D spriteText = Content.Load<Texture2D>("Tiles/rock");
            obj.ObjectSprite = new Sprite(spriteText);

        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {   
            InputHelper.Update();

            UpdateCameraMovement();
            UpdateObjectMovement(gameTime);
           
            
            base.Update(gameTime);
        }

        private void UpdateObjectMovement(GameTime gameTime)
        {

            Point motion = Point.Zero;
            int speed = 1;

            if (InputHelper.IsKeyDown(Keys.W))
                motion.Y--;
            if (InputHelper.IsKeyDown(Keys.S))
                motion.Y++;
            if (InputHelper.IsKeyDown(Keys.A))
                motion.X--;
            if (InputHelper.IsKeyDown(Keys.D))
                motion.X++;


            obj.Position = new Point(obj.Position.X + motion.X * speed, obj.Position.Y + motion.Y * speed);


            obj.Update(gameTime);
            //obj.IsSolid = true;

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

            
            tileMap.Draw(spriteBatch, camera);

            spriteBatch.Begin(
               SpriteSortMode.Texture,
               BlendState.AlphaBlend,
               null,
               null,
               null,
               null,
               camera.TransformMatrix);

            obj.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
