using UnityEngine;
using Model;

public class PlayerPresenter
{
    private readonly BulletSimulation _bulletSimulation;
    private readonly IInventaryService _inventary;

    public float BulletPerSecond => _inventary.Weapon.BulletPerSecond;

    public PlayerPresenter(BulletSimulation bulletSimulation, IInventaryService inventary)
    {
        _bulletSimulation = bulletSimulation;
        _inventary = inventary;
    }

    public void Enable()
    => _inventary.Weapon.onShoot += _bulletSimulation.Simulate;

    public void Disable()
    => _inventary.Weapon.onShoot -= _bulletSimulation.Simulate;

    public void Shoot(Vector3 direction)
    => _inventary.Weapon.Shoot(direction);
}