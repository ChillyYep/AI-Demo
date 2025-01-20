using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class GridBuilder : MonoBehaviour
{
    [Serializable]
    public class AIOwner
    {
        public Transform m_aiNode;
        public Vector2Int m_initPosition;

        [HideInInspector]
        public float m_timer;
        [HideInInspector]
        public Vector2Int m_curPosition;
        [HideInInspector]
        public Vector2Int m_nextPosition;
    }
    public Transform m_gridRoot;
    public Transform m_obstacleRoot;
    public GameObject m_gridTemplate;
    public GameObject m_obstacleTemplate;
    public int m_rowCount;
    public int m_colCount;


    public Vector2Int m_targetGridPosition;

    public float m_speed = 1f;

    public AIOwner[] m_aiNodes;


    public bool m_enablePlay;

    public Vector2Int[] m_obstaclePositions;

    private GameObject[][] m_grids;

    private List<GameObject> m_obstacles;

    private FlowFeild flowFeildSearch;

    private void CreateGridScene()
    {
        Vector3 initPosition = Vector3.zero;
        Vector3 size = Vector3.one;
        // 生成格子
        m_grids = new GameObject[m_rowCount][];
        for (int i = 0; i < m_rowCount; ++i)
        {
            m_grids[i] = new GameObject[m_colCount];
            for (int j = 0; j < m_colCount; ++j)
            {
                var gridGo = Instantiate(m_gridTemplate, m_gridRoot);
                gridGo.SetActive(true);
                gridGo.transform.localPosition = initPosition + new Vector3(size.x * i, 0.0f, size.y * j);
                gridGo.transform.localScale = Vector3.one;
                m_grids[i][j] = gridGo;
            }
        }
        // 生成障碍物
        m_obstacles = new List<GameObject>();
        for (int i = 0; i < m_obstaclePositions.Length; ++i)
        {
            var obstacle = Instantiate(m_obstacleTemplate, m_obstacleRoot);
            obstacle.SetActive(true);
            obstacle.transform.localPosition = initPosition + new Vector3(size.x * m_obstaclePositions[i].x, 0.0f, size.y * m_obstaclePositions[i].y);
            obstacle.transform.localScale = Vector3.one;
            m_obstacles.Add(obstacle);
        }
    }

    private void InitAllAIPos()
    {
        for (int i = 0; i < m_aiNodes.Length; ++i)
        {
            if (m_aiNodes[i] != null)
            {
                m_aiNodes[i].m_aiNode.localPosition = m_grids[m_aiNodes[i].m_initPosition.x][m_aiNodes[i].m_initPosition.y].transform.localPosition;
                m_aiNodes[i].m_curPosition = m_aiNodes[i].m_initPosition;
                m_aiNodes[i].m_nextPosition = m_aiNodes[i].m_initPosition;
            }
        }
    }

    private void Start()
    {
        CreateGridScene();
        flowFeildSearch = new FlowFeild();
        flowFeildSearch.CreateLogicGrids(m_rowCount, m_colCount, m_obstaclePositions);
        InitAllAIPos();
        flowFeildSearch.Search(m_rowCount, m_colCount, m_targetGridPosition);
#if UNITY_EDITOR
        for (int i = 0; i < m_grids.Length; ++i)
        {
            for (int j = 0; j < m_grids[i].Length; ++j)
            {
                var text = m_grids[i][j].GetComponentInChildren<TextMeshProUGUI>();
                text.text = flowFeildSearch.GetLogicNode(i, j).moveDir.ToString();
            }
        }
#endif
    }

    private void Update()
    {
        if (!m_enablePlay)
        {
            return;
        }
        foreach (var aiNode in m_aiNodes)
        {
            if (aiNode.m_curPosition == m_targetGridPosition)
            {
                continue;
            }
            if (aiNode.m_timer == 0 || aiNode.m_timer >= 1)
            {
                aiNode.m_timer = 0;
                aiNode.m_curPosition = aiNode.m_nextPosition;
                aiNode.m_nextPosition = aiNode.m_curPosition + flowFeildSearch.GetLogicNode(aiNode.m_curPosition.y, aiNode.m_curPosition.x).moveDir;
            }
            aiNode.m_aiNode.localPosition = Vector3.Lerp(m_grids[aiNode.m_curPosition.y][aiNode.m_curPosition.x].transform.localPosition, m_grids[aiNode.m_nextPosition.y][aiNode.m_nextPosition.x].transform.localPosition, aiNode.m_timer);

            aiNode.m_timer += Time.deltaTime * m_speed;
        }

    }
}
