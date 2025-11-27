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
                //recipe logic
                bool hasEnoughResources = true;
                if (recipe.Length > 0)
                {
                    for (int i = 0; i < recipe.Length; i++)
                    {
                        hasEnoughResources = false;
                        for (int j = 0; j < inventory.Length; j++)
                        {
                            if (inventory[j].ItemType == recipe[i].ItemType)
                            {
                                if (inventory[j].Amount >= recipe[i].Amount)
                                {
                                    hasEnoughResources = true;
                                }
                                break;
                            }
                        }
                    }
                }

                if (!hasEnoughResources) continue;
                
                production.ValueRW.TimeToProduce += SystemAPI.Time.DeltaTime;

                if (production.ValueRO.TimeToProduce < production.ValueRO.TickTime) continue;

                //Take
                for (int i = 0; i < recipe.Length; i++)
                {
                    for (int j = 0; j < inventory.Length; j++)
                    {
                        if (inventory[j].ItemType != recipe[i].ItemType) continue;

                        ref var item = ref inventory.ElementAt(j);
                        item.Amount -= recipe[i].Amount;
                    }
                }

                //Add
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