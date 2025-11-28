using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Abilities.Movement
{
    partial struct ReachTargetSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (unitMover, transform, entity) in SystemAPI.Query<RefRO<UnitMoverComponent>, RefRO<LocalTransform>>().WithEntityAccess())
            {
                var distance = math.distance(unitMover.ValueRO.TargetPosition, transform.ValueRO.Position);
                if (distance < 0.1f)
                {
                    SystemAPI.SetComponentEnabled<UnitMoverComponent>(entity, false);
                }
            }
        }
    }
}