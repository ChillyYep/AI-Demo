using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AIDemo.Demo1
{
    public enum MsgType
    {
        PleaseEscape,
        PleaseDie,
        PleaseStop
    }
    public class Telegram
    {
        public uint sender;
        public uint[] recievers;
        public MsgType msg;
        public float dispatchTime;//执行的时间戳
        public object extraInfo;
        public Telegram(uint sender, uint[] recievers, MsgType msg, float dispatchTime, object extraInfo = null)
        {
            this.sender = sender;
            this.recievers = recievers;
            this.msg = msg;
            this.dispatchTime = dispatchTime;
            this.extraInfo = extraInfo;
        }
    }
    public static class MessageDispatcher
    {
        static MessageDispatcher() { }
        private static List<Telegram> delayQueue = new List<Telegram>();
        private static void Discharge(BaseEntity reciever, Telegram msg)
        {
            reciever.HandleMessage(msg);
        }
        public static void DispatchMessage(float delayTime, uint sender, uint[] recievers, MsgType msg, object extraInfo = null)
        {
            float curTime = Time.realtimeSinceStartup;
            var telegram = new Telegram(sender, recievers, msg, curTime, extraInfo);
            if (delayTime <= 0f)
            {
                Send(recievers, telegram);
            }
            else
            {
                telegram.dispatchTime = curTime + delayTime;
                delayQueue.Add(telegram);
            }
        }
        private static void Send(uint[] recievers, Telegram telegram)
        {
            for (int i = 0; i < recievers.Length; ++i)
            {
                var entity = EntityManager.Instance.GetEntityByID(recievers[i]);
                if (entity != null)
                {
                    Discharge(entity, telegram);
                }
            }
        }
        public static void DispatchDelayMessage()
        {
            float curTime = Time.realtimeSinceStartup;
            List<int> expireList = new List<int>();
            for (int i = 0; i < delayQueue.Count; ++i)
            {
                if (delayQueue[i].dispatchTime < curTime && delayQueue[i].dispatchTime > 0)
                {
                    Send(delayQueue[i].recievers, delayQueue[i]);
                    expireList.Add(i);
                }
            }
            for (int i = expireList.Count - 1; i >= 0; --i)
            {
                delayQueue.RemoveAt(expireList[i]);
            }
        }
    }
}