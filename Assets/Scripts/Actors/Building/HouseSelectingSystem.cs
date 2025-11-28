using Actors.Citizen;
using Unity.Entities;

namespace Actors.Building
{
    partial struct HouseSelectingSystem:ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            
            foreach (var citizen in SystemAPI.Query<RefRW<CitizenComponent>>())
            {
                if (citizen.ValueRO.House != Entity.Null)continue;

                foreach (var( house,entity) in  SystemAPI.Query<RefRW<House>>().WithEntityAccess())
                {
                    if (house.ValueRO.OccupiedPlaces < house.ValueRO.Capacity)
                    {
                        citizen.ValueRW.House = entity;
                        house.ValueRW.OccupiedPlaces++;
                        break;
                    }
                }
            }
        }
    }
}