using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileEngine
{
    public static class Pathfinding
    {


        public static List<Vector2> DoAStar(Vector2 from, Vector2 to)
        {
            int tileAcross = TileMap.Width;
            int tileDown = TileMap.Height;
            int[] neighbourX = new int[] { -1, 0, 1, 0 };
            int[] neighbourY = new int[] { 0, -1, 0, 1 };

            Node[,] squares = new Node[tileAcross, tileDown];
            for (int y = 0; y < tileDown; y++)
            {
                for (int x = 0; x < tileAcross; x++)
                {
                    squares[x, y] = new Node(x, y);
                }
            }

            Node origin = squares[(int)from.X / Engine.TileWidth, (int)from.Y / Engine.TileHeight];
            Node destination = squares[(int)to.X / Engine.TileWidth, (int)to.Y / Engine.TileHeight];

            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();

            openList.Add(origin);

            Node current = null;

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
                    int pathX = (int)destination.Position.X, pathY = (int)destination.Position.Y;
                    List<Node> path = new List<Node>();
                    do
                    {
                        path.Add(current);
                        pathX = (int)current.Position.X;
                        pathY = (int)current.Position.Y;
                        current = current.ParentNode;

                    }
                    while (pathX != origin.Position.X || pathY != origin.Position.Y);


                    List<Vector2> pathCoord = new List<Vector2>();
                    foreach (Node n in path)
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
                    int NX = (int)current.Position.X + (int)neighbourX[neighOffset];
                    int NY = (int)current.Position.Y + (int)neighbourY[neighOffset];
                    if (NX < 0 || NX >= tileAcross || NY < 0 || NY >= tileDown) continue;

                    Node neighbour = squares[NX, NY];

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



        private static int CalculateHeuristic(Node current, Node target)
        {
            return (int)(Math.Abs(current.Position.X - target.Position.X) + Math.Abs(current.Position.Y - target.Position.Y)); ;
        }

        private static bool IsWalkable(Vector2 node)
        {
            return !TileMap.Tiles[(int)node.X, (int)node.Y].IsSolid;
        }

    }
}
