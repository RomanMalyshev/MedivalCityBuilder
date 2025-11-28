using Core;
using Unity.Entities;
using UnityEngine;

namespace Actors.Citizen
{
    public class CitizenBehaviourAuthoring:MonoBehaviour
    {
        public CitizenActivity InitialCitizenActivity = CitizenActivity.None;
        
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
        public CitizenActivity CitizenActivity;
    }
}