using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TileEngine;
using TownSimulator.Buildings;
using TownSimulator.Villagers;

namespace TownSimulator
{
    public static class GodMode
    {
        public enum ClickState
        {
            Tree,
            Woodcutter,
            WoodPile,
            Rock,
            House,
            LumberMill            
        }
        private static Dictionary<Tile, Color> tilesToDraw;
        private static ClickState _state;
        private static Villager _selectedUnit;

        public static void Update(GameTime gameTime, Camera camera, Town town)
        {
            HandleKeyboard(town);
            HandleMouse(camera, town);
        }
        
        private static void HandleKeyboard(Town town)
        {
            if (InputHelper.IsNewKeyPressed(Keys.T))
            {
                _state = ClickState.Tree;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.W))
            {
                _state = ClickState.WoodPile;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.V))
            {
                _state = ClickState.Woodcutter;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.R))
            {
                _state = ClickState.Rock;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.H))
            {
                _state = ClickState.House;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.L))
            {
                _state = ClickState.LumberMill;
            }
            else if (InputHelper.IsNewKeyPressed(Keys.S))
            {
                foreach (KeyValuePair<int, Villagers.Villager> v in town.Villagers)
                    v.Value.Warn(EnvironmentEvent.Manual);
            }           

        }

        private static void HandleMouse(Camera camera, Town town)
        {
            Rectangle mapRect = new Rectangle(0, 0, TileMap.Width * Engine.TileWidth, TileMap.Height * Engine.TileHeight);

            Point tilePos = (InputHelper.MousePosition + camera.Position).ConvertPositionToCell();
            Tile selectedTile;

            if (tilePos.X >= 0 && tilePos.X < TileMap.Width && tilePos.Y >= 0 && tilePos.Y < TileMap.Height)
            {
                selectedTile = TileMap.Tiles[tilePos.X, tilePos.Y];

                bool leftClicked = InputHelper.LeftMouseButtonClicked() && InputHelper.IsMouseOn(mapRect);
                bool rightClicked = InputHelper.RightMouseButtonClicked() && InputHelper.IsMouseOn(mapRect);
                bool leftDown = InputHelper.LeftMouseButtonDown() && InputHelper.IsMouseOn(mapRect);
                
                tilesToDraw = new Dictionary<Tile, Color>();


                //Handle selection
                if (rightClicked)
                {
                    Villager v = selectedTile.GetFirstObject<Villager>(true);

                    if (v != null)
                    {
                        _selectedUnit = v;
                    }
                }   

                
                //Handle mouse click on map
                switch (_state)
                {
                    case ClickState.Tree:
                        {
                            tilesToDraw.Add(selectedTile, Color.White);
                            if (leftDown && !selectedTile.IsSolid)
                            {
                                selectedTile.AddObject(new TownSimulator.Scenery.Tree());
                                foreach (KeyValuePair<int, Villagers.Villager> v in town.Villagers)
                                    v.Value.Warn(EnvironmentEvent.TreeGrowed);
                            }

                            break;
                        }
                    case ClickState.Woodcutter:
                        {
                            tilesToDraw.Add(selectedTile, Color.White);
                            if (leftClicked && !selectedTile.IsSolid)
                            {
                                selectedTile.AddObject(new TownSimulator.Villagers.Woodcutter("The", "Woodcutter", town));
                            }

                            break;
                        }
                    case ClickState.WoodPile:
                        { 
                            tilesToDraw.Add(selectedTile, Color.White);
                            if (leftClicked && !selectedTile.IsSolid)
                            {
                                selectedTile.AddObject(new TownSimulator.Items.WoodPile());
                            }

                            break;
                        }
                    case ClickState.Rock:
                        {
                            tilesToDraw.Add(selectedTile, Color.White);
                            if (leftDown && !selectedTile.IsSolid)
                            {
                                selectedTile.AddObject(new GameObject() { ObjectSprite = new Sprite(6), IsSolid = true });
                            }

                            break;
                        }
                    case ClickState.House:
                        {
                            House house = new House(town);

                            Point size = house.GetTileSize();
                            List<Tile> tiles = TileMap.GetTileArea(selectedTile.Position, size.X, size.Y);
                            if (tiles != null)
                            {
                                foreach (Tile t in tiles)
                                {
                                    tilesToDraw.Add(t, t.IsSolid ? Color.Red : Color.White);
                                }

                                if (leftClicked && !selectedTile.IsSolid)
                                {
                                    selectedTile.AddObject(house);
                                }
                            }


                            break;
                        }
                    case ClickState.LumberMill:
                        {
                            LumberMill lm = new LumberMill(town);

                            Point size = lm.GetTileSize();
                            List<Tile>tiles = TileMap.GetTileArea(selectedTile.Position, size.X, size.Y);
                            if (tiles != null)
                            {
                                foreach (Tile t in tiles)
                                {
                                    tilesToDraw.Add(t, t.IsSolid ? Color.Red : Color.White);
                                }

                                if (leftClicked && !selectedTile.IsSolid)
                                {
                                    selectedTile.AddObject(lm);
                                }
                            }

                            break;
                        }
                }


               
            }
        }

        //TODO remove this, it is not thread safe
        public static void Draw()
        {
            //Write current item to screen
            DrawingUtils.DrawMessage(_state.ToString());

            DrawSelectedUnitInfos();           

            if (tilesToDraw != null && tilesToDraw.Count > 0)
            {
                foreach (KeyValuePair<Tile, Color> t in tilesToDraw)
                {
                    DrawingUtils.DrawRectangle(new Rectangle(t.Key.Position.X * Engine.TileWidth, t.Key.Position.Y * Engine.TileHeight, Engine.TileWidth, Engine.TileHeight), t.Value);
                }
            }
        }

        private static void DrawSelectedUnitInfos()
        {
            if (_selectedUnit != null)
            {
                List<string> elements = new List<string>();                

                elements.Add("Name : " + _selectedUnit.FirstName + " " + _selectedUnit.LastName);
               
                //Custom properties for each type of villager
                if (_selectedUnit.GetType() == typeof(Woodcutter))
                {
                    elements.Add("State : " + ((Woodcutter)_selectedUnit).CurrentState.ToString());
                    elements.Add("Task : " + ((Woodcutter)_selectedUnit).CurrentTask.ToString());
                }
                else
                {


                }

                //Add the path of the Villager
                string nextPath = "Next : None";
                if (_selectedUnit.Path != null && _selectedUnit.Path.Count > 0)
                {
                    int count = _selectedUnit.Path.Count;
                    Point nextPos = _selectedUnit.Path[0];
                    Point lastPos = _selectedUnit.Path[count - 1];

                    nextPath = string.Format("Next : [{0} , {1}] ; To : [{2} , {3}]", nextPos.X, nextPos.Y, lastPos.X, lastPos.Y);
                }
                elements.Add(nextPath);




                int height = 20;
                int x = 350;
                for(int i = 0; i < elements.Count ; i++)
                {
                    DrawingUtils.DrawMessage(elements[i], new Vector2(x, height * i), Color.Red);
                }
            }
        }
    }
}
