using System;
using System.Collections.Generic;
using Core;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Abilities
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
                            Item = authoring.Items[i].Item,
                            Amount = authoring.Items[i].Amount
                        });
                    else
                    {
                        inventoryBuffer.Add(new InventoryItem()
                        {
                            Item = Item.None,
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
        [FormerlySerializedAs("ItemType")] public Item Item;
        public int Amount;
    }

    public struct Inventory : IComponentData
    {
        public int InventoryCapacity;
    }

    [InternalBufferCapacity(16)]
    public struct InventoryItem : IBufferElementData
    {
        public Item Item;
        public int Amount;
    }
}