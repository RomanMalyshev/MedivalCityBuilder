namespace Units
{
    public enum CitizenActivityType : byte
    {
        None,
        Move,
        Eat,
        ProducingFood,
        ProducingWood,
        Rest,
    }
    
    public enum BuildingActivityType : byte
    {
        None,
        Work,
        Waiting,
        Paused,
    }
}