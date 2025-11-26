using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Units.ECS
{
    
    [UpdateBefore(typeof(TransformSystemGroup))]
    partial struct UnitMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            UnitMoverJob job = new UnitMoverJob { DeltaTime = SystemAPI.Time.DeltaTime };
            job.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct UnitMoverJob : IJobEntity
    {
        
        public float DeltaTime;
    
        private void Execute(ref LocalTransform localTransform,in UnitMoverComponent unitMover)
        {
            float3 direction = unitMover.TargetPosition - localTransform.Position;
            direction = math.normalize(direction);
            localTransform.Position = math.lerp(localTransform.Position, unitMover.TargetPosition, unitMover.MoveSpeed * DeltaTime);
            localTransform.Rotation = math.slerp(localTransform.Rotation, quaternion.LookRotation(direction, math.up()), unitMover.RotationSpeed * DeltaTime);

            float3 target = unitMover.TargetPosition;
            float maxDistance = unitMover.MoveSpeed * DeltaTime;
        
            float3 newPosition = localTransform.Position + math.normalize(target - localTransform.Position) * maxDistance;
            localTransform.Position = newPosition;
        }
    }
}