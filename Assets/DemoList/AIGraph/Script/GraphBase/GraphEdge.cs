using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Graph
{
    public abstract class GraphEdge
    {
        public virtual int fromIndex { get; set; }
        public virtual int toIndex { get; set; }
        public virtual float cost { get; set; }
        public GraphEdge() { }
    }
}
