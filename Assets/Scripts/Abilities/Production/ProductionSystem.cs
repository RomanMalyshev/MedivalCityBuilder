using Core;
using Unity.Burst;
using Unity.Entities;

// InventoryItem is in Abilities namespace (parent)

namespace Abilities.Production
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
                            if (inventory[j].Item == recipe[i].Item)
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
                        if (inventory[j].Item != recipe[i].Item) continue;

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

                    if (item.Item != production.ValueRO.ProducedItem) continue;

                    item.Amount += production.ValueRO.ProducedAmountPerTick;
                    hasResource = true;
                    break;
                }

                if (!hasResource)
                {
                    inventory.Add(new InventoryItem()
                    {
                        Item = production.ValueRO.ProducedItem,
                        Amount = production.ValueRO.ProducedAmountPerTick
                    });
                }
            }
        }
    }
}