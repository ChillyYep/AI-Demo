using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Demo1
{
    public class GameMain : MonoBehaviour
    {
        const string defaultName = "Enemy_{0}";
        private void Awake()
        {
            Application.targetFrameRate = 30;
        }
        //// Start is called before the first frame update
        //void Start()
        //{
        //    for (int i = 0; i < 10; ++i)
        //    {
        //        var miner = new Miner(string.Format(defaultName, i));
        //        EntityManager.Instance.RegistEntity(miner);
        //        miner.Start();
        //    }
        //}
        //public void RunAll()
        //{
        //    MessageDispatcher.DispatchMessage(2f, 0, EntityManager.Instance.GetAllEntityID(), MsgType.PleaseEscape);
        //}
        //public void IdleAll()
        //{
        //    MessageDispatcher.DispatchMessage(2f, 0, EntityManager.Instance.GetAllEntityID(), MsgType.PleaseStop);
        //}
        //public void DieAll()
        //{
        //    MessageDispatcher.DispatchMessage(2f, 0, EntityManager.Instance.GetAllEntityID(), MsgType.PleaseDie);
        //}
        //// Update is called once per frame
        //void Update()
        //{
        //    EntityManager.Instance.Update();
        //    MessageDispatcher.DispatchDelayMessage();
        //}
    }
}
