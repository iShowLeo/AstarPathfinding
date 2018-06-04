using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PF
{

    public class AStarFinder : MonoBehaviour
    {
        public Transform startPoint;
        public Transform endPoint;


        Grid grid;
        List<Node> openList;
        List<Node> closedList;


        private void Awake()
        {
            GameObject gridGo = GameObject.Find("Grid");
            if (gridGo == null)
            {
                throw new System.Exception("Didn't find the Grid");
            }
            grid = gridGo.GetComponent<Grid>();
            if (grid == null)
            {
                grid = gridGo.AddComponent<Grid>();
            }
            openList = new List<Node>();
            closedList = new List<Node>();

        }

        private void Start()
        {

        }


        private void Update()
        {
           
           PathFinding(startPoint, endPoint); 
           
        }


        void PathFinding(Transform start, Transform end)
        {
            openList.Clear();
            closedList.Clear(); 
            Node startNode = grid.PosToIndex(start);
            Node endNode = grid.PosToIndex(end);
            endNode.parent = null;

            startNode.gCost = 0;
            startNode.hCost = startNode.ManhattanDistance(endNode);
            openList.Add(startNode);
            while (openList.Count > 0)
            {
                Node currentNode = GetMinFCost();
                if (currentNode == endNode)
                {
                    GeneratePath(startNode,endNode);
                    return;
                }
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                List<Node> neighbor = grid.GetNeighbor(currentNode);
                for (int i = 0; i < neighbor.Count; i++)
                {
                    Node TargetNode = neighbor[i];
                    if (TargetNode.walkable == false || closedList.Contains(TargetNode)) continue;

                    float gcost = currentNode.gCost + currentNode.NeighborDistance(TargetNode);
                    bool isBetter = false;
                    if (!openList.Contains(TargetNode))
                    {
                        openList.Add(TargetNode);
                        isBetter = true;
                    }
                    else if (gcost < TargetNode.gCost)
                    {
                        isBetter = true;
                    }
                    if (isBetter)
                    {
                        TargetNode.gCost = gcost;
                        TargetNode.hCost = TargetNode.ManhattanDistance(endNode);
                        TargetNode.parent = currentNode;
                    }

                }

            }

        }



        Node GetMinFCost()
        {
            float minCost = openList[0].FCost;
            int index = 0;
            for (int i = 0; i < openList.Count; i++)
            {
                if (minCost > openList[i].FCost)
                {
                    minCost = openList[i].FCost;
                    index = i;
                }
            }
            return openList[index];
        }

        void GeneratePath(Node startNode,Node endNode)
        { 
            grid.Path.Clear();
            Node templeNode = endNode;
            while (templeNode.parent != startNode)
            {
                grid.Path.Add(templeNode);
                templeNode = templeNode.parent;
            }
        }
    }

}

