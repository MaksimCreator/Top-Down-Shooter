using UnityEngine;

public class EnemySimulation : Simulated<Enemy>
{
    private readonly SpawnerEnemy _spawner;

    public EnemySimulation(EnemyViewFactory factory, Wallet walletPlayer,AllSoldier allSoldier,Camera camera,Transform defoltPointSpawn,Transform centerOfTheCircle,Player player,float cooldownUpdateSpawnEnemy,float deltaTimeDelay, float startCooldownSpawnEnemy,float minCooldownSpawn)
    {
        _spawner = new SpawnerEnemy(this, factory,walletPlayer,allSoldier,camera,defoltPointSpawn,
        centerOfTheCircle,player,cooldownUpdateSpawnEnemy,startCooldownSpawnEnemy,deltaTimeDelay,minCooldownSpawn);
    }

    public void Simulated(Enemy enemy)
    {
        Simulated(enemy);
    }

    public override void Update(float delta)
    {
        foreach (var enemy in Entities)
            enemy.Update(delta);
    }

    public void Enable()
    => _spawner.Enable();

    public void Disable()
    => _spawner.Disable();
}
