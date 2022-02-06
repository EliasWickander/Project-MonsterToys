using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode
{
    public int m_gridPosX;
    public int m_gridPosY;
    
    public bool m_obstacle;
    public Vector3 m_worldPos;

    //Distance from this node to start node
    public int GCost { get; set; } = int.MaxValue;
    
    //Distance from this node to target node
    public int HCost { get; set; }
    public int FCost => GCost + HCost;

    public NavNode PrevNode { get; set; } = null;

    public NavNode(Vector3 worldPos, bool obstacle, int gridPosX, int gridPosY)
    {
        m_obstacle = obstacle;
        m_worldPos = worldPos;
        m_gridPosX = gridPosX;
        m_gridPosY = gridPosY;
    }
}
