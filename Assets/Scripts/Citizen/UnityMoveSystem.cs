using Citizen;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct UnityMoveSystem : ISystem
{
   
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        UnitMoverJon job = new UnitMoverJon { DeltaTime = SystemAPI.Time.DeltaTime };
        job.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct UnitMoverJon : IJobEntity
{
    public float DeltaTime;
    
    public void Execute(ref LocalTransform localTransform,in UnitMoverComponent unitMover)
    {
        float3 direction = unitMover.TargetPosition - localTransform.Position;
        direction = math.normalize(direction);
        localTransform.Position = math.lerp(localTransform.Position, unitMover.TargetPosition - unitMover.OffsetPosition, unitMover.MoveSpeed * DeltaTime);
        localTransform.Rotation = math.slerp(localTransform.Rotation, quaternion.LookRotation(direction, math.up()), unitMover.RotationSpeed * DeltaTime);
    }
}