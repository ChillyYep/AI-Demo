using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Demo1
{
    public class Vehicle : MovingEntity
    {
        public SteeringBehaviour steeringBehaviour { get; protected set; }
        public Vehicle(SteeringBehaviour steeringBehaviour, string name) : base(steeringBehaviour.transform, name)
        {
            this.steeringBehaviour = steeringBehaviour;
        }
        protected StateType m_StateType = StateType.None;
        public override StateType stateType => m_StateType;

        public override EntityObjType EntityObjType => EntityObjType.Vehicle;

        public override bool HandleMessage(Telegram telegram)
        {
            throw new System.NotImplementedException();
        }

        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
        public void Update(float time_elapsed)
        {
            Vector2 combineForce = steeringBehaviour.Calculate();
            //F=ma
            Vector2 acceleration = combineForce / mass;
            velocity += acceleration * time_elapsed;
            velocity = velocity.normalized * Mathf.Min(maxSpeed, velocity.magnitude);
            pos += velocity * time_elapsed;
            if (velocity.magnitude > 0.00000001f)
            {
                transform.rotation = Quaternion.FromToRotation(transform.forward, new Vector3(velocity.x, 0f, velocity.y)) * transform.rotation;
            }
        }
    }
}
