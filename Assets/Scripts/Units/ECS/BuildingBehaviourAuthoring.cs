using Unity.Entities;
using UnityEngine;

namespace Units.ECS
{
    public class BuildingBehaviourAuthoring : MonoBehaviour
    {
        public BuildingActivityType InitialBuildingActivity = BuildingActivityType.None;

        public class Baker : Baker<BuildingBehaviourAuthoring>
        {
            public override void Bake(BuildingBehaviourAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BuildingActivityComponent
                {
                    BuildingActivityType = authoring.InitialBuildingActivity
                });
            }
        }
    }

    public struct BuildingActivityComponent : IComponentData
    {
        public BuildingActivityType BuildingActivityType;
    }
}