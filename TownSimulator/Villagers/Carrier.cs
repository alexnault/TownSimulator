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
                    int woodAvailable = Workplace.CheckWood();
                    int woodNeeded = MAX_LOAD_SIZE - LoadSize;
                    if (woodAvailable >= woodNeeded)
                    {
                        LoadSize += Workplace.DiscardWood(woodNeeded);

                        Building cs = Pathfinding.FindClosest<Building>(Position, (x => x.InConstruction && x.NeedWood()), true);
                        if (cs != null)
                        {
                            CurrentTask = CarrierTask.GoingToConstructionSite;
                            GoTo(cs.Position);
                        }

                        Warn(EnvironmentEvent.WoodPickedUp);
                    }
                    break;
                }
                case (CarrierTask.GoingToConstructionSite):
                {
                    Building cs = Pathfinding.FindClosest<Building>(Position, (x => x.InConstruction && x.NeedWood()), true);
                    if (cs != null)
                    {
                        if (Position.IsNextTo(cs.Position))
                        {
                            LoadSize -= cs.AddWood(LoadSize);

                            // If load remaining, give it to another construction site
                            if (LoadSize > 0)
                                CurrentTask = CarrierTask.GoingToConstructionSite;                                
                            else
                                CurrentTask = CarrierTask.None;

                            Warn(EnvironmentEvent.WoodStored);
                        }
                        else // keep going to construction site
                        {
                            GoTo(cs.Position);
                        }
                    }
                    else
                    {
                        CurrentTask = CarrierTask.GoingToLumberMill;
                        GoTo(Workplace.Position);
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawingUtils.DrawMessage(
                LoadSize.ToString(),
                new Vector2(Position.X * Engine.TileWidth, Position.Y * Engine.TileWidth + YDrawOffset),
                Color.GreenYellow,
                false);

            base.Draw(spriteBatch);
        }
    }
}
