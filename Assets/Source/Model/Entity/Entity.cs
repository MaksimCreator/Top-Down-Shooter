public abstract class Entity : ISpeed
{
    public float Speed { get; protected set; }

    public Entity(float speed)
    {
        Speed = speed;
    }
}