using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AIDemo.AITree
{
    public abstract class BehaviorNode
    {
        public int id { get; protected set; }
        public string className { get; set; }
        public BehaviorNode parent { get; protected set; }
        public List<BehaviorNode> children { get; protected set; }
        public BehaviorNode customCondition { get; protected set; }
        public List<BehaviorNode> preConditions { get; protected set; }
        public List<T> GetChildren<T>() where T : BehaviorNode
        {
            List<T> _children = new List<T>();
            for (int i = 0; i < children.Count; ++i)
            {
                if (children[i] is T child)
                {
                    _children.Add(child);
                }
            }
            return _children;
        }
        public BehaviorNode GetById(int nodeId)
        {
            for (int i = 0; i < children.Count; ++i)
            {
                if (children[i].id == nodeId)
                {
                    return children[i];
                }
            }
            return null;
        }
        public void Clear()
        {
            if (children != null)
            {
                children.Clear();
            }
        }
        public void AddChild(BehaviorNode node)
        {
            node.parent = this;
            if (children == null)
            {
                children = new List<BehaviorNode>();
            }
            children.Add(node);
        }
    }
    public class BehaviorTree : BehaviorNode
    {

    }
}
