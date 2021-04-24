using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Graph
{
    public enum NodeType
    {
        Normal,
        Source,
        Target
    }
    public class NavGraphNode : GraphNode
    {
        public Vector3 nodePos { get; protected set; }
        public NodeType nodeType { get; protected set; }
        public NavGraphNode(int index, Vector3 nodePos = default, NodeType nodeType = NodeType.Normal) : base(index)
        {
            this.nodePos = nodePos;
        }
    }
}
