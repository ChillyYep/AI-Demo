//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class JSP
//{
//    public class Node
//    {
//        public int x, y;
//        public bool isJumpPoint;
//        public bool visited;
//        public bool isObstacle;
//    }
//    private Node[][] m_logicGrids;
//    private Vector2Int[] m_neighboors = new Vector2Int[]
//        {
//            new Vector2Int( -1,-1),
//            new Vector2Int( 0,-1),
//            new Vector2Int( 1,-1),
//            new Vector2Int( 1,0),
//            new Vector2Int( 1,1),
//            new Vector2Int( 0,1),
//            new Vector2Int( -1,1),
//            new Vector2Int( -1,0),
//        };

//    public bool IsForcedNeighboor(int x, int y, Vector2Int fatherPos)
//    {
//        Vector2Int curPos = new Vector2Int(x, y);
//        Vector2Int offsetPos;
//        bool existObstacle = false;
//        for (int i = 0; i < m_neighboors.Length; ++i)
//        {
//            offsetPos = m_neighboors[i] + curPos;
//            if (offsetPos.x >= 0 && offsetPos.x < m_logicGrids.Length &&
//                offsetPos.y >= 0 && offsetPos.y < m_logicGrids[0].Length &&
//                m_logicGrids[offsetPos.y][offsetPos.x].isObstacle)
//            {
//                existObstacle = true;
//                break;
//            }
//        }
//        if (!existObstacle)
//        {
//            return false;
//        }

//        //左
//        if (x - 1 >= 0 && m_logicGrids[y][x - 1].cost < minCost)
//        {
//            m_logicGrids[y][x].moveDir = new Vector2Int(-1, 0);
//            minCost = m_logicGrids[y][x - 1].cost;
//        }
//        //左上
//        if (x - 1 >= 0 && y - 1 >= 0 && m_logicGrids[y - 1][x - 1].cost < minCost)
//        {
//            m_logicGrids[y][x].moveDir = new Vector2Int(-1, -1);
//            minCost = m_logicGrids[y - 1][x - 1].cost;
//        }
//        //上
//        if (y - 1 >= 0 && m_logicGrids[y - 1][x].cost < minCost)
//        {
//            m_logicGrids[y][x].moveDir = new Vector2Int(0, -1);
//            minCost = m_logicGrids[y - 1][x].cost;
//        }
//        //右上
//        if (y - 1 >= 0 && x + 1 < colCount && m_logicGrids[y - 1][x + 1].cost < minCost)
//        {
//            m_logicGrids[y][x].moveDir = new Vector2Int(1, -1);
//            minCost = m_logicGrids[y - 1][x + 1].cost;
//        }
//        //右
//        if (x + 1 < colCount && m_logicGrids[y][x + 1].cost < minCost)
//        {
//            m_logicGrids[y][x].moveDir = new Vector2Int(1, 0);
//            minCost = m_logicGrids[y][x + 1].cost;
//        }
//        //右下
//        if (x + 1 < colCount && y + 1 < rowCount && m_logicGrids[y + 1][x + 1].cost < minCost)
//        {
//            m_logicGrids[y][x].moveDir = new Vector2Int(1, 1);
//            minCost = m_logicGrids[y + 1][x + 1].cost;
//        }
//        //下
//        if (y + 1 < rowCount && m_logicGrids[y + 1][x].cost < minCost)
//        {
//            m_logicGrids[y][x].moveDir = new Vector2Int(0, 1);
//            minCost = m_logicGrids[y + 1][x].cost;
//        }
//        //左下
//        if (y + 1 < rowCount && x - 1 >= 0 && m_logicGrids[y + 1][x - 1].cost < minCost)
//        {
//            m_logicGrids[y][x].moveDir = new Vector2Int(-1, 1);
//            minCost = m_logicGrids[y + 1][x - 1].cost;
//        }
//    }
//    /// <summary>
//    /// 跳点判断
//    /// </summary>
//    /// <returns></returns>
//    public bool IsJumpPoint(int x, int y, Vector2Int sourceGridPosition, Vector2Int targetGridPosition)
//    {
//        if ((sourceGridPosition.x == x && sourceGridPosition.y == y) ||
//            (targetGridPosition.x == x && targetGridPosition.y == y))
//        {
//            return true;
//        }
//    }
//    public void CreateLogicGrids(int rowCount, int colCount, Vector2Int[] obstaclePositions)
//    {
//        m_logicGrids = new Node[rowCount][];
//        for (int i = 0; i < rowCount; ++i)
//        {
//            m_logicGrids[i] = new Node[colCount];
//            for (int j = 0; j < colCount; ++j)
//            {
//                m_logicGrids[i][j] = new Node()
//                {
//                    x = j,
//                    y = i,
//                    isJumpPoint = false,
//                };
//            }
//        }
//        for (int i = 0; i < obstaclePositions.Length; ++i)
//        {
//            m_logicGrids[obstaclePositions[i].x][obstaclePositions[i].y].isObstacle = true;
//        }
//    }
//    private bool CanWalk(int x, int y)
//    {
//        return !m_logicGrids[y][x].isObstacle;
//    }
//    public Vector2Int Jump(int x, int y, int px, int py)
//    {
//        int dx = x - px;
//        int dy = y - py;
//        // 斜对角方向
//        if (dx != 0 && dy != 0)
//        {
//            if ((CanWalk(x - dx, y + dy) && !CanWalk(x - dx, y)) ||
//                (CanWalk(x + dx, y - dy) && !CanWalk(x, y - dy)))
//            {
//                return new Vector2Int(x, y);
//            }
//            if(CanWalk(x+dx,y))
//            {
//                if()
//            }
//        }
//        else
//        {
//            // 垂直方向
//            if (dx != 0)
//            {

//            }
//            else
//            // 水平方向
//            {

//            }
//        }
//    }
//    public void Search(int rowCount, int colCount, Vector2Int sourceGridPosition, Vector2Int targetGridPosition)
//    {
//        List<Node> openList = new List<Node>();
//    }
//}
