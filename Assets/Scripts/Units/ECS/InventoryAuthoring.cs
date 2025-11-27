using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Units.ECS
{
    public class InventoryAuthoring : MonoBehaviour
    {
        public int InventoryCapacity = 10;
        public List<ItemAmount> Items;

        public class Baker : Baker<InventoryAuthoring>
        {
            public override void Bake(InventoryAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                DynamicBuffer<InventoryItem> inventoryBuffer = AddBuffer<InventoryItem>(entity);

                for (int i = 0; i < authoring.InventoryCapacity; i++)
                {
                    if (i < authoring.Items.Count)
                        inventoryBuffer.Add(new InventoryItem()
                        {
                            ItemType = authoring.Items[i].ItemType,
                            Amount = authoring.Items[i].Amount
                        });
                    else
                    {
                        inventoryBuffer.Add(new InventoryItem()
                        {
                            ItemType = ItemType.None,
                            Amount = 0
                        });
                    }
                }

                AddComponent(entity, new Inventory
                {
                    InventoryCapacity = authoring.InventoryCapacity,
                });
            }
        }
    }
    
    [Serializable]
    public class ItemAmount
    {
        public ItemType ItemType;
        public int Amount;
    }

    public struct Inventory : IComponentData
    {
        public int InventoryCapacity;
    }

    [InternalBufferCapacity(16)]
    public struct InventoryItem : IBufferElementData
    {
        public ItemType ItemType;
        public int Amount;
    }
}