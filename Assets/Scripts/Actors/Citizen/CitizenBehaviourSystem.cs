using Core;
using Unity.Entities;

namespace Actors.Citizen
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    partial struct CitizenBehaviourSystem: ISystem
    {

        public void OnUpdate(ref SystemState state)
        {
            foreach (var citizenActivity in SystemAPI.Query<RefRW<CitizenActivityTypeComponent>>())
            {
                if (citizenActivity.ValueRO.CitizenActivity == CitizenActivity.None)
                {
                    citizenActivity.ValueRW.CitizenActivity = CitizenActivity.ProducingFood;
                }
            }
        }
    }
}