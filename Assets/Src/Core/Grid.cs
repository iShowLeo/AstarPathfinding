using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace PF
{
    public class Grid : MonoBehaviour
    {
        //地图锚点
        public Vector3 anchor; 

        //路障遮罩
        public LayerMask Mask;
        //地图大小
        public Vector2 MapSize;
        //格子边长
        public float GridSize = 1;


        //节点坐标
        public Node[,] nodes;
        //路径
        public List<Node> Path;

        //格子
        private int Width;
        private int Height;
        //绘制尺寸
        private Vector3 DrawSize;

        private void Awake()
        {
            anchor = Vector3.zero; 
            Width = Mathf.RoundToInt(MapSize.x / GridSize);
            Height = Mathf.RoundToInt(MapSize.y / GridSize);
            Path = new List<Node>();
            DrawSize = new Vector3(GridSize, GridSize, GridSize);
        }

        private void Start()
        {
            nodes = _BuildNodes();
        }

        private void OnDrawGizmos()
        {
            _DrawGrid(); 
            _DrawPath();
        }



        public List<Node> GetNeighbor(Node node)
        {
            List<Node> neibor = new List<Node>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    if (_isWalkableAt(node.X + i, node.Y + j))
                    {
                        neibor.Add(nodes[node.X + i, node.Y + j]);
                    }
                }
            }
            return neibor;
        }

        public Vector3 IndexToPos(Node node)
        {
            float x = anchor.x + node.X * GridSize + GridSize / 2;
            float y = anchor.z + node.Y * GridSize + GridSize / 2;
            return new Vector3(x, 1, y);
        }

        public Node PosToIndex(Transform target)
        {
            Vector3 pos = target.position;
            int x = Mathf.RoundToInt((pos.x - anchor.x - GridSize / 2) / GridSize);
            int y = Mathf.RoundToInt((pos.z - anchor.z - GridSize / 2) / GridSize);
            return nodes[x, y];
        }






        #region privateFunc
        Node[,] _BuildNodes()
        {
            Node[,] _nodes = new Node[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _nodes[i, j] = new Node(i, j, true);
                }
            }
            return _nodes;
        }


        bool _isWalkableAt(int x, int y)
        {
            return _isInside(x, y) && nodes[x, y].walkable;
        }



        void _DrawGrid()
        {
            if (nodes == null) return;
            foreach (var node in nodes)
            { 
                Vector3 pos = IndexToPos(node);
                bool isObs = Physics.CheckSphere(pos, GridSize / 2, Mask);
                node.walkable = !isObs;
                Gizmos.color = isObs ? Color.red : Color.gray;
                Gizmos.DrawWireCube(pos, DrawSize);
            }
        }


        void _DrawPath()
        {
            if (Path==null||Path.Count == 0) return;
            for (int i = 0; i < Path.Count; i++)
            {
                Node node = Path[i];
                Vector3 pos = IndexToPos(node);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(pos, DrawSize);
            }

        }


       



        bool _isInside(int x, int y)
        {
            return (x > 0 && y > 0 && x < Width && y < Height);
        }

        #endregion
    }
}


