using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode
{
    public bool m_obstacle;
    public Vector3 m_worldPos;

    public NavNode(Vector3 worldPos, bool obstacle)
    {
        m_obstacle = obstacle;
        m_worldPos = worldPos;
    }
}
