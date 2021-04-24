using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AIDemo.Graph
{
    enum VisitedState
    {
        UnVisited,
        Visited,
        NoParentAssigned = -1
    }
    public interface ISearch
    {
        void SetSrcDst(int source, int target);
        void Search();
        List<int> GetPath();

    }
    public abstract class GraphSearch : ISearch
    {
        public abstract void SetSrcDst(int source, int target);
        public abstract void Search();
        public abstract List<int> GetPath();
        protected List<int> GetPath(int srcIndex, int dstIndex, int[] routes, bool hasFound)
        {
            var pathList = new List<int>();
            int backIndex = dstIndex;
            if (hasFound)
            {
                while (backIndex != srcIndex)
                {
                    pathList.Add(backIndex);
                    backIndex = routes[backIndex];
                }
                pathList.Add(srcIndex);
            }
            return pathList;
        }
        public T As<T>() where T: GraphSearch
        {
            return this as T;
        }
    }
    public abstract class GraphSearch<NodeType, EdgeType> : GraphSearch where NodeType : GraphNode where EdgeType : GraphEdge, new()
    {
        protected GraphSearch()
        {
        }
        public abstract void SetUp(SparseGraph<NodeType, EdgeType> graph, int source = ConstDefine.InvalidNodeIndex, int target = ConstDefine.InvalidNodeIndex);
        public Graph_SearchDFS<NodeType, EdgeType> AsGraphSearchDFS()
        {
            return As<Graph_SearchDFS<NodeType, EdgeType>>();
        }
        public Graph_SearchBFS<NodeType, EdgeType> AsGraphSearchBFS()
        {
            return As<Graph_SearchBFS<NodeType, EdgeType>>();
        }
    }
    /// <summary>
    /// 深度优先搜索，优点：内存消耗低，效率高，缺电：不能找到最优路径
    /// </summary>
    /// <typeparam name="NodeType"></typeparam>
    /// <typeparam name="EdgeType"></typeparam>
    public class Graph_SearchDFS<NodeType, EdgeType> : GraphSearch<NodeType, EdgeType> where NodeType : GraphNode where EdgeType : GraphEdge, new()
    {

        SparseGraph<NodeType, EdgeType> graph;
        int[] m_Visited;//节点访问状态
        int[] m_Route;//最终路径
        public bool hasFound { get; protected set; }
        private int srcIndex;
        private int dstIndex;

        private Stack<EdgeType> edgeStack = new Stack<EdgeType>();
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
        public override List<int> GetPath()
        {
            return GetPath(srcIndex, dstIndex, m_Route, hasFound);
        }
        public override void Search()
        {
            edgeStack.Clear();
            var srcEdge = new EdgeType()
            {
                fromIndex = srcIndex,
                toIndex = srcIndex
            };
            edgeStack.Push(srcEdge);
            while (edgeStack.Count > 0)
            {
                var edge = edgeStack.Pop();
                m_Route[edge.toIndex] = edge.fromIndex;
                m_Visited[edge.toIndex] = (int)VisitedState.Visited;
                if (edge.toIndex == dstIndex)
                {
                    hasFound = true;
                    edgeStack.Clear();
                    return;
                }
                graph.ForEachEdge(edge.toIndex, (toEdge) =>
                {
                    if (m_Visited[toEdge.toIndex] == (int)VisitedState.UnVisited)
                    {
                        edgeStack.Push(toEdge);
                        return true;
                    }
                    return false;
                });
            }
            edgeStack.Clear();
            hasFound = false;
        }
    }
}
