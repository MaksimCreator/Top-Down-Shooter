using Model;
using System.Collections.Generic;
using UnityEngine;
using static PhysicsRouter;

public class CollisionRecords
{
    private readonly GainViewFactory _gainViewFactory;
    private readonly WeaponViewFactory _weaponViewFactory;
    private readonly BulletSimulation _bulletSimulation;
    private readonly Transform _transformWeapon;
    private readonly Inventary _playerInventary;
    private readonly SpawnerBonus _spawner;

    public CollisionRecords(GainViewFactory gainViewFactory,WeaponViewFactory weaponViewFactory,BulletSimulation bulletSimulation, Inventary playerInventary,SpawnerBonus spawner,Transform transformWeapon)
    {
        _weaponViewFactory = weaponViewFactory;
        _gainViewFactory = gainViewFactory;
        _bulletSimulation = bulletSimulation;
        _playerInventary = playerInventary;
        _transformWeapon = transformWeapon;
        _spawner = spawner;
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
            _gainViewFactory.Destroy(gain);
            gain.Active(player);
        });

        yield return new Record<Weapon, Player>((weapon, player) => 
        {
            _weaponViewFactory.Destroy(weapon);
            _weaponViewFactory.Destroy(_playerInventary.Weapon);
            _weaponViewFactory.Creat(weapon, _transformWeapon.position, _transformWeapon.rotation, parent: _transformWeapon, IsPhysics: false);
            _playerInventary.BindWeapon(weapon);
        });
    }
}