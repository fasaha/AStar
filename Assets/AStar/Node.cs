using System;

public class Node : IComparable<Node>
{
    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public int x { get; private set; }
    public int y { get; private set; }

    public int f { get { return g + h; } }
    public int g;
    public int h;

    public Node parent;

    public bool Equals(Node node)
    {
        return node.x == x && node.y == y;
    }

    public bool Equals(int x, int y)
    {
        return this.x == x && this.y == y;
    }

    public override string ToString()
    {
        return $"node(x={x}, y={y}, f={f}, g={g}, h={h}, parent.x={parent?.x}, parent.y={parent?.y})";
    }

    public int CompareTo(Node other)
    {
        return this.f - other.f;
    }
}
