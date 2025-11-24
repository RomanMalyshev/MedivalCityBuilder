using Unity.Entities;
using Unity.Physics;
using UnityEngine;

namespace Units
{
    public class UnitSelector : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _unitLayer;

        private EntityManager _entityManager;
        private Entity? _selectedEntity;
        
        private void Start()
        {
            _entityManager =   World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var entityQuery = _entityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                var collisionWorld = entityQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;

                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastInput raycastInput = new RaycastInput()
                {
                    Start = ray.GetPoint(0f),
                    End = ray.GetPoint(1000f),
                    Filter = new CollisionFilter()
                    {
                        BelongsTo = ~0u,
                        CollidesWith = (uint)_unitLayer.value,
                        GroupIndex = 0
                    }
                };
                
                DeselectUnit();

                if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit))
                {
                    if (_entityManager.HasComponent<Selected>(hit.Entity))
                    {
                        _selectedEntity = hit.Entity;
                        _entityManager.SetComponentEnabled<Selected>(hit.Entity, true);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeselectUnit();
            }
        }

        private void DeselectUnit()
        {
            if (_selectedEntity == null) return;
            _entityManager.SetComponentEnabled<Selected>(_selectedEntity.Value, false);
        }
    }
}