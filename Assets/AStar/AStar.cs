using System.Collections.Generic;

public class AStar
{
    public AStar(Map map)
    {
        openList = new MinHeap<Node>(map.totalCount >> 3);
        closeList = new List<Node>(map.totalCount >> 3);
        path = new List<Node>(map.rowCount + map.colCount);
        this.map = map;
    }

    private const int DIAGONAL_COST = 14;
    private const int STRAIGHT_COST = 10;
   
    public Map map { get; private set; }
    public Node startNode { get; private set; }
    public Node endNode { get; private set; }

    public List<Node> path { get; private set; }
    public MinHeap<Node> openList { get; private set; }
    public List<Node> closeList { get; private set; }

    public bool FindPath(Node startNode, Node endNode)
    {
        openList.Clear();
        closeList.Clear();
        openList.Clear();

        bool succ = true;
        this.startNode = startNode;
        this.endNode = endNode;
        startNode.g = 0;
        startNode.h = HeuristicDiagonal(startNode, endNode);
        /////////////////////////////
        var node = startNode;
        while(!node.Equals(endNode))
        {
            int startX = System.Math.Max(0, node.x - 1);
            int endX = System.Math.Min(map.colCount - 1, node.x + 1);
            int startY = System.Math.Max(0, node.y - 1);
            int endY = System.Math.Min(map.rowCount - 1, node.y + 1);
            for(int x = startX; x <= endX; x++)
            {
                for(int y = startY; y <= endY; y++)
                {
                    if (node.Equals(x, y))
                        continue;
                    if (!map.CanPass(x, y, out byte resistance))
                        continue;
                    if (!map.CanPass(node.x, y, out byte resistance1) || !map.CanPass(x, node.y, out byte resistance2))
                    {
                        continue;
                    }

                    Node testNode = map.GetNode(x, y);

                    int cost;
                    if(node.x == testNode.x || node.y == testNode.y)
                    {
                        cost = STRAIGHT_COST;
                    }
                    else
                    {
                        cost = DIAGONAL_COST;
                    }

                    int g = node.g + cost;
                    int h = HeuristicDiagonal(testNode, endNode);
                    int f = g + h;
                    if (IsOpen(testNode.x, testNode.y) || IsClose(testNode.x, testNode.y))
                    {
                        if(f < testNode.f)
                        {
                            testNode.g = g;
                            testNode.h = h;
                            testNode.parent = node;
                        }
                    }
                    else
                    {
                        testNode.g = g;
                        testNode.h = h;
                        testNode.parent = node;
                        openList.Push(testNode);
                    }
                }
            }
            closeList.Add(node);

            if (openList.Count <= 0)
            {
                succ = false;
                UnityEngine.Debug.LogError("find path failure!");
                break;
            }
            node = openList.Pop();
        }

        BuildPath();
        return succ;
    }

    private void BuildPath()
    {
        path.Clear();
        Node node = endNode;
        if(node.parent != null)
        {
            path.Add(node);
        }
        while (true)
        {
            var parent = node.parent;
            if(parent == null)
            {
                break;
            }
            path.Add(parent);
            node = parent;
        }
        path.Reverse();
    }

    private bool IsOpen(int x, int y)
    {
        bool isOpen = false;
        for(int i = 0, count = openList.Count; i < count; i++)
        {
            Node node = openList[i];
            if (node.x == x && node.y == y)
            {
                isOpen = true;
                break;
            }
        }
        return isOpen;
    }

    private bool IsClose(int x, int y)
    {
        bool isClose = false;
        for (int i = 0, count = closeList.Count; i < count; i++)
        {
            Node node = closeList[i];
            if (node.x == x && node.y == y)
            {
                isClose = true;
                break;
            }
        }
        return isClose;
    }

    // 启发函数 对角启发函数
    private static int HeuristicDiagonal(Node nodeA, Node nodeB)
    {
        int result;
        int dx = System.Math.Abs(nodeA.x - nodeB.x);
        int dy = System.Math.Abs(nodeA.y - nodeB.y);
        int min = System.Math.Min(dx, dy);
        int add = dx + dy;
        result = min * DIAGONAL_COST + (add - 2 * min) * STRAIGHT_COST;
        result = (int)(result * 1.05f);//加快寻路
        return result;
    }

}
