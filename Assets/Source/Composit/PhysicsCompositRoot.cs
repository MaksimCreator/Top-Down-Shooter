using System.Collections;
using UnityEngine;

public class PhysicsCompositRoot : CompositRoot
{
    [SerializeField] private PlayerCompositRoot _playerCompositRoot;

    public PhysicsRouter Router { get; private set; }

    public override void Init()
    { 
        CollisionRecords records = new CollisionRecords(_playerCompositRoot.Presenter,_playerCompositRoot.Inevntary);
        Router = new PhysicsRouter(records.Values);

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
