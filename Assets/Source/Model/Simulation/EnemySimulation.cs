using System.Collections.Generic;
using UnityEngine;

public class EnemySimulation : Simulated<Enemy>
{
    private readonly SpawnerEnemy _spawner;

    public bool CanSimulated => isActive;

    public EnemySimulation(EnemyViewFactory factory, Wallet walletPlayer,AllSoldier allSoldier,Camera camera,MapBounds mapBounds,Player player,float cooldownUpdateSpawnEnemy,float deltaTimeDelay, float startCooldownSpawnEnemy,float minCooldownSpawn)
    {
        _spawner = new SpawnerEnemy(this, factory,walletPlayer,allSoldier,camera,
        mapBounds,player,cooldownUpdateSpawnEnemy,startCooldownSpawnEnemy,deltaTimeDelay,minCooldownSpawn);
        OnDistroy += Destroy;
    }

    public void Simulate(EnemyManagerSimulated enemy)
    => TryAddEntity(enemy);

    public void Enable()
    => _spawner.Enable();

    public void Disable()
    => _spawner.Disable();

    protected override void onUpdate(float delta)
    {
        foreach (var enemy in Entitys)
            enemy.Update(delta);
    }

    public override void StartSimulate()
    {
        base.StartSimulate();
        _spawner.StartTimer();
    }

    public override void StopSimulate()
    {
        base.StopSimulate();
        _spawner.StopTimer();
    }

    private void Destroy(IEnumerable<Enemy> enemys)
    {
        Disable();
        StopSimulate();
        _spawner.Destroy(enemys);
        OnDistroy -= Destroy;
    }
}
