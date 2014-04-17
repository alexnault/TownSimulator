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
    enum BuilderTask
    {
        None,
        GoingToConstructionSite,
        Build
    };

    class Builder : Villager
    {
        public BuilderTask CurrentTask { get; private set; }

        public Builder(Town hometown)
            :this(NameGenerator.GetFirstname(), NameGenerator.GetLastname(), hometown)
        {


        }

        public Builder(string firstname, string lastname, Town hometown)
            : base(firstname, lastname, hometown)
        {
            CurrentTask = BuilderTask.None;
            IsBig = false;

            ObjectSprite = new TileEngine.Sprite(14, Position.X, Position.Y, 32, 32);

            //Workplace = workplace;

            Start();
        }

        protected override void MakeDecision(EnvironmentEvent latestEvent)
        {
            switch (CurrentTask)
            {
                case(BuilderTask.None):
                {
                    Building cs = FindUnattendedConstructionSite();
                    if (cs != null)
                    {
                        CurrentTask = BuilderTask.GoingToConstructionSite;
                        GoTo(cs.Position);
                    }
                    break;
                }
                case(BuilderTask.GoingToConstructionSite):
                {
                    Building cs = FindUnattendedConstructionSite();
                    if (cs != null)
                    {
                        if (Position.IsNextTo(cs.Position))
                        {
                            if(cs.Consort(this))
                            {
                                CurrentTask = BuilderTask.Build;
                                SetFacingDirection(cs.Position);
                            }
                            else
                            {
                                Warn(EnvironmentEvent.Auto);
                            } 
                        }
                        else
                            GoTo(cs.Position);
                    }
                    break;
                }
                case (BuilderTask.Build):
                {
                    if (latestEvent == EnvironmentEvent.BuildingBuilt)
                    {
                        Building cs = FindMyConstructionSite();
                        TileMap.Tiles[cs.Position.X, cs.Position.Y].AddObject(new House(HomeTown));

                        cs.Deconsort();

                        TileMap.Tiles[cs.Position.X, cs.Position.Y].RemoveObject(cs);

                        CurrentTask = BuilderTask.None;

                        Warn(EnvironmentEvent.Auto);
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        protected Building FindUnattendedConstructionSite()
        {
            return Pathfinding.FindClosest<Building>(Position, (x => x.InConstruction && x.Foreman == null), true);
        }

        protected Building FindMyConstructionSite()
        {
            return Pathfinding.FindClosest<Building>(Position, x => x.Foreman == this, true);
        }
    }
}
