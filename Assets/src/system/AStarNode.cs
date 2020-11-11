using UnityEngine;

public class AStarNode
{
    public Vector2Int Position;
    public int gCost;
    public int hCost;
    public int fCost;

    public AStarNode PreviousNode;

    public AStarNode(Vector2Int position)
    {
        Position = position;
        gCost = int.MaxValue;
        CalculateFCost();
        PreviousNode = null;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
