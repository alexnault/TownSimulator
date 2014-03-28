using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;

namespace TownSimulator.Villagers
{
    enum WoodcutterState
    {
        Idle,
        Thinking,
        Walking
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

            ObjectSprite = new TileEngine.Sprite(3, Position.X, Position.Y, 32, 32, 0.5f);

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

                CurrentState = WoodcutterState.Thinking;

                switch(CurrentTask)
                {
                    case(WoodcutterTask.None):
                        // TODO other behavior
                        // if hungry, go eat
                        // if thirsty, go drink
                        // else, go cut wood
                        Point p = TileMap.FindClosest(Position, typeof(Scenery.Tree), 25);
                        if (p.X != -1)
                        {
                            Path = Pathfinding.DoAStar(p, Position);
                        }
                        else
                        {
                            Console.WriteLine("Cannot find a tree");
                        }
                        CurrentTask = WoodcutterTask.GoingToTree;
                        CurrentState = WoodcutterState.Walking;
                        break;
                    case(WoodcutterTask.GoingToTree):
                        // if tree next to me, cut it
                        Thread.Sleep(3000); // free ressources maybe
                        // else, recalculate path to tree
                        break;
                    case(WoodcutterTask.GoingToLumberMill):
                        // if lumber mill next to me, drop wood
                        // else recalculate path to lumber mill
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
