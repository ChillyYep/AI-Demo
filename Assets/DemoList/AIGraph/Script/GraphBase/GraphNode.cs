using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Graph
{
    public abstract class GraphNode
    {
        public virtual int index { get; set; } = ConstDefine.InvalidNodeIndex;
        public GraphNode(int index)
        {
            this.index = index;
        }
    }
}
