using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    public bool m_debug = true;

    public Vector2 m_gridWorldSize;
    public float m_nodeRadius;
    private NavNode[,] m_grid;

    private float m_nodeDiameter;
    private int m_gridSizeX;
    private int m_gridSizeY;

    public PolygonCollider2D m_boundsCollider;

    private void OnValidate()
    {
        Start();
    }

    private void Start()
    {
        m_nodeDiameter = m_nodeRadius * 2;
        m_gridSizeX = Mathf.RoundToInt(m_gridWorldSize.x / m_nodeDiameter);
        m_gridSizeY = Mathf.RoundToInt(m_gridWorldSize.y / m_nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        m_grid = new NavNode[m_gridSizeX, m_gridSizeY];
        Vector3 worldBottomLeft = transform.position;

        for (int x = 0; x < m_gridSizeX; x++)
        {
            for (int y = 0; y < m_gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * m_nodeDiameter + m_nodeRadius) +
                                     Vector3.up * (y * m_nodeDiameter + m_nodeRadius);
                bool isObstacle = !m_boundsCollider.OverlapPoint(worldPoint);
                m_grid[x, y] = new NavNode(worldPoint, isObstacle);
            }
        }
    }

    public NavNode GetNode(Vector3 worldPosition)
    {
        worldPosition -= transform.position;
        
        float percentX = (worldPosition.x + m_gridWorldSize.x / 2) / m_gridWorldSize.x;
        float percentY = (worldPosition.z + m_gridWorldSize.y / 2) / m_gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((m_gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((m_gridSizeY - 1) * percentY);
        return m_grid[x, y];
    }

    private void OnDrawGizmos()
    {
        if (!m_debug)
            return;
        
        if (m_grid == null)
            return;
        
        foreach (NavNode node in m_grid)
        {
            Gizmos.color = node.m_obstacle ? Color.red : Color.cyan;
            Gizmos.DrawCube(node.m_worldPos, new Vector3(m_nodeRadius, m_nodeRadius, 0.1f));
        }
    }
}
