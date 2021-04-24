using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AIDemo.Demo1
{
    public abstract class MovingEntity : BaseEntity
    {
        protected Transform transform;
        private Vector2 m_pos;
        public Vector2 pos
        {
            get => m_pos;
            protected set
            {
                m_pos = value;
                transform.position = new Vector3(m_pos.x, y, m_pos.y);
            }
        }
        protected float y;
        public Vector2 velocity { get; protected set; }
        public float mass { get; protected set; } = 1f;
        public float maxSpeed { get; protected set; } = 10f;//最大速度限制，标量
        public float maxForce { get; protected set; } = 10f;//施加的拉力
        public float maxTurnRate { get; protected set; } = 10f;//旋转速度
        public MovingEntity(Transform transform, string name) : base(name)
        {
            this.transform = transform;
            transform.gameObject.name = name;
            y = transform.position.y;
            m_pos = new Vector2(transform.position.x, transform.position.z);
        }
    }
}
