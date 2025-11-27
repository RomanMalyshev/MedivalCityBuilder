using Unity.Entities;
using UnityEngine;

namespace Units.ECS
{
    public class HungerAuthoring : MonoBehaviour
    {
        public GameObject VisualHunger;
        
        public class HungerAuthoringBaker : Baker<HungerAuthoring>
        {
            public override void Bake(HungerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Hunger
                {
                    VisualHunger = GetEntity(authoring.VisualHunger, TransformUsageFlags.Dynamic),
                    LastTimeEaten = 0f,
                    StartHungerTime = 3f
                });
                SetComponentEnabled<Hunger>(entity,false);
            }
        }
    }

    public struct Hunger : IComponentData, IEnableableComponent
    {
        public Entity VisualHunger;
        public float LastTimeEaten;
        public float StartHungerTime;
    }
}


