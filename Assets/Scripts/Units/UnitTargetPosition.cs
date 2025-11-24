using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Units
{
    public class UnitTargetPosition : MonoBehaviour
    {
    
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                var position = GetMousePosition();

                EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
                EntityQuery query = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMoverComponent, Selected>().Build(entityManager);

                NativeArray<Entity> entityArray = query.ToEntityArray(Allocator.Temp);
                NativeArray<UnitMoverComponent> unitMoverArray = query.ToComponentDataArray<UnitMoverComponent>(Allocator.Temp);

                for (var i = 0; i < unitMoverArray.Length; i++)
                {
                    UnitMoverComponent unitMover = unitMoverArray[i];
                    unitMover.TargetPosition = position;
                    entityManager.SetComponentData(entityArray[i], unitMover);
                }
            }
        }
    
        private Vector3 GetMousePosition()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                return hit.point;
            }

            Debug.LogWarning("No hit");
            return Vector3.zero;
        }
    }
}