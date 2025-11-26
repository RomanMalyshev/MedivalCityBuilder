using Unity.Burst;
using Unity.Entities;

namespace Units.ECS
{
    partial struct HungerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            foreach (var (hunger,entity) in SystemAPI.Query<RefRW<Hunger>>().WithDisabled<Hunger>().WithEntityAccess())
            {
                hunger.ValueRW.LastTimeEaten += SystemAPI.Time.DeltaTime;
                if (hunger.ValueRW.LastTimeEaten >= hunger.ValueRO.StartHungerTime)
                {
                    ecb.SetComponentEnabled<Hunger>(entity, true);
                }
            }

            foreach (var (hunger, inventory, entity) in SystemAPI.Query<RefRW<Hunger>, DynamicBuffer<InventoryItem>>().WithEntityAccess())
            {
                for (int i = 0; i < inventory.Length; i++)
                {
                    var inventoryItems = inventory;
                    var item = inventoryItems[i];
                    if (item.ItemType == ItemType.Grain && item.Amount > 0)
                    {
                        item.Amount--;
                        if (item.Amount == 0)
                        {
                            item.ItemType = ItemType.None;
                        }

                        inventoryItems[i] = item;
                        hunger.ValueRW.LastTimeEaten = 0;
                        ecb.SetComponentEnabled<Hunger>(entity, false);
                        break;
                    }
                }
            }
        }
        
    }
}
