using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AIDemo.Demo1
{
    //public enum StatePriority
    //{
    //    Lowest,
    //    Low,
    //    Middle,
    //    High,
    //    Highest
    //}
    public enum StateType
    {
        None,
        Idle,
        Die,
        Run
    }
    public abstract class State<T> where T : BaseEntity
    {
        public virtual bool blocked => false;
        public readonly StateType stateType;
        public State(StateType stateType)
        {
            this.stateType = stateType;
        }
        public virtual bool isGlobal => false;
        //public virtual StatePriority priority => StatePriority.Highest;
        public abstract void Enter(T entity);
        public abstract void Execute(T entity);
        public abstract void Pause(T entity);
        public abstract void Exit(T entity);
        public abstract bool OnMessage(T entity, Telegram telegram);
    }
    #region NoneState
    /// <summary>
    /// 空状态
    /// </summary>
    public class NoneState<T> : State<T> where T : BaseEntity
    {
        protected NoneState() : base(StateType.None) { }
        protected static NoneState<T> m_GlobalUniqueNoneState = new NoneState<T>();
        public static NoneState<T> GlobalUniqueNoneState => m_GlobalUniqueNoneState;
        public override void Enter(T entity) { }

        public override void Execute(T entity) { }

        public override void Exit(T entity) { }
        public override void Pause(T entity) { }

        public override bool OnMessage(T entity, Telegram telegram) => true;
    }
    #endregion
    #region IdleState
    /// <summary>
    /// 闲置状态
    /// </summary>
    public class MinerIdleState : State<Miner>
    {
        public static MinerIdleState Instance { get; } = new MinerIdleState();
        protected MinerIdleState() : base(StateType.Idle) { }
        public override void Enter(Miner entity)
        {
            Debug.LogFormat("Entity {0} will Idle!", entity.name);
        }

        public override void Execute(Miner entity)
        {
            Debug.LogFormat("Entity {0} is Idle!", entity.name);
        }

        public override void Exit(Miner entity)
        {
            Debug.LogFormat("Entity {0} Leave IdleState!", entity.name);
        }

        public override void Pause(Miner entity)
        {
            Debug.LogFormat("Entity {0}:Pause!", entity.name);
        }

        public override bool OnMessage(Miner entity, Telegram telegram)
        {
            switch (telegram.msg)
            {
                case MsgType.PleaseEscape:
                    entity.GetFSM().ChangeState(MinerRunState.Instance);
                    break;
                case MsgType.PleaseDie:
                    entity.GetFSM().ChangeState(MinerDieState.Instance);
                    break;
                default:
                    return false;
            }
            return true;
        }
    }
    #endregion

    #region DieState
    public class MinerDieState : State<Miner>
    {
        private MinerDieState() : base(StateType.Die) { }
        public static MinerDieState Instance { get; } = new MinerDieState();
        public override void Enter(Miner entity)
        {
            Debug.LogFormat("Entity {0} will Die!", entity.name);
        }

        public override void Execute(Miner entity)
        {
            Debug.LogFormat("Entity {0} is Dying!", entity.name);
        }

        public override void Exit(Miner entity)
        {
            Debug.LogFormat("Entity {0} has Died!", entity.name);
        }

        public override void Pause(Miner entity)
        {
            Debug.LogFormat("Entity {0}:Pause!", entity.name);
        }

        public override bool OnMessage(Miner entity, Telegram telegram) => true;
    }
    #endregion
    #region RunState
    public class MinerRunState : State<Miner>
    {
        private MinerRunState() : base(StateType.Run) { }
        public static MinerRunState Instance { get; } = new MinerRunState();
        public override void Enter(Miner entity)
        {
            Debug.LogFormat("Entity {0} will Run!", entity.name);
        }

        public override void Execute(Miner entity)
        {
            Debug.LogFormat("Entity {0} is Running!", entity.name);
        }

        public override void Exit(Miner entity)
        {
            Debug.LogFormat("Entity {0} Ends Running!", entity.name);
        }

        public override bool OnMessage(Miner entity, Telegram telegram)
        {
            switch (telegram.msg)
            {
                case MsgType.PleaseStop:
                    entity.GetFSM().ChangeState(MinerIdleState.Instance);
                    break;
                case MsgType.PleaseDie:
                    entity.GetFSM().ChangeState(MinerDieState.Instance);
                    break;
                default:
                    return false;
            }
            return true;
        }

        public override void Pause(Miner entity)
        {
            Debug.LogFormat("Entity {0}:Pause!", entity.name);
        }
    }
    #endregion
}
