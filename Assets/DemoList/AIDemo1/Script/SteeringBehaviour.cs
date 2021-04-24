using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Demo1
{
    public enum MyForceMode
    {
        ContinuousForce,
        InstantForce
    }
    public class SteeringBehaviour : MonoBehaviour
    {
        Vector2 instanceCombineForce;
        Vector2 continuousCombineForce;
        Vehicle vehicle;
        private void Awake()
        {
            vehicle = new Vehicle(this, name);
            EntityManager.Instance.RegistEntity(vehicle);
        }
        public void AddForce(Vector2 force, MyForceMode myForceMode)
        {
            switch (myForceMode)
            {
                case MyForceMode.InstantForce:
                    instanceCombineForce += force;
                    break;
                case MyForceMode.ContinuousForce:
                    continuousCombineForce += force;
                    break;
            }
        }
        public void AfterApplyForce()
        {
            instanceCombineForce = Vector2.zero;
        }
        /// <summary>
        /// 计算合力
        /// </summary>
        /// <returns></returns>
        public Vector2 Calculate()
        {
            return instanceCombineForce + continuousCombineForce;
        }
        #region 操纵行为
        /// <summary>
        /// 向目标位置移动，不会停止
        /// </summary>
        public Vector2 Seek(Vector2 targetPos)
        {
            Vector2 toTarget = targetPos - vehicle.pos;
            Vector2 desiredVelocity = toTarget.normalized * vehicle.maxSpeed;
            return desiredVelocity - vehicle.velocity;
        }
        public Vector2 Flee(Vector2 targetPos)
        {
            //恐惧范围，使角色往反方向移动
            const float PanicDistanceSq = 100f * 100f;
            if (Mathf.Pow(2f, Vector2.Distance(targetPos, vehicle.pos)) > PanicDistanceSq)
            {
                return Vector2.zero;
            }

            Vector2 desiredVelocity = (vehicle.pos - targetPos).normalized * vehicle.maxSpeed;
            return desiredVelocity - vehicle.velocity;
        }
        public enum Deceleration
        {
            Fast = 1,
            Normal,
            Slow
        }
        /// <summary>
        /// 向目标位置移动，在即将抵达目标位置时减速
        /// </summary>
        public Vector2 Arrive(Vector2 targetPos, Deceleration deceleration)
        {
            Vector2 toTarget = targetPos - vehicle.pos;
            float dist = toTarget.magnitude;
            if (dist > 0f)
            {
                const float DecelerationTweaker = 0.3f;
                float speed = dist / ((float)deceleration * DecelerationTweaker);
                speed = Mathf.Min(speed, vehicle.maxSpeed);
                Vector2 desiredVelocity = toTarget * speed / dist;
                return desiredVelocity - vehicle.velocity;
            }
            return Vector2.zero;
        }
        #endregion
    }
}
