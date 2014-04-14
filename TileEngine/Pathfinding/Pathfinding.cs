using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public static class Pathfinding
    {


        public static List<Point> DoAStar(Point from, Point to)
        {
            int tileAcross = TileMap.Width;
            int tileDown = TileMap.Height;
            int[] neighbourX = new int[] { -1, 0, 1, 0 };
            int[] neighbourY = new int[] { 0, -1, 0, 1 };

            AStarNode[,] squares = new AStarNode[tileAcross, tileDown];
            for (int y = 0; y < tileDown; y++)
            {
                for (int x = 0; x < tileAcross; x++)
                {
                    squares[x, y] = new AStarNode(x, y);
                }
            }

            //Node origin = squares[from.X, from.Y];
            //Node destination = squares[to.X, to.Y];

            AStarNode origin = squares[from.X, from.Y];
            AStarNode destination = squares[to.X, to.Y];

            List<AStarNode> openList = new List<AStarNode>();
            List<AStarNode> closedList = new List<AStarNode>();

            openList.Add(origin);

            AStarNode current = null;

            while (openList.Count > 0)
            {
                current = openList[0];

                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].G + CalculateHeuristic(openList[i], destination) < current.G + CalculateHeuristic(current, destination))
                    {
                        current = openList[i];
                    }
                }

                if (current == destination) // If destination has been reached.
                {
                    int pathX = destination.Position.X,
                        pathY = destination.Position.Y;

                    List<AStarNode> path = new List<AStarNode>();
                    do
                    {
                        path.Add(current);
                        pathX = current.Position.X;
                        pathY = current.Position.Y;
                        current = current.ParentNode;

                    }
                    while (pathX != origin.Position.X || pathY != origin.Position.Y);


                    List<Point> pathCoord = new List<Point>();
                    foreach (AStarNode n in path)
                    {
                        pathCoord.Add(n.Position);
                    }

                    return pathCoord;
                }

                // Add current to closed list              
                closedList.Add(current);

                // Remove current from openlist.
                openList.Remove(current);

                //Grab surrounding squares 
                for (int neighOffset = 0; neighOffset < 4; neighOffset++)
                {
                    // Grab the coordinates of a neighbour tile. 
                    int NX = current.Position.X + neighbourX[neighOffset];
                    int NY = current.Position.Y + neighbourY[neighOffset];
                    if (NX < 0 || NX >= tileAcross || NY < 0 || NY >= tileDown) continue;

                    AStarNode neighbour = squares[NX, NY];

                    if (!IsWalkable(neighbour.Position)) continue;

                    if (!closedList.Contains(neighbour))
                    {
                        if (!openList.Contains(neighbour))
                        {
                            neighbour.H = CalculateHeuristic(neighbour, destination); // Calculate heuristic value. 
                            neighbour.ParentNode = current;

                            neighbour.G = 10 + current.G; // Add 10 (movement cost) to the current tiles cost. 
                            openList.Add(neighbour);
                        }
                        else
                        {
                            if (current.G + CalculateHeuristic(current, destination) < neighbour.G + CalculateHeuristic(neighbour, destination))
                            {
                                neighbour.ParentNode = current;
                                neighbour.G = 10 + current.G; // Add 10 (movement cost) to the current tiles cost.
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static T FindClosest<T>(Point from) where T : GameObject
        {
            int nbElements = TileMap.Width * TileMap.Height;
            int nbSolidItems = 0;

            List<DijkstraNode> nodes = new List<DijkstraNode>();
            for(int i = 0; i < nbElements; i++)
            {
                int x = i % TileMap.Width;
                int y = (int)Math.Floor((double)(i / TileMap.Height));

                nodes.Add( new DijkstraNode()
                    {
                        Distance = int.MaxValue,
                        Position = new Point(x, y)
                    });

                if (!IsWalkable(new Point(x, y))) nbSolidItems++;
            }
            
            DijkstraNode start = nodes.FirstOrDefault(x => x.Position == from);
            start.Distance = 0;

            List<DijkstraNode> Q = nodes.ToList();

            while (Q.Count - nbSolidItems > 0)
            {
                //TODO : Improve it?
                DijkstraNode u = 
                    Q
                    .Where(x => IsWalkable(x.Position))   //Get all non-solid items
                    .Aggregate((curmin, x) => (curmin == null || x.Distance < curmin.Distance ? x : curmin));   //Get the one with the smallest value
                                        
                Q.Remove(u);
                
                if (nodes.Find(o => o == u).Distance == int.MaxValue) break;

                //Get Neighbors
                //TODO : Improve it?
                List<DijkstraNode> neighbors = new List<DijkstraNode>();
                DijkstraNode leftNeighbor = nodes.FirstOrDefault(o => o.Position == new Point(u.Position.X - 1, u.Position.Y));
                if(leftNeighbor != null) neighbors.Add(leftNeighbor);

                DijkstraNode rightNeighbor = nodes.FirstOrDefault(o => o.Position == new Point(u.Position.X + 1, u.Position.Y));
                if(rightNeighbor != null) neighbors.Add(rightNeighbor);

                DijkstraNode topNeighbor = nodes.FirstOrDefault(o => o.Position == new Point(u.Position.X, u.Position.Y - 1));
                if(topNeighbor != null) neighbors.Add(topNeighbor);

                DijkstraNode bottomNeighbor = nodes.FirstOrDefault(o => o.Position == new Point(u.Position.X, u.Position.Y + 1));
                if(bottomNeighbor != null) neighbors.Add(bottomNeighbor);


                foreach (DijkstraNode v in neighbors)
                {
                    if (TileMap.Tiles[v.Position.X, v.Position.Y].ContainsObject<T>())
                    {
                        return TileMap.Tiles[v.Position.X, v.Position.Y].GetFirstObject<T>();
                    }

                    int alt = u.Distance + 1;
                    if(alt < v.Distance)
                    {
                        v.Distance = alt;
                        v.Previous = u;
                    }
                }

            }

            return null;

        }

        private static int CalculateHeuristic(AStarNode current, AStarNode target)
        {
            return (int)(Math.Abs(current.Position.X - target.Position.X) + Math.Abs(current.Position.Y - target.Position.Y));
        }

        private static bool IsWalkable(Point node)
        {
            return !TileMap.Tiles[node.X, node.Y].IsSolid;
        }

    }
}
