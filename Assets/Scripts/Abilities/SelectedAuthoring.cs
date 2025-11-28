using Unity.Entities;
using UnityEngine;

namespace Abilities
{
    public class SelectedAuthoring : MonoBehaviour
    {
        public GameObject VisualSelector;
        public float EnableScale;
        public float ScaleSpeed;
        
        public class BakerComponent : Baker<SelectedAuthoring>
        {
            public override void Bake(SelectedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Renderable);
                AddComponent(entity, new Selected
                {
                    VisualSelector = GetEntity(authoring.VisualSelector.gameObject, TransformUsageFlags.Dynamic) ,
                    EnableScale =  authoring.EnableScale,
                    ScaleSpeed = authoring.ScaleSpeed
                });
                SetComponentEnabled<Selected>(entity,false);
            }
        }
    }

    public struct Selected : IComponentData, IEnableableComponent
    {
        public Entity VisualSelector;
        public float EnableScale;
        public float ScaleSpeed;
    }
}