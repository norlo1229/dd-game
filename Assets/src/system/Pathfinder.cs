using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private List<AStarNode> OpenList;
    private List<AStarNode> ClosedList;
    private List<Vector2Int> TileList;

    public Pathfinder(List<Vector2Int> tiles)
    {
        TileList = tiles;
        OpenList = new List<AStarNode>();
        ClosedList = new List<AStarNode>();
    }

    public List<Vector2Int> FindPath(Vector2Int startPosition, Vector2Int endPosition)
    {
        var startNode = new AStarNode(startPosition);
        var endNode = new AStarNode(endPosition);

        //initialize cost of start node.
        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        OpenList.Add(startNode);

        //loop and find lowest cost nodes.
        while(OpenList.Count > 0)
        {
            AStarNode currentNode = GetLowestFCostNode(OpenList);

            //reached end, build path.
            if(currentNode.Position == endNode.Position)
            {
                return CalculatePath(currentNode);
            }

            OpenList.Remove(currentNode);
            ClosedList.Add(currentNode);

            //get neightbords and calculate costs
            foreach(AStarNode neighborNode  in GetNeighbors(currentNode))
            {
                if (ClosedList.Contains(neighborNode))
                {
                    continue;
                }

                //g-cost from current node to the neighbor.
                int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighborNode);

                if(tentativeGCost < neighborNode.gCost)
                {
                    neighborNode.PreviousNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistance(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!OpenList.Contains(neighborNode))
                    {
                        OpenList.Add(neighborNode); 
                    }
                }
            }
        }

        return null;
    }

    private List<Vector2Int> CalculatePath(AStarNode endNode)
    {
        var path = new List<Vector2Int>();
        var currentNode = endNode;
        path.Add(currentNode.Position);
        
        while(currentNode.PreviousNode != null)
        {
            path.Add(currentNode.PreviousNode.Position);
            currentNode = currentNode.PreviousNode;
        }

        //remove last node since this is where we start and it cost extra movement.
        path.RemoveAt(path.Count - 1);

        return path;
    }

    private List<AStarNode> GetNeighbors(AStarNode currentNode)
    {
        var neighbors = new List<AStarNode>();

        //left
        if(ContainsTileAtPosition(new Vector2Int(currentNode.Position.x - 1, currentNode.Position.y)))
        {
            neighbors.Add(new AStarNode(new Vector2Int(currentNode.Position.x - 1, currentNode.Position.y)));
        }

        //right
        if (ContainsTileAtPosition(new Vector2Int(currentNode.Position.x + 1, currentNode.Position.y)))
        {
            neighbors.Add(new AStarNode(new Vector2Int(currentNode.Position.x + 1, currentNode.Position.y)));
        }

        //up
        if (ContainsTileAtPosition(new Vector2Int(currentNode.Position.x, currentNode.Position.y + 1)))
        {
            neighbors.Add(new AStarNode(new Vector2Int(currentNode.Position.x, currentNode.Position.y + 1)));
        }

        //down
        if (ContainsTileAtPosition(new Vector2Int(currentNode.Position.x , currentNode.Position.y - 1)))
        {
            neighbors.Add(new AStarNode(new Vector2Int(currentNode.Position.x, currentNode.Position.y - 1)));
        }

        //up-left
        if (ContainsTileAtPosition(new Vector2Int(currentNode.Position.x - 1, currentNode.Position.y + 1)))
        {
            neighbors.Add(new AStarNode(new Vector2Int(currentNode.Position.x - 1, currentNode.Position.y + 1)));
        }

        //up-right
        if (ContainsTileAtPosition(new Vector2Int(currentNode.Position.x + 1, currentNode.Position.y + 1)))
        {
            neighbors.Add(new AStarNode(new Vector2Int(currentNode.Position.x + 1, currentNode.Position.y + 1)));
        }

        //down-left
        if (ContainsTileAtPosition(new Vector2Int(currentNode.Position.x - 1, currentNode.Position.y - 1)))
        {
            neighbors.Add(new AStarNode(new Vector2Int(currentNode.Position.x - 1, currentNode.Position.y - 1)));
        }

        //down-right
        if (ContainsTileAtPosition(new Vector2Int(currentNode.Position.x + 1, currentNode.Position.y - 1)))
        {
            neighbors.Add(new AStarNode(new Vector2Int(currentNode.Position.x + 1, currentNode.Position.y - 1)));
        }

        return neighbors;
    }

    private int CalculateDistance(AStarNode a, AStarNode b)
    {
        int xDistance = Mathf.Abs(a.Position.x - b.Position.x);
        int yDistance = Mathf.Abs(a.Position.y - b.Position.y);
        int remaining = Mathf.Abs(xDistance - yDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private AStarNode GetLowestFCostNode(List<AStarNode> pathNodes)
    {
        AStarNode lowestFCostNode = pathNodes[0];

        for(int i = 0; i< pathNodes.Count; i++)
        {
            if(pathNodes[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodes[i];
            }
        }

        return lowestFCostNode;
    }

    private bool ContainsTileAtPosition(Vector2Int position)
    {
        if (TileList.Contains(position))
        {
            return true;
        }

        return false;
    }
}
