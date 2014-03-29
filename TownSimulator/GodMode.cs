using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;

namespace TownSimulator
{
    public static class GodMode
    {
        public enum ObjectToAdd
        {
            Tree,
            Woodcutter,
            WoodPile
        }

        public static ObjectToAdd State { get; private set; }

        //public static void Update(GameTime gameTime, Camera camera, Town town)
        public static void Update(GameTime gameTime, Camera camera, Town town)
        {
            HandleKeyboardClick(town);
            HandleMouseClick(camera, town);
        }

        private static void HandleKeyboardClick(Town town)
        {
            if (InputHelper.IsNewKeyPressed(Keys.T))
            {
                State = ObjectToAdd.Tree;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.W))
            {
                State = ObjectToAdd.WoodPile;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.V))
            {
                State = ObjectToAdd.Woodcutter;
            }
            else if(InputHelper.IsNewKeyPressed(Keys.S))
            {
                foreach (TownSimulator.Villagers.Villager v in town.Villagers)
                    v.MakeDecision.Release();
            }


        }

        private static void HandleMouseClick(Camera camera, Town town)
        {
            Rectangle mapRect = new Rectangle(0, 0, TileMap.Width * Engine.TileWidth, TileMap.Height * Engine.TileHeight);

            if (InputHelper.IsLeftMouseButtonClicked() && InputHelper.IsMouseColliding(mapRect))
            {

                Point tilePos = Engine.ConvertPositionToCell(InputHelper.MousePosition + camera.Position);

                if (tilePos.X > 0 && tilePos.X < TileMap.Width && tilePos.Y > 0 && tilePos.Y < TileMap.Height)
                {
                    switch (State)
                    {
                        case ObjectToAdd.Tree:
                            TileMap.Tiles[tilePos.X, tilePos.Y].AddObject(new TownSimulator.Scenery.Tree());
                            break;

                        case ObjectToAdd.Woodcutter:
                            TileMap.Tiles[tilePos.X, tilePos.Y].AddObject(new TownSimulator.Villagers.Woodcutter("The", "Woodcutter", town));
                            break;

                        case ObjectToAdd.WoodPile:
                            TileMap.Tiles[tilePos.X, tilePos.Y].AddObject(new TownSimulator.Items.WoodPile());

                            break;
                    }

                }

            }
        }

    }
}
