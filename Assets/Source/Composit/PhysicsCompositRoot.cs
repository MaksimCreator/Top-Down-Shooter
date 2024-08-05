using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsCompositRoot : CompositRoot
{
    [SerializeField] private List<PhysicsEventBroadcaster> _obstacle;
    [SerializeField] private PlayerCompositRoot _playerCompositRoot;
    [SerializeField] private CompositRootSimulated _allSimulated;

    public PhysicsRouter Router { get; private set; }

    public override void Compos()
    {
        CollisionRecords records = new CollisionRecords(_allSimulated.BulletSimulation,_playerCompositRoot.Inventary,);
        Router = new PhysicsRouter(records.Values);

        for (int i = 0; i < _obstacle.Count; i++)
            _obstacle[i].Init(new Obstacle(),Router);

        StartCoroutine(GetRouterSteper());
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
