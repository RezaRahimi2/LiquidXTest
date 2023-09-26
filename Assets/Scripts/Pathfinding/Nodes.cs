using System.Collections.Generic;

public enum NodeType
{
    Blocked = 0, UnBlocked = 1
}

//a place holder of Node for using it in custom pathfinding
public class Nodes
{
    public NodeType type = NodeType.UnBlocked;

    public float HCost = -1f;
    public float GCost = -1f;
    public float FCost = -1f;

    public void SetNodeType(NodeType typ)
    {
        type = typ;
    }

    public bool Checked = false;

    public List<Nodes> NeighbourList = new List<Nodes>();

    public Nodes PrevNode = null;

    public Nodes(float x, float y, float z)
    {
    }
}