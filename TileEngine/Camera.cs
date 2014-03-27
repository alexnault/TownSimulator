using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace TileEngine
{
    public class Camera
    {
        public Vector2 Position = Vector2.Zero;
        private float speed = 10f;
        
        public float Speed
        {
            get { return speed; }
            set
            {
                speed = (float)Math.Max(value, 2f); //Vérifie la value entrée et si elle est plus grande que 1f,
                //elle est garder sinon la vitesse devient 1f
            }
        }

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Position, 0));
            }
        }

        public void ClampToArea(int width, int height)
        {
            //Pour ne pas sortir de la map (à droite ou en bas)
            if (Position.Y > height) //Hauteur de la map - largeur de l'écran
                Position.Y = height;
            if (Position.X > width)   //Largeur de la map - hauteur de l'écran
                Position.X = width;

            //Pour ne pas sortir de la map (en haut ou à gauche)
            if (Position.Y < 0)
                Position.Y = 0;
            if (Position.X < 0)
                Position.X = 0;

        }

        ////Pour suivre un sprite
        //public void LockToTarget(GameObject sprite, int screenWidth, int screenHeight)
        //{
        //    //Suit le character si il dépasse la moitié de l'écran et qu'il y a encore de la map dans cette direction
        //    //va centrer le character, jusqu'a ce qu'il soit rendu trop loin pour que la camera puisse le centrer (pour ne pas regarder hors de la map)
        //    Position.X =
        //        sprite.Position.X +
        //        (sprite.CurrentAnimation.CurrentRect.Width / 2) -
        //        (screenWidth / 2);

        //    Position.Y =
        //        sprite.Position.Y +
        //        (sprite.CurrentAnimation.CurrentRect.Height / 2) -
        //        (screenHeight / 2);
        //}

    }
}
