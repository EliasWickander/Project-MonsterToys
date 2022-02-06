using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    [SerializeField] 
    private NavGrid m_grid;


    //Find closest path between two points using A* pathfinding
    public List<NavNode> FindPath(Vector3 startPoint, Vector3 endPoint)
    {
        NavNode startNode = m_grid.GetNode(startPoint);

        if (startNode.m_obstacle)
        {
            Debug.LogError("Pathfinding failed: Start node is obstacle");
            return null;
        }
        
        NavNode endNode = m_grid.GetNode(endPoint);
        
        List<NavNode> openList = new List<NavNode>() {startNode};
        List<NavNode> closedList = new List<NavNode>();

        startNode.GCost = 0;
        startNode.HCost = CalculateDistanceCost(startNode, endNode);
        
        while (openList.Count > 0)
        {
            //Find most attractive node to investigate
            NavNode current = GetLowestFCostNode(openList);

            //If at end node, we found a path
            if (current == endNode)
            {
                return CalculatePath(endNode);
            }
            
            openList.Remove(current);
            closedList.Add(current);

            //Check all the neighbors of current node
            foreach (NavNode neighbour in m_grid.GetNeighbours(current))
            {
                if (neighbour.m_obstacle || closedList.Contains(neighbour))
                    continue;

                //If neighbour is closer to goal than current node 
                int tentativeGCost = current.GCost + CalculateDistanceCost(current, neighbour);
                if (tentativeGCost < neighbour.GCost)
                {
                    neighbour.PrevNode = current;
                    neighbour.GCost = tentativeGCost;
                    neighbour.HCost = CalculateDistanceCost(neighbour, endNode);

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        Debug.LogError("Pathfinding failed: Couldn't find path to end point");
        return null;
    }

    //Retrace the path from node
    private List<NavNode> CalculatePath(NavNode endNode)
    {
        List<NavNode> path = new List<NavNode>() {endNode};

        NavNode currentNode = endNode;

        while (currentNode.PrevNode != null)
        {
            path.Add(currentNode.PrevNode);
            currentNode = currentNode.PrevNode;
        }
        
        path.Reverse();
        return path;
    }

    //Get node in list with lowest F cost
    private NavNode GetLowestFCostNode(List<NavNode> nodeList)
    {
        NavNode lowestNode = nodeList[0];
        
        foreach (NavNode node in nodeList)
        {
            if (node.FCost < lowestNode.FCost)
            {
                lowestNode = node;
            }
        }

        return lowestNode;
    }
    
    //Calculate distance cost between two nodes
    private int CalculateDistanceCost(NavNode first, NavNode second)
    {
        int xDist = Mathf.Abs(second.m_gridPosX - first.m_gridPosX);
        int yDist = Mathf.Abs(second.m_gridPosY - first.m_gridPosY);
        int dist = Mathf.Abs(xDist - yDist);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDist, yDist) + MOVE_STRAIGHT_COST * dist;
    }
}
