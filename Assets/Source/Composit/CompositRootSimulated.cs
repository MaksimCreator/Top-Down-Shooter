using UnityEngine;
using System.Collections.Generic;

public class CompositRootSimulated : CompositRoot
{
    [SerializeField] private BulletViewFactory _bulletViewFactory;
    [SerializeField] private EnemyViewFactory _enemyViewFactory;

    private Queue<IUpdateble> _simulated = new();

    public BulletSimulation BulletSimulation { get; private set; }

    public override void Compos()
    {
        _simulated.Enqueue(BulletSimulation = new BulletSimulation(new SpawnerBullet(_bulletViewFactory)));
        _simulated.Enqueue(new EnemySimulation());
    }

    private void Update()
    {
        foreach (var simulated in _simulated)
            simulated.Update(Time.deltaTime);
    }
}
