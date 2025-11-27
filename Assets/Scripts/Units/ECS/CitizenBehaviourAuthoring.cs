using Unity.Entities;
using UnityEngine;

namespace Units.ECS
{
    public class CitizenBehaviourAuthoring:MonoBehaviour
    {
        public CitizenActivityType InitialCitizenActivity = CitizenActivityType.None;
        
        public class Baker: Baker<CitizenBehaviourAuthoring> 
        {
            public override void Bake(CitizenBehaviourAuthoring authoring)
            {
              var entity = GetEntity(TransformUsageFlags.Dynamic);
              AddComponent(entity, new CitizenActivityTypeComponent
              {
                  CitizenActivity = authoring.InitialCitizenActivity
              });
            }
        }
    }

    public struct CitizenActivityTypeComponent : IComponentData
    {
        public CitizenActivityType CitizenActivity;
    }
}