using Core;
using Unity.Entities;
using UnityEngine;

namespace Actors.Building
{
    public class BuildingBehaviourAuthoring : MonoBehaviour
    {
        public BuildingActivity InitialBuildingActivity = BuildingActivity.None;

        public class Baker : Baker<BuildingBehaviourAuthoring>
        {
            public override void Bake(BuildingBehaviourAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BuildingActivityComponent
                {
                    BuildingActivity = authoring.InitialBuildingActivity
                });
            }
        }
    }

    public struct BuildingActivityComponent : IComponentData
    {
        public BuildingActivity BuildingActivity;
    }
}