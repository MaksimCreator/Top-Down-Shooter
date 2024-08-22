using System;
using UnityEngine;

public interface IViewFactoryService<T> : IService
{
    void Creat(T prefab, Vector3 position, Quaternion rotation, Action<T, GameObject> action = null, Transform parent = null, bool IsPhysics = true);
    void Destroy(T prefab);
}