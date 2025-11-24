using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Units
{
    public class UnitMoverAuthoring : MonoBehaviour
    {
        public float Speed;
        public float RotationSpeed = 10f;

        public class BakerComponent : Baker<UnitMoverAuthoring>
        {
            public override void Bake(UnitMoverAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new UnitMoverComponent
                {
                    MoveSpeed = authoring.Speed,
                    RotationSpeed = authoring.RotationSpeed,
                    OffsetPosition = authoring.transform.position,
                });
            }
        }
    }
    
    public struct UnitMoverComponent : IComponentData
    {
        public float MoveSpeed;
        public float RotationSpeed;
        public float3 TargetPosition;
        public float3 OffsetPosition;
    }
}