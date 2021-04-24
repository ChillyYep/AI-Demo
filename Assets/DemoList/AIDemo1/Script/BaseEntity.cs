using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Demo1
{
    using IDType = UInt32;
    public enum EntityObjType
    {
        Vehicle,
        Miner
    }
    public abstract class BaseEntity
    {
        public IDType ID { get; protected set; }
        static IDType m_NextValidID = 0;
        public string name { get; protected set; }
        public abstract StateType stateType { get; }
        public abstract EntityObjType EntityObjType { get; }
        public BaseEntity(string name)
        {
            ID = ++m_NextValidID;
            this.name = name;
        }
        public abstract bool HandleMessage(Telegram telegram);
        public abstract void Start();
        public abstract void Update();
    }
    public abstract class BaseEntity<T> : BaseEntity where T : BaseEntity
    {
        public BaseEntity(string name) : base(name)
        {
        }
        protected StateMachine<T> stateMachine;
        public StateMachine<T> GetFSM() => stateMachine;
        public override StateType stateType => stateMachine.currentState.stateType;
    }
    public class Miner : BaseEntity<Miner>
    {
        public override EntityObjType EntityObjType => EntityObjType.Miner;

        public Miner(string name) : base(name)
        {
            stateMachine = new StateMachine<Miner>(this);
        }

        public override void Start()
        {
            stateMachine.ChangeState(MinerIdleState.Instance);
        }

        public override void Update()
        {
            stateMachine.Update();
        }

        public override bool HandleMessage(Telegram telegram)
        {
            return stateMachine.HandleMessage(telegram);
        }
    }
}
