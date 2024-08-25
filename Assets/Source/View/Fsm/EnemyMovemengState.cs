using UnityEngine;

public class EnemyMovemengState : FsmState
{
    private readonly IEnemyData _enemyData;

    public EnemyMovemengState(IEnemyData enemyData,Fsm fsm) : base(fsm)
    {
        _enemyData = enemyData;
    }

    public override void Update()
    {
        if (_enemyData.isMoveming == false)
            Fsm.SetState<EnemyMovemingIdelState>();

        _enemyData.EnemyTransform.Translate(_enemyData.Direction, Space.World);
        _enemyData.EnemyTransform.LookAt(_enemyData.TargetPosition);
    }
}
public class EnemyMovemingIdelState : FsmState
{
    private readonly IMovemeng _enemyData;

    public EnemyMovemingIdelState(IEnemyData enemyData,Fsm fsm) : base(fsm)
    {
        _enemyData = enemyData;
    }

    public override void Update()
    {
        if (_enemyData.isMoveming)
            Fsm.SetState<EnemyMovemengState>();
    }
}