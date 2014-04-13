using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public enum Direction
    {
        Down = 0,
        Left = 1,
        Right = 2,
        Up = 3
    }

    public class Actor : GameObject
    {
        private TimeSpan lastGameTime;

        public TimeSpan MovingTime { get; set; }    //Time to move between 2 tiles
        public List<Point> Path { get; protected set; }
        public Direction CurrentDirection { get; private set; }

        public Actor(int movingTimeMS = 100)
            :base()
        {
            lastGameTime = new TimeSpan();
            MovingTime = TimeSpan.FromMilliseconds(movingTimeMS);
        }

        public override void Update(GameTime gameTime)
        {
            if (lastGameTime > MovingTime)
            {
                MoveToNext();
                lastGameTime = TimeSpan.FromMilliseconds(0);
            }

            lastGameTime += gameTime.ElapsedGameTime;

            base.Update(gameTime);
        }

        private void MoveToNext()
        {
            if (Path != null && Path.Count > 0)
            {
                Point nextPoint = Path[0];

                if (!TileMap.Tiles[nextPoint.X, nextPoint.Y].IsSolid)
                {
                    Point oldPos = new Point(Position.X, Position.Y);
                    SetFacingDirection(nextPoint);

                    TileMap.Tiles[nextPoint.X, nextPoint.Y].AddObject(this);
                    TileMap.Tiles[oldPos.X, oldPos.Y].RemoveObject(this);
                    
                    Path.Remove(nextPoint);

                    if (Path.Count == 0)
                        DestinationReached(); // Event
                }
                else // Path has been blocked
                {
                    Path.Clear();
                    PathBlocked(); // Event
                }
            }
        }

        
        protected void SetFacingDirection(Point newDirection)
        {            
            if (Position.Y == newDirection.Y)
            {
                //Going Horizontally
                CurrentDirection = (Position.X > newDirection.X) ? Direction.Left : Direction.Right;
            }
            else
            {
                //Going Vertically
                CurrentDirection = (Position.Y > newDirection.Y) ? Direction.Up : Direction.Down;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (ObjectSprite != null)
            {
                ObjectSprite.TexturePortion = new Rectangle(0, (int)CurrentDirection * ObjectSprite.Height, ObjectSprite.Width, ObjectSprite.Height);
            }

            base.Draw(spriteBatch);
        }

        protected virtual void PathBlocked() { }
        protected virtual void DestinationReached() { }
    }
}
