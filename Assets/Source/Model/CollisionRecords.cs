using static PhysicsRouter;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CollisionRecords
{
    private readonly BulletSimulation _bulletSimulation;
    private readonly Inventary _playerInventary;
    private readonly SpawnerWeapon _spawner;

    public CollisionRecords(BulletSimulation bulletSimulation, Inventary playerInventary,SpawnerWeapon spawner)
    {
        _bulletSimulation = bulletSimulation;
        _playerInventary = playerInventary;
        _spawner = spawner;
    }

    public IEnumerable<Record> Values()
    {
        yield return new Record<Bullet, Obstacle>((bullet, obstacle) => _bulletSimulation.Stop(bullet));

        yield return new Record<Bullet, Enemy>((bullet, enemy) => _bulletSimulation.Stop(bullet));

        yield return new Record<DefoltBullet, Enemy>((bullet, health) => health.TakeDamage(bullet.Damage));

        yield return new Record<Enemy, Player>((enemy, player) => player.Death());

        yield return new Record<Gain, Player>((gain, player) => gain.Active());

        yield return new Record<Weapon, Player>((weapon, player) => 
        {
            _spawner.Distroy(weapon);
            _spawner.Creat(weapon,true);
            _playerInventary.BindWeapon(weapon, _bulletSimulation.Simulate);
        });

        yield return new Record<Zone, Player>((zone, player) => zone.Active(player));
    }
}
public class BulletSimulation : Simulation<Bullet>
{
    private SpawnerBullet _spawner;

    public void Simulate(Bullet bullet,Vector3 startPosition,Vector3 ditection)
    {

    }

    public void Stop(Bullet bullet)
    {
        foreach (var entity in Entites)
        {
            if (entity.Equals(bullet))
            {
                Stop(entity);
                return;
            }
        }

        throw new InvalidOperationException();
    }

    public override void Update(float delta)
    {

    }

    protected override void OnStoped(PlacedEntity placedEntity)
    {
        if (placedEntity.Entity is ExplosiveBullet bullet)
            bullet.OnEnd(placedEntity.Transform.position);
    }
}
public abstract class Simulation<T>
{
    private List<PlacedEntity> _entities = new List<PlacedEntity>();

    public IEnumerable<PlacedEntity> Entites => _entities;

    public event Action<T, Transform> Start;
    public event Action<T> End;

    public abstract void Update(float delta);

    protected void Simulate(PlacedEntity placedEntity)
    {
        _entities.Add(placedEntity);
        Start?.Invoke(placedEntity.Entity,placedEntity.Transform);
    }

    protected void Stop(PlacedEntity placedEntity)
    {
        _entities.Remove(placedEntity);
        End?.Invoke(placedEntity.Entity);
        OnStoped(placedEntity);
    }

    protected virtual void OnStoped(PlacedEntity placedEntity) { }

    public class PlacedEntity
    {
        public readonly T Entity;
        public readonly Transform Transform;

        public PlacedEntity(T entity, Transform transform)
        {
            Entity = entity;
            Transform = transform;
        }
    }
}
public class SpawnerBullet : PoolObject<Bullet>
{
    private readonly BulletViewFactory _factory;

    protected override (Bullet, GameObject) Instantiat()
    {

    }
}