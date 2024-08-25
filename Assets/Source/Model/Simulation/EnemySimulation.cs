using Model;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimulation : Simulated<IEnemyData,Enemy>
{
    private readonly SpawnerEnemy _spawner;
    private readonly Action<IEnemyData> _addNewEnemy;

    private Enemy _curentEnemy;

    public EnemySimulation(Action<IEnemyData> AddNewEnemy,ServiceLocator serviceLocator,Wallet walletPlayer,Camera camera,Player player)
    {
        _spawner = new SpawnerEnemy(Simulate,CanSimulated,this,serviceLocator.GetSevice<EnemyViewFactory>(),
            walletPlayer,serviceLocator.GetSevice<AllSoldierConffig>(),camera,serviceLocator.GetSevice<IMapBoundsService>()
            ,player,serviceLocator.GetSevice<EnemySpawnreConffig>());

        _addNewEnemy = AddNewEnemy;
        OnDistroy += Destroy;
    }
    
    public void Enable()
    => _spawner.Enable();

    public void Disable()
    => _spawner.Disable();

    public override void StartSimulate()
    {
        base.StartSimulate();
        _spawner.StartTimer();

        foreach (var enemy in Entitys)
            enemy.isMoveming = true;
    }

    public override void StopSimulate()
    {
        base.StopSimulate();
        _spawner.StopTimer();

        foreach (var enemy in Entitys)
            enemy.isMoveming = false;
    }

    protected override void onUpdate(float delta)
    {
        foreach (var enemy in Entitys)
            enemy.Update(delta);
    }

    private void Simulate(Enemy enemy)
    { 
        TryAddEntity(enemy, enemy);
        _curentEnemy = enemy;
        _addNewEnemy.Invoke(enemy);
    }

    private void Destroy(IEnumerable<Enemy> enemys)
    {
        Disable();
        StopSimulate();
        _spawner.Destroy(enemys);

        foreach (var enemy in enemys)
            enemy.isMoveming = false;

        OnDistroy -= Destroy;
    }
}