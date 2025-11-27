using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Units.ECS
{
    public class ProductionAuthoring : MonoBehaviour
    {
        public ItemType ProducedItemType;
        public List<ItemAmount> ItemRecipe;
        public int WorkPlaces;
        public float TickTime;
        public int ProducedAmountPerTick;

        public class Baker : Baker<ProductionAuthoring>
        {
            public override void Bake(ProductionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);

                DynamicBuffer<ProductionItemRecipe> itemRecipeBuffer = AddBuffer<ProductionItemRecipe>(entity);

                for (int i = 0; i < authoring.ItemRecipe.Count; i++)
                {
                    itemRecipeBuffer.Add(new ProductionItemRecipe()
                    {
                        ItemType = authoring.ItemRecipe[i].ItemType,
                        Amount = authoring.ItemRecipe[i].Amount
                    });
                }

                AddComponent(entity, new Production
                {
                    ProducedItemType = authoring.ProducedItemType,
                    ProducedAmountPerTick = authoring.ProducedAmountPerTick,
                    TickTime = authoring.TickTime,
                    TimeToProduce = 0f,
                    WorkPlaces = authoring.WorkPlaces,
                    OccupiedPlaces = 0
                });
            }
        }
    }

    public struct Production : IComponentData
    {
        public ItemType ProducedItemType;
        public int ProducedAmountPerTick;
        public float TickTime;
        public float TimeToProduce;
        public int WorkPlaces;
        public int OccupiedPlaces;
    }

    [InternalBufferCapacity(4)] //Max ingredients per recipe
    public struct ProductionItemRecipe : IBufferElementData
    {
        public ItemType ItemType;
        public int Amount;
    }
}