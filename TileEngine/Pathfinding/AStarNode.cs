using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public class AStarNode
    {

        public int H;  //Nombre de points totaux calculer avec seulement un chemin de nodes verticales/horizontales
        //Méthode Manhattan : H = 10*(absolute(currentX-targetX) + absolute(currentY-targetY))
        public int G;  //Nombre de points lors du mouvement vers une case (10 = horizontal/vertical, 14 = diagonal)
        public int F;  //Points par case utilisé pour le choix de la meilleur case (Plus petit = meilleur). Calcul : F = G + H  

        public Point Position { get; private set; }
        public AStarNode ParentNode { get; set; }

        public AStarNode()
            : this(0, 0)
        {
        }

        public AStarNode(Vector2 pos)
            : this((int)pos.X, (int)pos.Y)
        {
        }

        public AStarNode(int x, int y)
        {
            this.G = 0;
            this.H = 0;
            this.F = -1;
            Position = new Point(x, y);
            ParentNode = null;
        }


    }
}
