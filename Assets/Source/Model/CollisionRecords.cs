using System.Collections.Generic;
using static PhysicsRouter;

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

        yield return new Record<Gain, Player>((gain, player) => gain.Active(player));

        yield return new Record<Weapon, Player>((weapon, player) => 
        {
            _spawner.Distroy(weapon);
            _spawner.Distroy(_playerInventary.Weapon);
            _spawner.Creat(weapon,true,false);
            _playerInventary.BindWeapon(weapon, _bulletSimulation.Simulate);
        });

        yield return new Record<Zone, Player>((zone, player) => zone.Active(player));
    }
}