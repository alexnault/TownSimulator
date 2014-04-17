using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;
using TownSimulator.Buildings;
using TownSimulator.Scenery;

namespace TownSimulator.Villagers
{
    enum WoodcutterState
    {
        Idle,
        Thinking,
        Walking,
        Cutting
    };
    enum WoodcutterTask
    {
        None,
        GoingToTree,
        PickUpWood,
        GoingToLumberMill
    };

    class Woodcutter : Villager
    {
        public WoodcutterState CurrentState { get; private set; }
        public WoodcutterTask CurrentTask { get; private set; }

        public int WoodCount { get; private set; }

        public Woodcutter(Town hometown)
            :this(NameGenerator.GetFirstname(), NameGenerator.GetLastname(), hometown)
        {
            
        }


        public Woodcutter(string firstname, string lastname, Town hometown)
            : base(firstname, lastname, hometown)
        {
            CurrentState = WoodcutterState.Idle;
            CurrentTask = WoodcutterTask.None;

            IsBig = false;

            ObjectSprite = new TileEngine.Sprite(3, Position.X, Position.Y, 32, 32);
 
            Start();
        }

        protected override void MakeDecision(EnvironmentEvent latestEvent)
        {
            CurrentState = WoodcutterState.Thinking;

            switch (CurrentTask)
            {
                case (WoodcutterTask.None):
                    {
                        // TODO other behaviors
                        // if hungry, go eat
                        // if thirsty, go drink
                        // else, go cut wood
                        Tree tree = FindUnusedTree();
                        if (tree != null)
                        {
                            CurrentTask = WoodcutterTask.GoingToTree;
                            CurrentState = WoodcutterState.Walking;
                            GoTo(tree.Position);
                        }
                        break;
                    }
                case (WoodcutterTask.GoingToTree):
                    {
                        Tree tree = FindUnusedTree();
                        if (tree != null)
                        {
                            if (Position.IsNextTo(tree.Position))
                            {
                                if(tree.Consort(this))
                                {
                                    CurrentState = WoodcutterState.Cutting;
                                    CurrentTask = WoodcutterTask.PickUpWood;
                                    SetFacingDirection(tree.Position);
                                }
                                else
                                {
                                    Warn(EnvironmentEvent.Auto);
                                }
                            }
                            else // keep going to tree
                            {
                                CurrentState = WoodcutterState.Walking;
                                GoTo(tree.Position);
                            }
                        }
                        break;
                    }
                case (WoodcutterTask.PickUpWood):
                    {
                        if (latestEvent == EnvironmentEvent.TreeCutted)
                        {
                            Tree tree = FindMyTree();
                            if (tree != null)
                            {
                                tree.Deconsort();
                                TileMap.Tiles[tree.Position.X, tree.Position.Y].RemoveObject(tree);
                                WoodCount++;

                                CurrentTask = WoodcutterTask.GoingToLumberMill;
                                CurrentState = WoodcutterState.Walking;

                                Warn(EnvironmentEvent.WoodPickedUp);
                            }
                        }
                        break;
                    }
                case (WoodcutterTask.GoingToLumberMill):
                    {
                        LumberMill lumbermill = Pathfinding.FindClosest<LumberMill>(Position);
                        if (lumbermill != null)
                        {
                            if (Position.IsNextTo(lumbermill.Position))
                            {
                                WoodCount -= lumbermill.StoreWood(WoodCount);
                                CurrentTask = WoodcutterTask.None;
                                Warn(EnvironmentEvent.WoodStored);
                            }
                            else
                            {
                                CurrentTask = WoodcutterTask.GoingToLumberMill;
                                CurrentState = WoodcutterState.Walking;
                                GoTo(lumbermill.Position);
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        protected Tree FindUnusedTree()
        {
            return Pathfinding.FindClosest<Tree>(Position, x => x.Slayer == null);
        }

        protected Tree FindMyTree()
        {
            return Pathfinding.FindClosest<Tree>(Position, x => x.Slayer == this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawingUtils.DrawMessage(
                WoodCount.ToString(),
                new Vector2(Position.X * Engine.TileWidth, Position.Y * Engine.TileWidth + YDrawOffset),
                Color.GreenYellow,
                false);

            base.Draw(spriteBatch);
        }
    }
}
