using Unity.Entities;

namespace GameWorld
{
    public struct DayNightComponent: IComponentData
    {
        public float TimeOfDay;
    }
    
    partial struct DayNightSystem:ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.EntityManager.CreateSingleton<DayNightComponent>();
        }
    }
}