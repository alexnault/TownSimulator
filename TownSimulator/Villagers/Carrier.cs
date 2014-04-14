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
    /*enum CarrierState
    {
        Idle,
        Thinking,
        Walking,
        Cutting
    };*/
    enum CarrierTask
    {
        None,
        GoingToConstructionSite,
        PickUpWood,
        GoingToLumberMill
    };

    class Carrier : Villager
    {
        //private WoodcutterState CurrentState;
        private CarrierTask CurrentTask;

        public LumberMill Workplace { get; set; }

        public Carrier(string firstname, string lastname, Town hometown, LumberMill workplace)
            : base(firstname, lastname, hometown)
        {
            CurrentTask = CarrierTask.None;
            //Position = new Point(0, 1);
            IsBig = false;

            ObjectSprite = new TileEngine.Sprite(11, Position.X, Position.Y, 32, 32);

            Workplace = workplace;

            //XDrawOffset = 0;
            //YDrawOffset = 0;   
        }

        protected override void MakeDecision(EnvironmentEvent latestEvent)
        {
            switch (CurrentTask)
            {
                case (CarrierTask.None):
                {
                    // TODO other behaviors
                    // if hungry, go eat
                    // if thirsty, go drink
                    CurrentTask = CarrierTask.GoingToLumberMill;
                    GoTo(Workplace.Position);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        // TODO put in engine
        /*protected bool IsNextTo(Point p1, Point p2)
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
        }*/
    }
}
