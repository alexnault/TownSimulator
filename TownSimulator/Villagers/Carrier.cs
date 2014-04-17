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
        GoingToLumberMill,
        DropWood
    };

    class Carrier : Villager
    {
        public const int MAX_LOAD_SIZE = 3;

        //private WoodcutterState CurrentState;
        public CarrierTask CurrentTask { get; private set; }

        public LumberMill Workplace { get; set; }

        public int LoadSize { get; private set; }

        public Carrier(Town hometown, LumberMill workplace)
            :this(NameGenerator.GetFirstname(), NameGenerator.GetLastname(), hometown, workplace)
        {


        }

        public Carrier(string firstname, string lastname, Town hometown, LumberMill workplace)
            : base(firstname, lastname, hometown)
        {
            CurrentTask = CarrierTask.None;
            IsBig = false;

            ObjectSprite = new TileEngine.Sprite(11, Position.X, Position.Y, 32, 32);

            Workplace = workplace;

            Start();
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
                case (CarrierTask.GoingToLumberMill):
                {
                    if (Position.IsNextTo(Workplace.Position))
                    {
                        CurrentTask = CarrierTask.PickUpWood;
                        Workplace.Consort(this);
                        SetFacingDirection(Workplace.Position);

                        // in case lumbermill already has the supply needed
                        Warn(EnvironmentEvent.Auto);
                    }
                    else // keep going to lumberMill
                    {
                       GoTo(Workplace.Position);
                    }
                    break;
                }
                case (CarrierTask.PickUpWood):
                {
                    //if (latestEvent == EnvironmentEvent.WoodStored)
                    //{
                        if (Workplace.CheckWood() >= MAX_LOAD_SIZE)
                        {
                            LoadSize = Workplace.DiscardWood(MAX_LOAD_SIZE);
                            CurrentTask = CarrierTask.GoingToConstructionSite;
                            Warn(EnvironmentEvent.WoodPickedUp);
                        }
                    //}
                    break;
                }
                case (CarrierTask.GoingToConstructionSite):
                {
                    Building cs = Pathfinding.FindClosest<Building>(Position, (x => x.InConstruction && !x.ResourcesSpent), true);
                    if (cs != null)
                    {
                        if (Position.IsNextTo(cs.Position))
                        {
                            LoadSize -= cs.AddWood(LoadSize);
                            CurrentTask = CarrierTask.None;
                            Warn(EnvironmentEvent.WoodStored);
                        }
                        else // keep going to house
                        {
                            GoTo(cs.Position);
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
    }
}
