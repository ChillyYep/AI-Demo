using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIDemo.Demo1
{
    public class InputManager : MonoBehaviour
    {
        static public InputManager Instance { get; private set; }
        private Vector2 hitPoint;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            GetAllVehicle();
        }
        List<Vehicle> allVehicles = new List<Vehicle>();
        private void GetAllVehicle()
        {
            allVehicles.Clear();
            foreach (var id in EntityManager.Instance.GetAllEntityID())
            {
                var entity = EntityManager.Instance.GetEntityByID(id);
                if (entity.EntityObjType == EntityObjType.Vehicle)
                {
                    var vehicle = entity as Vehicle;
                    if (vehicle != null)
                    {
                        allVehicles.Add(vehicle);
                    }
                }
            }
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
                {
                    hitPoint = new Vector2(hitInfo.point.x, hitInfo.point.z);
                }
            }
            foreach (var vehicle in allVehicles)
            {
                //vehicle.steeringBehaviour.AddForce(vehicle.steeringBehaviour.Seek(hitPoint), MyForceMode.InstantForce);
                vehicle.steeringBehaviour.AddForce(vehicle.steeringBehaviour.Arrive(hitPoint, SteeringBehaviour.Deceleration.Normal), MyForceMode.InstantForce);
                vehicle.Update(Time.deltaTime);
                vehicle.steeringBehaviour.AfterApplyForce();
            }
        }
    }
}
