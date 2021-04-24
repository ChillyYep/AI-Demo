using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Demo1
{
    public class StateMachine<T> where T : BaseEntity
    {
        public T entity { get; protected set; }
        public State<T> currentState { get; protected set; }
        public State<T> previousState { get; protected set; }
        public State<T> globalState { get; protected set; }//暂定全局状态优先级高于其他状态
        public StateMachine(T entity)
        {
            this.entity = entity;
            currentState = NoneState<T>.GlobalUniqueNoneState;
            previousState = NoneState<T>.GlobalUniqueNoneState;
            globalState = NoneState<T>.GlobalUniqueNoneState;
        }
        public void Update()
        {
            //if (m_GlobalState.priority > m_CurrentState.priority)
            //{
            if (globalState.blocked)
            {
                globalState.Execute(entity);
                currentState.Pause(entity);
            }
            else
            {
                globalState.Execute(entity);
                currentState.Execute(entity);
            }
            if (currentState.stateType == StateType.Die)
            {
                ChangeState(NoneState<T>.GlobalUniqueNoneState);
            }
            //}
        }
        public void SetGlobalState(State<T> newState)
        {
            globalState.Exit(entity);
            globalState = newState;
            globalState.Enter(entity);
        }
        public void ChangeState(State<T> newState)
        {
            previousState = currentState;
            currentState.Exit(entity);
            currentState = newState;
            currentState.Enter(entity);
        }
        /// <summary>
        /// 只能在两个状态间来回切换
        /// </summary>
        public void RevertToPrevious()
        {
            ChangeState(previousState);
        }
        public bool IsInState(State<T> state)
        {
            return state.stateType == currentState.stateType;
        }
        public bool IsInState(StateType stateType)
        {
            return stateType == currentState.stateType;
        }
        public bool HandleMessage(Telegram telegram)
        {
            if (globalState.blocked)
            {
                return globalState.OnMessage(entity, telegram);
            }
            bool hasMessage1 = globalState.OnMessage(entity, telegram);
            bool hasMessage2 = currentState.OnMessage(entity, telegram);
            return hasMessage1 || hasMessage2;
        }
    }
}
