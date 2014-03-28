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
        WoodcutterState CurrentState;
        WoodcutterTask CurrentTask;

        public Woodcutter(string firstname, string lastname, Town hometown)
            : base(firstname, lastname, hometown)
        {
            CurrentState = WoodcutterState.Idle;
            CurrentTask = WoodcutterTask.None;
            Position = new Point(10, 10);
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
                        FindClosest(Position, typeof(TownSimulator.Items.WoodPile), 10);
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
                //Semaphore s = new Semaphore(0, 1);
                //Thread.Sleep(1000);
                //s.WaitOne();
            }
        }

        // Put in Engine
        // Adapted from
        // http://stackoverflow.com/questions/3330181/algorithm-for-finding-nearest-object-on-2d-grid
        void FindClosest(Point origin, Type gameObjectSearching, int maxDistance = 30)
        {
            if (!(gameObjectSearching is GameObject))
            {
                throw new ArgumentException("FindClosest method needs to search for a GameObject type.");
            }

            //int xs, ys; // Start coordinates

            // Check point (xs, ys)

            for (int d = 1; d < maxDistance; d++)
            {
                for (int i = 0; i < d + 1; i++)
                {
                    Point point1 = new Point(origin.X - d + i, origin.Y - i);
                    //int x1 = origin.X - d + i;
                    //int y1 = origin.Y - i;
                    //TileMap.
                    // Check point (x1, y1)

                    Point point2 = new Point(origin.X + d - i, origin.Y + i);
                    //int x2 = origin.X + d - i;
                    //int y2 = origin.Y + i;

                    // Check point (x2, y2)
                }

                for (int i = 1; i < d; i++)
                {

                    Point point1 = new Point(origin.X - i, origin.Y + d - i);
                    //int x1 = origin.X - i;
                    //int y1 = origin.Y + d - i;

                    // Check point (x1, y1)
                    Point point2 = new Point(origin.X + d - i, origin.Y - i);
                    //int x2 = origin.X + d - i;
                    //int y2 = origin.Y - i;

                    // Check point (x2, y2)
                }
            }
        }
    }
}
