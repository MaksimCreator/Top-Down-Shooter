using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsCompositRoot : CompositRoot
{
    [SerializeField] private List<PhysicsEventBroadcaster> _obstacle;
    [SerializeField] private PhysicsEventBroadcaster _player;
    [SerializeField] private PlayerCompositRoot _playerCompositRoot;
    [SerializeField] private CompositRootSimulated _allSimulated;

    public PhysicsRouter Router { get; private set; }

    public override void Compos()
    {
        CollisionRecords records = new CollisionRecords(_allSimulated.BulletSimulation,_playerCompositRoot.Inventary,_playerCompositRoot.SpawnerBonus,_playerCompositRoot.WeaponStartPosition);
        Router = new PhysicsRouter(records.Values);
        
        PhysicsInit();
        StartCoroutine(GetRouterSteper());
    }

    private void PhysicsInit()
    {
        _player.Init(_playerCompositRoot.Player, Router);
        for (int i = 0; i < _obstacle.Count; i++)
            _obstacle[i].Init(new Obstacle(), Router);
    }

    private IEnumerator GetRouterSteper()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            Router.Step();
        }
    }
}
