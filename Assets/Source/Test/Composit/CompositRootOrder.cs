using System.Collections.Generic;
using UnityEngine;

public class CompositRootOrder : MonoBehaviour
{
    [SerializeField] private CompositRootSimulated _simulated;
    [SerializeField] private CompositRootUI _compositRootUI;
    [SerializeField] private InatelisateLevelCompositRoot _levelCompositRoot;
    [SerializeField] private PhysicsCompositRoot _physicsCompositRoot;

    private Queue<CompositRoot> _order;

    private void Awake()
    {
        _order = new Queue<CompositRoot>();

        _order.Enqueue(_levelCompositRoot);
        _order.Enqueue(_simulated);
        _order.Enqueue(_compositRootUI);
        _order.Enqueue(_physicsCompositRoot);

        foreach (var compositRoot in _order)
        {
            compositRoot.Compos();
            compositRoot.enabled = true;
        }
    }
}