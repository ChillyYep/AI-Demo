using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFeild
{
    public class Node
    {
        public int x, y;
        public int cost;
        public Vector2Int moveDir;
        public bool visited;
        public bool isObstacle;
    }

    private Node[][] m_logicGrids;

    public void CreateLogicGrids(int rowCount, int colCount, Vector2Int[] obstaclePositions)
    {
        m_logicGrids = new Node[rowCount][];
        for (int i = 0; i < rowCount; ++i)
        {
            m_logicGrids[i] = new Node[colCount];
            for (int j = 0; j < colCount; ++j)
            {
                m_logicGrids[i][j] = new Node()
                {
                    x = j,
                    y = i,
                    cost = 0,
                    moveDir = Vector2Int.zero
                };
            }
        }
        for (int i = 0; i < obstaclePositions.Length; ++i)
        {
            m_logicGrids[obstaclePositions[i].x][obstaclePositions[i].y].isObstacle = true;
        }
    }
    public void Search(int rowCount, int colCount, Vector2Int targetGridPosition)
    {
        Queue<Node> queue = new Queue<Node>();
        m_logicGrids[targetGridPosition.x][targetGridPosition.y].cost = 0;
        m_logicGrids[targetGridPosition.x][targetGridPosition.y].visited = true;
        queue.Enqueue(m_logicGrids[targetGridPosition.x][targetGridPosition.y]);
        // 更新距离场
        while (queue.Count > 0)
        {
            var first = queue.Dequeue();
            if (first.isObstacle)
            {
                continue;
            }
            if (first.x - 1 >= 0 && !m_logicGrids[first.y][first.x - 1].visited)
            {
                m_logicGrids[first.y][first.x - 1].cost = m_logicGrids[first.y][first.x - 1].isObstacle ? int.MaxValue : first.cost + 1;
                m_logicGrids[first.y][first.x - 1].visited = true;
                queue.Enqueue(m_logicGrids[first.y][first.x - 1]);
            }
            if (first.y - 1 >= 0 && !m_logicGrids[first.y - 1][first.x].visited)
            {
                m_logicGrids[first.y - 1][first.x].cost = m_logicGrids[first.y - 1][first.x].isObstacle ? int.MaxValue : first.cost + 1;
                m_logicGrids[first.y - 1][first.x].visited = true;
                queue.Enqueue(m_logicGrids[first.y - 1][first.x]);
            }
            if (first.x + 1 <= colCount - 1 && !m_logicGrids[first.y][first.x + 1].visited)
            {
                m_logicGrids[first.y][first.x + 1].cost = m_logicGrids[first.y][first.x + 1].isObstacle ? int.MaxValue : first.cost + 1;
                m_logicGrids[first.y][first.x + 1].visited = true;
                queue.Enqueue(m_logicGrids[first.y][first.x + 1]);
            }
            if (first.y + 1 <= rowCount - 1 && !m_logicGrids[first.y + 1][first.x].visited)
            {
                m_logicGrids[first.y + 1][first.x].cost = m_logicGrids[first.y + 1][first.x].isObstacle ? int.MaxValue : first.cost + 1;
                m_logicGrids[first.y + 1][first.x].visited = true;
                queue.Enqueue(m_logicGrids[first.y + 1][first.x]);
            }
        }

        for (int i = 0; i < m_logicGrids.Length; ++i)
        {
            for (int j = 0; j < m_logicGrids[i].Length; ++j)
            {
                int minCost = int.MaxValue;
                m_logicGrids[i][j].moveDir = Vector2Int.zero;
                //左
                if (j - 1 >= 0 && m_logicGrids[i][j - 1].cost < minCost)
                {
                    m_logicGrids[i][j].moveDir = new Vector2Int(-1, 0);
                    minCost = m_logicGrids[i][j - 1].cost;
                }
                //左上
                if (j - 1 >= 0 && i - 1 >= 0 && m_logicGrids[i - 1][j - 1].cost < minCost)
                {
                    m_logicGrids[i][j].moveDir = new Vector2Int(-1, -1);
                    minCost = m_logicGrids[i - 1][j - 1].cost;
                }
                //上
                if (i - 1 >= 0 && m_logicGrids[i - 1][j].cost < minCost)
                {
                    m_logicGrids[i][j].moveDir = new Vector2Int(0, -1);
                    minCost = m_logicGrids[i - 1][j].cost;
                }
                //右上
                if (i - 1 >= 0 && j + 1 < colCount && m_logicGrids[i - 1][j + 1].cost < minCost)
                {
                    m_logicGrids[i][j].moveDir = new Vector2Int(1, -1);
                    minCost = m_logicGrids[i - 1][j + 1].cost;
                }
                //右
                if (j + 1 < colCount && m_logicGrids[i][j + 1].cost < minCost)
                {
                    m_logicGrids[i][j].moveDir = new Vector2Int(1, 0);
                    minCost = m_logicGrids[i][j + 1].cost;
                }
                //右下
                if (j + 1 < colCount && i + 1 < rowCount && m_logicGrids[i + 1][j + 1].cost < minCost)
                {
                    m_logicGrids[i][j].moveDir = new Vector2Int(1, 1);
                    minCost = m_logicGrids[i + 1][j + 1].cost;
                }
                //下
                if (i + 1 < rowCount && m_logicGrids[i + 1][j].cost < minCost)
                {
                    m_logicGrids[i][j].moveDir = new Vector2Int(0, 1);
                    minCost = m_logicGrids[i + 1][j].cost;
                }
                //左下
                if (i + 1 < rowCount && j - 1 >= 0 && m_logicGrids[i + 1][j - 1].cost < minCost)
                {
                    m_logicGrids[i][j].moveDir = new Vector2Int(-1, 1);
                    minCost = m_logicGrids[i + 1][j - 1].cost;
                }
            }
        }
    }

    public Node GetLogicNode(int gridX, int gridY)
    {
        return m_logicGrids[gridX][gridY];
    }
}
