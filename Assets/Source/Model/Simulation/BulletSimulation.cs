using UnityEngine;
using System.Collections.Generic;

public class BulletSimulation : Simulated<Bullet>
{
    private readonly SpawnerBullet _spawner;

    public BulletSimulation(SpawnerBullet spawner)
    {
        _spawner = spawner;
        OnDistroy += Distroy;
    }

    public void Simulate(Bullet bullet, Transform startPosition,Vector3 targetPosition)
    {
        if (IsSimulated() == false)
            return;

        Bullet curentBullet = _spawner.Enable(bullet, startPosition,targetPosition);

        TryAddEntity(curentBullet);

        curentBullet.InitStop(Stop);
    }

    public void Stop(Bullet bullet)
    {
        _spawner.Disable(bullet);
        bullet.OnEnd();
    }

    protected override void onUpdate(float delta)
    {
        foreach(var bullet in Entitys)
            bullet.Update(delta);
    }

    private void Distroy(IEnumerable<Bullet> bullets)
    {
        StopSimulate();
        _spawner.Distroy(bullets);
        OnDistroy -= Distroy;
    }
}
public abstract class EntityManagerSimulated<T> where T : class
{
    public readonly T Entitys;
    protected readonly Fsm _fsm;

    public EntityManagerSimulated(T entitys, Fsm fsm)
    {
        Entitys = entitys;
        _fsm = fsm;
    }
}
public class BulletManagerSimulated : EntityManagerSimulated<Bullet>
{
    public BulletManagerSimulated(Bullet entitys, Fsm fsm) : base(entitys, fsm)
    {
    }
}
public class EnemyManagerSimulated : EntityManagerSimulated<Enemy>
{
    public EnemyManagerSimulated(Enemy entitys, Fsm fsm) : base(entitys, fsm)
    {

    }
}