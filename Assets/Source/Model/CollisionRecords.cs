using System.Collections.Generic;
using UnityEngine;
using static PhysicsRouter;

public class CollisionRecords
{
    private readonly BulletSimulation _bulletSimulation;
    private readonly Transform _transfomWeapon;
    private readonly Inventary _playerInventary;
    private readonly SpawnerBonus _spawner;

    public CollisionRecords(BulletSimulation bulletSimulation, Inventary playerInventary,SpawnerBonus spawner,Transform transformWeapon)
    {
        _bulletSimulation = bulletSimulation;
        _playerInventary = playerInventary;
        _spawner = spawner;
        _transfomWeapon = transformWeapon;
    }

    public IEnumerable<Record> Values()
    {
        yield return new Record<Bullet, Obstacle>((bullet, obstacle) => _bulletSimulation.Stop(bullet));

        yield return new Record<Bullet, Enemy>((bullet, enemy) => _bulletSimulation.Stop(bullet));

        yield return new Record<DefoltBullet, Enemy>((bullet, health) => health.TakeDamage(bullet.Damage));

        yield return new Record<Enemy, Player>((enemy, player) => player.TryDeath());

        yield return new Record<Zone, Player>((zone, player) => zone.Active(player));

        yield return new Record<Gain, Player>((gain, player) => 
        {
            _spawner.Destroy(gain);
            gain.Active(player);
        });

        yield return new Record<Weapon, Player>((weapon, player) => 
        {
            _spawner.Destroy(weapon);
            _spawner.Destroy(_playerInventary.Weapon);
            _spawner.Creat(weapon,_transfomWeapon);
            _playerInventary.BindWeapon(weapon, _bulletSimulation.Simulate);
        });
    }
}