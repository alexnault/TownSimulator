using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public class DijkstraNode
    {
        public int Distance;
        public DijkstraNode Previous;
        public Point Position;

        public List<DijkstraNode> Neighbors = new List<DijkstraNode>();        
        
        public void SetNeighbors(List<DijkstraNode> nodes)
        {

            Point left = new Point(Position.X - 1, Position.Y);
            if (left.X >= 0)
            {
                int pos = left.Y * TileMap.Height + left.X;
                DijkstraNode leftNeighbor = nodes[pos];
                if (leftNeighbor != null) Neighbors.Add(leftNeighbor);
            }

            Point right = new Point(Position.X + 1, Position.Y);
            if (right.X < TileMap.Width)
            {
                int pos = right.Y * TileMap.Height + right.X;
                DijkstraNode rightNeighbor = nodes[pos];
                if (rightNeighbor != null) Neighbors.Add(rightNeighbor);
            }

            Point top = new Point(Position.X, Position.Y - 1);
            if (top.Y >= 0)
            {
                int pos = top.Y * TileMap.Height + top.X;
                DijkstraNode topNeighbor = nodes[pos];
                if (topNeighbor != null) Neighbors.Add(topNeighbor);
            }


            Point bottom = new Point(Position.X, Position.Y + 1);
            if (bottom.Y < TileMap.Height)
            {
                int pos = bottom.Y * TileMap.Height + bottom.X;
                DijkstraNode bottomNeighbor = nodes[pos];
                if (bottomNeighbor != null) Neighbors.Add(bottomNeighbor);
            }
        }


        
    }
}
