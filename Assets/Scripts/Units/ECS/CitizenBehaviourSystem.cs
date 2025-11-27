using Unity.Entities;

namespace Units.ECS
{
    partial struct CitizenBehaviourSystem: ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (var citizenActivity in SystemAPI.Query<RefRW<CitizenActivityTypeComponent>>())
            {
                if (citizenActivity.ValueRO.CitizenActivity == CitizenActivityType.None)
                {
                    citizenActivity.ValueRW.CitizenActivity = CitizenActivityType.ProducingFood;
                }
            }
        }
    }
}