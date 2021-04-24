using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AIDemo.Graph
{
    public abstract class SparseGraph<NodeType, EdgeType> where NodeType : GraphNode where EdgeType : GraphEdge
    {
        protected List<NodeType> m_Nodes;
        protected List<LinkedList<EdgeType>> m_EdgeList;//每个元素与相同索引值的m_Nodes元素一一对应，即一个节点有多个边，通向其他节点
        public bool isDigraph { get; protected set; }//是有向图吗
        public int nextNodeIndex { get; protected set; }
        public int validNodeAmount { get; protected set; }
        public int allNodeAmount => m_Nodes.Count;
        public int invalidEdgeAmount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < m_Nodes.Count; ++i)
                {
                    if (m_Nodes[i].index != ConstDefine.InvalidNodeIndex)
                    {
                        foreach (var edge in m_EdgeList[i])
                        {
                            if (m_Nodes[edge.toIndex].index != ConstDefine.InvalidNodeIndex)
                            {
                                ++count;
                            }
                        }
                    }
                }
                return count;
            }
        }
        public SparseGraph(bool digraph)
        {
            m_Nodes = new List<NodeType>();
            m_EdgeList = new List<LinkedList<EdgeType>>();
            isDigraph = digraph;
            nextNodeIndex = 0;
        }
        public NodeType GetNode(int index)
        {
            return (index < m_Nodes.Count && index >= 0 && m_Nodes[index].index != ConstDefine.InvalidNodeIndex) ? m_Nodes[index] : default;
        }
        public void Clear()
        {
            m_Nodes.Clear();
            m_EdgeList.Clear();
            nextNodeIndex = validNodeAmount = 0;
        }
        /// <summary>
        /// 表示两节点是否可能相连（只要任一节点不存在即不可相连）
        /// </summary>
        protected bool isValideEdge(int from, int to)
        {
            return (from >= 0 && from < m_Nodes.Count) && (to >= 0 && to < m_Nodes.Count) && m_Nodes[from].index != ConstDefine.InvalidNodeIndex && m_Nodes[to].index != ConstDefine.InvalidNodeIndex;
        }
        public EdgeType GetEdge(int from, int to)
        {
            if (isValideEdge(from, to))
            {
                var edgeList = m_EdgeList[from];
                if (edgeList != null)
                {
                    foreach (var edge in edgeList)
                    {
                        if (edge.toIndex == to)
                        {
                            return edge;
                        }
                    }
                }
            }
            return default;
        }
        public int AddNode(NodeType node)
        {
            validNodeAmount++;
            m_Nodes.Add(node);
            m_EdgeList.Add(new LinkedList<EdgeType>());
            return nextNodeIndex++;
        }
        public void RemoveNode(int nodeIndex)
        {
            if (nodeIndex >= 0 && nodeIndex < m_Nodes.Count)
            {
                --validNodeAmount;
                m_Nodes[nodeIndex].index = ConstDefine.InvalidNodeIndex;
                m_EdgeList[nodeIndex].Clear();//删或者不删都可以，删了节省内存，不删可以执行节点的恢复操作
            }
        }
        public void AddEdge(EdgeType edge)
        {
            if (isValideEdge(edge.fromIndex, edge.toIndex))
            {
                m_EdgeList[edge.fromIndex].AddLast(edge);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromIndex"></param>
        /// <param name="edgeCallback">返回true停止循环</param>
        /// <returns></returns>
        public void ForEachEdge(int fromIndex, System.Predicate<EdgeType> edgeCallback)
        {
            if (edgeCallback == null)
            {
                return;
            }
            var node = GetNode(fromIndex);
            if (node != null)
            {
                foreach (var edge in m_EdgeList[fromIndex])
                {
                    if (edge.toIndex != ConstDefine.InvalidNodeIndex && edgeCallback(edge))
                    {
                        return;
                    }
                }
            }
        }
        public void ForEachNode(System.Predicate<NodeType> nodeCallback)
        {
            if (nodeCallback == null)
            {
                return;
            }
            for (int i = 0; i < m_Nodes.Count; ++i)
            {
                if (m_Nodes[i].index != ConstDefine.InvalidNodeIndex && nodeCallback(m_Nodes[i]))
                {
                    return;
                }
            }
        }
        public void RemoveEdge(int from, int to)
        {
            //不活跃的边不用删除
            if (isValideEdge(from, to))
            {
                var edgeList = m_EdgeList[from];
                if (edgeList != null)
                {
                    foreach (var edge in edgeList)
                    {
                        if (edge.toIndex == to)
                        {
                            edgeList.Remove(edge);
                            break;
                        }
                    }
                }
            }
        }
        //存储或读取图
        public void Save() { }
        public abstract void Load();
    }
}
