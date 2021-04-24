using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AIDemo.Demo1
{
    public class EntityManager
    {
        private EntityManager() { }
        public static EntityManager Instance { get; } = new EntityManager();
        private Dictionary<uint, BaseEntity> entityDict = new Dictionary<uint, BaseEntity>();
        //public List<BaseEntity> GetInfluencedEntity()
        //{
        //    List<BaseEntity> baseEntities = new List<BaseEntity>();
        //    return baseEntities;
        //}
        public T CreateEntity<T>() where T : BaseEntity, new()
        {
            T entity = new T();
            entityDict[entity.ID] = entity;
            return entity;
        }
        public uint[] GetAllEntityID()
        {
            uint[] ids = new uint[entityDict.Count];
            int index = 0;
            foreach (var item in entityDict.Keys)
            {
                ids[index++] = item;
            }
            return ids;
        }
        public void RegistEntity(BaseEntity entity)
        {
            if (entity == null)
            {
                return;
            }
            entityDict[entity.ID] = entity;
        }
        public void Remove(BaseEntity entity)
        {
            if (entity == null)
            {
                return;
            }
            Remove(entity.ID);
        }
        public void Remove(uint entityID)
        {
            if (entityDict.ContainsKey(entityID))
            {
                entityDict.Remove(entityID);
            }
            Debug.Log("Remove:" + entityID.ToString());
        }
        public BaseEntity GetEntityByID(uint entityID)
        {
            return entityDict.TryGetValue(entityID, out BaseEntity entity) ? entity : null;
        }
        public void Update()
        {
            List<uint> expireList = new List<uint>();
            foreach (var item in entityDict.Values)
            {
                if (item.stateType == StateType.None)
                {
                    expireList.Add(item.ID);
                }
                else
                {
                    item.Update();
                }
            }
            for (int i = 0; i < expireList.Count; ++i)
            {
                Remove(expireList[i]);
            }
        }
    }
}
