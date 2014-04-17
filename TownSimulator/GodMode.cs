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
    static class GodMode
    {
        public enum ClickState
        {
            Tree,
            Woodcutter,
            Rock,
            House,
            //LumberMill
        }
        private static Dictionary<Tile, Color> _tilesToDraw;
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
            //else if (InputHelper.IsNewKeyPressed(Keys.L))
            //{
            //    _state = ClickState.LumberMill;
            //}
            else if (InputHelper.IsNewKeyPressed(Keys.S))
            {
                foreach (KeyValuePair<int, Villagers.Villager> v in town.Villagers)
                    v.Value.Warn(EnvironmentEvent.Manual);
            }

        }

        public static void ShowCommands()
        {
            Console.Write(
            "===============GOD MODE===============\n" +
            "Press one of the following keys and\n" +
            "click on the map to spawn a GameObject\n" +
            "T = Tree;\n" +
            "W = Woodcutter;\n" +
            "R = Rock;\n" +
            "H = House;\n" +
            //"L = LumberMill;\n" +
            "\n" +
            "Pressing S will warn all Villagers so\n" +
            "they can make a decision.\n" +
            "\n" +
            "Move camera = Arrow keys\n" + 
            "Right-click on villager to see its status.\n"
            );
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

                _tilesToDraw = new Dictionary<Tile, Color>();


                //Handle selection
                if (rightClicked)
                {
                    _selectedUnit = selectedTile.GetFirstObject<Villager>(true);
                }


                //Handle mouse click on map
                switch (_state)
                {
                    case ClickState.Tree:
                        {
                            _tilesToDraw.Add(selectedTile, Color.White);
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
                            _tilesToDraw.Add(selectedTile, Color.White);
                            if (leftClicked && !selectedTile.IsSolid)
                            {
                                selectedTile.AddObject(new TownSimulator.Villagers.Woodcutter(town));
                            }

                            break;
                        }
                    case ClickState.Rock:
                        {
                            _tilesToDraw.Add(selectedTile, Color.White);
                            if (leftDown && !selectedTile.IsSolid)
                            {
                                selectedTile.AddObject(new GameObject() { ObjectSprite = new Sprite(6), IsSolid = true });
                            }

                            break;
                        }
                    case ClickState.House:
                        {
                            //Problem
                            House house = new House(town);

                            Size size = house.GetTileSize();
                            if (DrawingUtils.DrawingRectangle)
                            {
                                List<Tile> tiles = TileMap.GetTileArea(selectedTile.Position, size);
                                if (tiles != null)
                                {
                                    foreach (Tile t in tiles)
                                    {
                                        _tilesToDraw.Add(t, t.IsSolid ? Color.Red : Color.White);
                                    }
                                }
                            }

                            if (leftClicked && !selectedTile.IsSolid)
                            {
                                selectedTile.AddObject(house);
                            }


                            break;
                        }
                    //case ClickState.LumberMill:
                    //    {
                    //        //Problem
                    //        LumberMill lm = new LumberMill(town);

                    //        Size size = lm.GetTileSize();
                    //        List<Tile> tiles = TileMap.GetTileArea(selectedTile.Position, size.Width, size.Height);
                    //        if (tiles != null)
                    //        {
                    //            foreach (Tile t in tiles)
                    //            {
                    //                _tilesToDraw.Add(t, t.IsSolid ? Color.Red : Color.White);
                    //            }

                    //            if (leftClicked && !selectedTile.IsSolid)
                    //            {
                    //                selectedTile.AddObject(lm);
                    //            }
                    //        }

                    //        break;
                    //    }
                }
            }
        }

        //TODO Not finished, can still crashed if begin was not called
        public static void Draw()
        {
            //Draw the GUI

            //Write current click state to screen
            DrawingUtils.DrawFullRectangle(new Rectangle(0, 0, 210, 30), new Color(Color.Black, 150), true);            
            DrawingUtils.DrawMessage("Current : " + _state.ToString(), Color.Orange);
            DrawSelectedUnitInfos();

            //Draw the mouse over tiles
            if (_tilesToDraw != null && _tilesToDraw.Count > 0)
            {
                foreach (KeyValuePair<Tile, Color> t in _tilesToDraw)
                {
                    DrawingUtils.DrawRectangle(new Rectangle(t.Key.Position.X * Engine.TileWidth, t.Key.Position.Y * Engine.TileHeight, Engine.TileWidth, Engine.TileHeight), t.Value, true);
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
                else if (_selectedUnit.GetType() == typeof(Carrier))
                {
                    elements.Add("Task : " + ((Carrier)_selectedUnit).CurrentTask.ToString());
                }
                else if (_selectedUnit.GetType() == typeof(Builder))
                {
                    elements.Add("Task : " + ((Builder)_selectedUnit).CurrentTask.ToString());
                }

                //Add the path of the Villager
                string nextPath = "Next : None";
                if (_selectedUnit.Path != null && _selectedUnit.Path.Count > 0)
                {
                    //foreach (Point p in _selectedUnit.Path)
                    //{
                    //    TileMap.GetTile(p).GroundTextureID = 1;
                    //}

                    int count = _selectedUnit.Path.Count;
                    Point nextPos = _selectedUnit.Path[0];
                    Point lastPos = _selectedUnit.Path[count - 1];

                    nextPath = string.Format("Next : [{0} , {1}] ; To : [{2} , {3}]", nextPos.X, nextPos.Y, lastPos.X, lastPos.Y);

                    //Draw the path of the selected unit
                    foreach(Point p in _selectedUnit.Path)
                    {
                        DrawingUtils.DrawDotOnTile(p, Color.Brown);
                        //DrawingUtils.DrawRectangle(
                        //    new Rectangle(p.X * Engine.TileWidth, p.Y * Engine.TileHeight, Engine.TileWidth, Engine.TileHeight),
                        //    Color.Brown, true);
                    }

                }
                elements.Add(nextPath);


               
                //Draw all the messages
                //TODO: change to the real line height in the SpriteFont
                int lineHeight = 20;
                int rectWidth = 350;
                int x = Engine.ScreenWidth - rectWidth;

                DrawingUtils.DrawFullRectangle(new Rectangle(x - 10, 0, rectWidth + 10, lineHeight * elements.Count + 10), new Color(Color.Black, 150), true);

                for (int i = 0; i < elements.Count; i++)
                {
                    DrawingUtils.DrawMessage(elements[i], new Vector2(x, lineHeight * i), Color.Orange);
                }
            }
        }
    }
}
