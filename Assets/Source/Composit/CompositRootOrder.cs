using System.Collections.Generic;
using UnityEngine;

public class CompositRootOrder : MonoBehaviour
{
    [SerializeField] private List<CompositRoot> _compositOrder;

    private void Awake()
    {
        foreach (var compositRoot in _compositOrder)
        {
            compositRoot.Compos();
            compositRoot.enabled = true;
        }
    }
}