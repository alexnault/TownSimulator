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
        GoingToLumberMill
    };

    class Woodcutter : Villager
    {
        private WoodcutterState CurrentState;
        private WoodcutterTask CurrentTask;

        public Woodcutter(string firstname, string lastname, Town hometown)
            : base(firstname, lastname, hometown)
        {
            CurrentState = WoodcutterState.Idle;
            CurrentTask = WoodcutterTask.None;
            //Position = new Point(0, 1);

            ObjectSprite = new TileEngine.Sprite(3, Position.X, Position.Y, 32, 32);

            XDrawOffset = 0;
            YDrawOffset = 0;   
        }

        protected override void Run()
        {
            while (true) // Kill Villager
            {
                //Decision making process
                // Wait for main thread to warn me about making a decision
                MakeDecision.WaitOne();
                
                // TODO No one is currently thinking
                HomeTown.AITurn.WaitOne();

                CurrentState = WoodcutterState.Thinking;

                switch(CurrentTask)
                {
                    case(WoodcutterTask.None):
                        // TODO other behaviors
                        // if hungry, go eat
                        // if thirsty, go drink
                        // else, go cut wood
                        Tree tree1 = FindUnusedTree();
                        if (tree1 != null)
                            MoveToTree(tree1);
                        break;
                    case(WoodcutterTask.GoingToTree):
                        Tree tree2 = FindUnusedTree();
                        if (tree2 != null)
                        {
                            if (IsNextTo(Position, tree2.Position))
                            {
                                CutTree(tree2);
                                CurrentTask = WoodcutterTask.GoingToLumberMill;
                                MakeDecision.Release(); // May want to remove this later on
                            }
                            else // keep going to tree
                                MoveToTree(tree2);
                        }
                        break;
                    case(WoodcutterTask.GoingToLumberMill):
                        LumberMill lumbermill = TileMap.FindClosest<LumberMill>(Position);
                        if (lumbermill != null)
                        {
                            if (IsNextTo(Position, lumbermill.Position))
                            {
                                // TODO Deposit of 1 unit of wood
                                CurrentTask = WoodcutterTask.None;
                                MakeDecision.Release(); // May want to remove this later on
                            }
                            else
                                MoveToLumberMill(lumbermill);
                        }
                        break;
                    default:
                        break;
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

        protected void MoveToTree(Tree tree)
        {
            Path = Pathfinding.DoAStar(tree.Position, Position);
            CurrentTask = WoodcutterTask.GoingToTree;
            CurrentState = WoodcutterState.Walking;
        }

        protected void CutTree(Tree tree)
        {
            tree.Consort(this); // Locks the tree
            CurrentState = WoodcutterState.Cutting;
            HomeTown.AITurn.Release(); // Allow others to make decisions while he cuts
                Thread.Sleep(3000); // Cutting tree
            HomeTown.AITurn.WaitOne();
            TileMap.Tiles[tree.Position.X, tree.Position.Y].RemoveObject(tree);
            CurrentState = WoodcutterState.Idle;
        }

        protected void MoveToLumberMill(LumberMill lumbermill) // TODO add LumberMill class
        {
            Path = Pathfinding.DoAStar(lumbermill.Position, Position);
            CurrentTask = WoodcutterTask.GoingToLumberMill;
            CurrentState = WoodcutterState.Walking;
        }
    }
}
