using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public bool m_obstacle;
    public Vector3 m_worldPos;

    public int gCost;
    public int hCost;
    public int FCost => gCost + hCost;

    public GridNode(Vector3 worldPos, bool obstacle)
    {
        m_obstacle = obstacle;
        m_worldPos = worldPos;
    }
}
