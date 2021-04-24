using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Graph
{
    /// <summary>
    /// 广度优先搜索，优点：可以找到最优路径，缺点：在大地图中寻路，内存消耗会很大，主要是由于队列进的快出的慢
    /// </summary>
    /// <typeparam name="NodeType"></typeparam>
    /// <typeparam name="EdgeType"></typeparam>
    public class Graph_SearchBFS<NodeType, EdgeType> : GraphSearch<NodeType, EdgeType> where NodeType : GraphNode where EdgeType : GraphEdge, new()
    {
        SparseGraph<NodeType, EdgeType> graph;
        int[] m_Visited;//节点访问状态
        int[] m_Route;//最终路径
        public bool hasFound { get; protected set; }
        private int srcIndex;
        private int dstIndex;

        private Queue<EdgeType> edgeQueue = new Queue<EdgeType>();
        public override void SetSrcDst(int source, int target)
        {
            srcIndex = source;
            dstIndex = target;
        }
        public override void SetUp(SparseGraph<NodeType, EdgeType> graph, int source = ConstDefine.InvalidNodeIndex, int target = ConstDefine.InvalidNodeIndex)
        {
            this.graph = graph;
            srcIndex = source;
            dstIndex = target;
            m_Visited = new int[graph.allNodeAmount];
            m_Route = new int[graph.allNodeAmount];
            hasFound = false;
            Initialize(m_Visited, (int)VisitedState.UnVisited);
            Initialize(m_Route, (int)VisitedState.NoParentAssigned);
        }
        private void Initialize(int[] arr, int value)
        {
            for (int i = 0; i < arr.Length; ++i)
            {
                arr[i] = value;
            }
        }
        public override void Search()
        {
            edgeQueue.Clear();
            var srcEdge = new EdgeType()
            {
                fromIndex = srcIndex,
                toIndex = srcIndex
            };
            edgeQueue.Enqueue(srcEdge);
            while (edgeQueue.Count > 0)
            {
                var edge = edgeQueue.Dequeue();
                m_Route[edge.toIndex] = edge.fromIndex;
                m_Visited[edge.toIndex] = (int)VisitedState.Visited;
                if (edge.toIndex == dstIndex)
                {
                    hasFound = true;
                    edgeQueue.Clear();
                    return;
                }
                graph.ForEachEdge(edge.toIndex, (toEdge) =>
                {
                    if (m_Visited[toEdge.toIndex] == (int)VisitedState.UnVisited)
                    {
                        edgeQueue.Enqueue(toEdge);
                    }
                    return false;
                });
            }
            edgeQueue.Clear();
            hasFound = false;
        }

        public override List<int> GetPath()
        {
            return GetPath(srcIndex, dstIndex, m_Route, hasFound);
        }
    }
}
