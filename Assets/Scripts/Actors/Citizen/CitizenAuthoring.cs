using Unity.Entities;
using UnityEngine;

namespace Actors.Citizen
{
    public class CitizenAuthoring : MonoBehaviour
    {

        public class Baker : Baker<CitizenAuthoring>
        {
            public override void Bake(CitizenAuthoring authoring)
            {
               var entity = GetEntity(TransformUsageFlags.None);
               AddComponent(entity, new CitizenComponent());
            }
        }
    }

    public struct CitizenComponent: IComponentData
    {
        public Entity House;
        public Entity WorkPlace;
    }
}