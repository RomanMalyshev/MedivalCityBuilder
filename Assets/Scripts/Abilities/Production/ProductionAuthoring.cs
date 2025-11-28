using System.Collections.Generic;
using Core;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

// ItemAmount is in Abilities namespace (parent)

namespace Abilities.Production
{
    public class ProductionAuthoring : MonoBehaviour
    {
        [FormerlySerializedAs("ProducedItemType")] public Item ProducedItem;
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
                        Item = authoring.ItemRecipe[i].Item,
                        Amount = authoring.ItemRecipe[i].Amount
                    });
                }

                AddComponent(entity, new Production
                {
                    ProducedItem = authoring.ProducedItem,
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
        public Item ProducedItem;
        public int ProducedAmountPerTick;
        public float TickTime;
        public float TimeToProduce;
        public int WorkPlaces;
        public int OccupiedPlaces;
    }

    [InternalBufferCapacity(4)] //Max ingredients per recipe
    public struct ProductionItemRecipe : IBufferElementData
    {
        public Item Item;
        public int Amount;
    }
}