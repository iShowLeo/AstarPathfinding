using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PF
{
    public class Node
    {
        public int X;
        public int Y;
        public bool walkable;
        public Node parent = null;
        public float gCost = 0;
        public float hCost = 0;
        public float FCost
        {
            get
            {
                return gCost + hCost;
            }
        }


        public Node(int _x, int _y, bool _walkable)
        {
            this.X = _x;
            this.Y = _y;
            this.walkable = _walkable;
        }

        public float ManhattanDistance(Node target)
        {
            return Mathf.Abs(X - target.X) + Mathf.Abs(Y - target.Y);
        }
         

        public float NeighborDistance(Node target)
        {
            return (X == target.X || Y == target.Y) ? 1 : Mathf.Sqrt(2);  
        }

    }

}
