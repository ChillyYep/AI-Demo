using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.AITree
{
    public enum EBTStatus
    {
        BT_INVALID,
        BT_SUCCESS,
        BT_FAILURE,
        BT_RUNNING,
    }
    public abstract class BehaviorTask
    {
        public EBTStatus m_status;
        public int id { get; protected set; }
        public BehaviorNode node { get; protected set; }
        public virtual void Init(BehaviorNode node)
        {
            this.node = node;
            id = node.id;
        }
        public virtual void Clear()
        {
            this.m_status = EBTStatus.BT_INVALID;
            this.node = null;
            this.id = -1;
        }
        public string GetClassNameString()
        {
            return node == null ? "SubBT" : node.className;
        }
        public virtual int GetNextStateId()
        {
            return -1;
        }
    }
    public abstract class BranchTask : BehaviorTask
    {

    }
}
