using Microsoft.Xna.Framework;
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
        private WoodcutterState _currentState;
        private WoodcutterTask _currentTask;

        public WoodcutterState CurrentState { get {return _currentState;} }
        public WoodcutterTask CurrentTask { get { return _currentTask; } }

        public Woodcutter(string firstname, string lastname, Town hometown)
            : base(firstname, lastname, hometown)
        {
            _currentState = WoodcutterState.Idle;
            _currentTask = WoodcutterTask.None;
            //Position = new Point(0, 1);
            IsBig = false;

            ObjectSprite = new TileEngine.Sprite(3, Position.X, Position.Y, 32, 32);
            
            //XDrawOffset = 0;
            //YDrawOffset = 0;   
        }

        protected override void Run()
        {
            while (true) // While the Woodcutter lives
            {
                // Decision making process
                // Wait for main thread to warn me about making a decision
                EnvironmentEvent latestEvent = Wait();
                // Wait until its my turn
                HomeTown.AITurn.WaitOne();

                _currentState = WoodcutterState.Thinking;

                switch(_currentTask)
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
                            _currentTask = WoodcutterTask.GoingToTree;
                            _currentState = WoodcutterState.Walking;
                            GoTo(tree.Position);
                        }
                        break;
                    }
                    case (WoodcutterTask.GoingToTree):
                    {
                        Tree tree = FindUnusedTree();
                        if (tree != null)
                        {
                            if (IsNextTo(Position, tree.Position))
                            {
                                _currentState = WoodcutterState.Cutting;
                                _currentTask = WoodcutterTask.PickUpWood;
                                tree.Consort(this);
                                SetFacingDirection(tree.Position);
                            }
                            else // keep going to tree
                            {
                                _currentState = WoodcutterState.Walking;
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
                            TileMap.Tiles[tree.Position.X, tree.Position.Y].RemoveObject(tree);

                            _currentTask = WoodcutterTask.GoingToLumberMill;
                            _currentState = WoodcutterState.Walking;

                            Warn(EnvironmentEvent.WoodPickedUp);
                        }
                        break;
                    }
                    case (WoodcutterTask.GoingToLumberMill):
                    {
                        LumberMill lumbermill = TileMap.FindClosest<LumberMill>(Position);
                        if (lumbermill != null)
                        {
                            if (IsNextTo(Position, lumbermill.Position))
                            {
                                lumbermill.StoreWood();
                                _currentTask = WoodcutterTask.None;
                                Warn(EnvironmentEvent.WoodStored);
                            }
                            else
                            {
                                _currentTask = WoodcutterTask.GoingToLumberMill;
                                _currentState = WoodcutterState.Walking;
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
                HomeTown.AITurn.Release();
            }
        }

        // TODO put in engine
        protected bool IsNextTo(Point p1, Point p2)
        {
            if ((p2.X == p1.X) && (p2.Y == p1.Y))
                return true;
            if (p2.Y == p1.Y && (p2.X == p1.X + 1 || p2.X == p1.X - 1))
                return true;
            if (p2.X == p1.X && (p2.Y == p1.Y + 1 || p2.Y == p1.Y - 1))
                return true;
            return false;
        }

        protected Tree FindUnusedTree()
        {
            Tree tree;
            int i = 0;
            do {
                i++; // Find next tree
                tree = TileMap.Find<Tree>(Position, i);
                if (tree == null)
                    return null; // No unused tree
            } while(tree.Slayer != null);
            return tree;
        }

        protected Tree FindMyTree()
        {
            Tree tree;
            int i = 0;
            do
            {
                i++;
                tree = TileMap.Find<Tree>(Position, i);
                if (tree == null)
                    return null;
            } while (tree.Slayer != this);
            return tree;
        }
    }
}
