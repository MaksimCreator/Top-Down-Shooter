public abstract class Entity : ISpeed
{
    public float Speed { get; protected set; }
    private Fsm _fsm;

    public Entity(float speed)
    {
        Speed = speed;
    }
}
public class FsmStateBulletMoveming : FsmState
{
    public FsmStateBulletMoveming(Fsm fsm) : base(fsm)
    {
    }
}
public class FsmStateEnemyMovemeng
{

}