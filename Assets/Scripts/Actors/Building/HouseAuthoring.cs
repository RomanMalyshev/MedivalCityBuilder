using Unity.Entities;
using UnityEngine;

namespace Actors.Building
{
    public class HouseAuthoring:MonoBehaviour
    {
        public int HouseCapacity = 2;
        
        public class Baker : Baker<HouseAuthoring>
        {
            public override void Bake(HouseAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new House
                {
                    Capacity = authoring.HouseCapacity,
                    OccupiedPlaces = 0
                });
            }
        }
    }

    public struct House : IComponentData
    {
        public int Capacity;
        public int OccupiedPlaces;
    }
}