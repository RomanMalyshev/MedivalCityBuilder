using Unity.Burst;
using Unity.Entities;

namespace Units.ECS
{
    partial struct ProductionSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (
                         production,
                         inventory,
                         recipe)
                     in SystemAPI.Query<
                         RefRW<Production>,
                         DynamicBuffer<InventoryItem>,
                         DynamicBuffer<ProductionItemRecipe>>())
            {
                if (recipe.Length > 0)
                {
                    //recipe logic
                }

                production.ValueRW.TimeToProduce += SystemAPI.Time.DeltaTime;

                if (!(production.ValueRO.TimeToProduce >= production.ValueRO.TickTime)) continue;
                
                production.ValueRW.TimeToProduce = 0;
                bool hasResource = false;
                for (var i = 0; i < inventory.Length; i++)
                {
                    ref var item = ref inventory.ElementAt(i);

                    if (item.ItemType != production.ValueRO.ProducedItemType) continue;
                        
                    item.Amount += production.ValueRO.ProducedAmountPerTick;
                    hasResource = true;
                    break;
                }

                if (!hasResource)
                {
                    inventory.Add(new InventoryItem()
                    {
                        ItemType = production.ValueRO.ProducedItemType,
                        Amount = production.ValueRO.ProducedAmountPerTick
                    });
                }
            }
        }
    }
}