using Units;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct SelectedVisualSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>())
        {
            //TODO: optimize GetComponentRW
            var selector = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.VisualSelector);
            selector.ValueRW.Scale = math.lerp(selector.ValueRW.Scale, selected.ValueRO.EnableScale,  selected.ValueRO.ScaleSpeed * SystemAPI.Time.DeltaTime);
        }
        
        foreach (RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>().WithDisabled<Selected>())
        {
            //TODO: optimize GetComponentRW
            var selector = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.VisualSelector);
            selector.ValueRW.Scale = math.lerp(selector.ValueRW.Scale, 0f,  selected.ValueRO.ScaleSpeed * SystemAPI.Time.DeltaTime);
        }
    }
}