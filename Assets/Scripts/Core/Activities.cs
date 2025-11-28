namespace Core
{
    public enum CitizenActivity : byte
    {
        None,
        Move,
        Eat,
        ProducingFood,
        ProducingWood,
        Rest,
    }
    
    public enum BuildingActivity : byte
    {
        None,
        Work,
        Waiting,
        Paused,
    }
}