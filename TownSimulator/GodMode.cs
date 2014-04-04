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
            WoodPile,
            Rock,
            House
        }
        private static Dictionary<Tile, Color> tilesToDraw;
        private static ObjectToAdd _state;

        public static void Update(GameTime gameTime, Camera camera, Town town)
        {
            HandleKeyboard(town);
            HandleMouse(camera, town);
        }
        
        private static void HandleKeyboard(Town town)
        {
            if (InputHelper.IsNewKeyPressed(Keys.T))
            {
                _state = ObjectToAdd.Tree;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.W))
            {
                _state = ObjectToAdd.WoodPile;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.V))
            {
                _state = ObjectToAdd.Woodcutter;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.R))
            {
                _state = ObjectToAdd.Rock;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.H))
            {
                _state = ObjectToAdd.House;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.S))
            {
                foreach (KeyValuePair<int, Villagers.Villager> v in town.Villagers)
                    v.Value.MakeDecision.Release();
            }


        }

        private static void HandleMouse(Camera camera, Town town)
        {
            Rectangle mapRect = new Rectangle(0, 0, TileMap.Width * Engine.TileWidth, TileMap.Height * Engine.TileHeight);

            Point tilePos = Engine.ConvertPositionToCell(InputHelper.MousePosition + camera.Position);
            Tile selectedTile;

            if (tilePos.X >= 0 && tilePos.X < TileMap.Width && tilePos.Y >= 0 && tilePos.Y < TileMap.Height)
            {
                selectedTile = TileMap.Tiles[tilePos.X, tilePos.Y];

                bool leftClicked = InputHelper.LeftMouseButtonClicked() && InputHelper.IsMouseOn(mapRect);
                bool leftDown = InputHelper.LeftMouseButtonDown() && InputHelper.IsMouseOn(mapRect);


                tilesToDraw = new Dictionary<Tile, Color>();

                GameObject objToDraw = null;
                bool draw = false;

                //Handle mouse click on map
                switch (_state)
                {
                    case ObjectToAdd.Tree:

                        objToDraw = new TownSimulator.Scenery.Tree();

                        //tilesToDraw.Add(selectedTile, selectedTile.IsSolid ? Color.Red : Color.White);
                        if (leftDown && !selectedTile.IsSolid) draw = true;
                        //{
                        //    //selectedTile.AddObject(new TownSimulator.Scenery.Tree());
                        //}
                        break;

                    case ObjectToAdd.Woodcutter:
                        objToDraw = new TownSimulator.Villagers.Woodcutter("The", "Woodcutter", town);


                        if (leftClicked && !selectedTile.IsSolid) draw = true;

                        break;

                    case ObjectToAdd.WoodPile:
                        
                        objToDraw = new TownSimulator.Items.WoodPile();

                        if (leftClicked && !selectedTile.IsSolid) draw = true;

                        break;
                    case ObjectToAdd.Rock:

                        objToDraw = new GameObject() { ObjectSprite = new Sprite(6), IsSolid = true };

                        if (leftDown && !selectedTile.IsSolid) draw = true;

                        break;
                    case ObjectToAdd.House:


                        objToDraw = new Buildings.House();

                        if (leftClicked) draw = true;
                        
                        break;
                }

                if(objToDraw != null)
                {
                    Point size = objToDraw.GetTileSize();
                    List<Tile> tiles = TileMap.GetTileArea(selectedTile.Position, size.X, size.Y);
                    if(tiles != null)
                    {
                        foreach (Tile t in tiles)
                        {
                            tilesToDraw.Add(t, t.IsSolid ? Color.Red : Color.White);
                        }
                    }
                }

                if (draw)
                {
                    selectedTile.AddObject(objToDraw);
                }

            }

        }

        //TODO remove this, it is not thread safe
        public static void Draw()
        {
            //Write current item to screen
            DrawingUtils.DrawMessage(_state.ToString());

            if (tilesToDraw != null && tilesToDraw.Count > 0)
            {
                foreach (KeyValuePair<Tile, Color> t in tilesToDraw)
                {
                    DrawingUtils.DrawRectangle(new Rectangle(t.Key.Position.X * Engine.TileWidth, t.Key.Position.Y * Engine.TileHeight, Engine.TileWidth, Engine.TileHeight), t.Value);
                }
            }



        }
    }
}
