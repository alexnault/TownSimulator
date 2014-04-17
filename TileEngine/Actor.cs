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
        private TimeSpan _lastGameTime;

        //Animated frames 
        //private TimeSpan _timePerFrame;
        //private TimeSpan _frameUpdateTime;
        private int _nbFrames;
        private int _currentFrame;


        public TimeSpan MovingTime { get; set; }    //Time to move between 2 tiles
        
        public List<Point> Path { get; protected set; }
        public Direction CurrentDirection { get; private set; }
        
        public Actor(int movingTimeMS = 150)
            :base()
        {
            _lastGameTime = new TimeSpan();
            //_frameUpdateTime = new TimeSpan();
            _currentFrame = 0;
            _nbFrames = 4;
            MovingTime = TimeSpan.FromMilliseconds(movingTimeMS);
            //_timePerFrame = TimeSpan.FromMilliseconds(movingTimeMS);
        }

        public override void Update(GameTime gameTime)
        {
            if (_lastGameTime > MovingTime)
            {
                //Update animation
                if(Path != null && Path.Count > 0)
                    _currentFrame = (_currentFrame + 1) % _nbFrames;

                MoveToNext();
                _lastGameTime = TimeSpan.FromMilliseconds(0);
            }

            _lastGameTime += gameTime.ElapsedGameTime;

            if (ObjectSprite != null)
            {
                ObjectSprite.TexturePortion =
                    new Rectangle(
                        _currentFrame * ObjectSprite.Width,
                        (int)CurrentDirection * ObjectSprite.Height,
                        ObjectSprite.Width,
                        ObjectSprite.Height);
            }



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

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    if (ObjectSprite != null)
        //    {
        //        ObjectSprite.TexturePortion = 
        //            new Rectangle(
        //                _currentFrame * ObjectSprite.Width,
        //                (int)CurrentDirection * ObjectSprite.Height,
        //                ObjectSprite.Width,
        //                ObjectSprite.Height);
        //    }

        //    base.Draw(spriteBatch);
        //}

        protected virtual void PathBlocked() { }
        protected virtual void DestinationReached() { }
    }
}
