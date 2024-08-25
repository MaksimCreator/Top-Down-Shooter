using UnityEngine;
using System.Collections.Generic;

public class BulletSimulation : Simulated<IBulletData,Bullet>
{
    private readonly SpawnerBullet _spawner;

    public IBulletData CurentBullet { get; private set; }

    public BulletSimulation(SpawnerBullet spawner)
    {
        _spawner = spawner;
        OnDistroy += Distroy;
    }

    public void Simulate(Bullet bullet, Transform startPosition,Vector3 targetPosition)
    {
        if (CanSimulated())
            return;

        Bullet curentBullet = _spawner.Enable(bullet, startPosition,targetPosition);
        CurentBullet = curentBullet;
        TryAddEntity(CurentBullet,curentBullet);
    }

    public void Stop(Bullet bullet)
    {
        _spawner.Disable(bullet);
        bullet.OnEnd();
    }

    public void Stop(IBulletData bulletIntarface)
    {
        Bullet bullet = GetEntity(bulletIntarface);
        Stop(bullet);
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