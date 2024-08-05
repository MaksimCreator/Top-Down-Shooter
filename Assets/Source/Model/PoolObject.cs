using System.Collections.Generic;
using System;
using UnityEngine;

public class PoolObject<T> where T : class
{
    private readonly List<T> _poolModel = new();
    private readonly List<GameObject> _poolGameObject = new();
    private readonly List<Transform> _poolTransform = new();

    public event Action<T,Transform> onInstantiat;

    public void AddObject(T model, GameObject prefab)
    {
        prefab.SetActive(false);
        _poolModel.Add(model);
        _poolGameObject.Add(prefab);
        _poolTransform.Add(prefab.transform);
    }

    public (T,GameObject) Enable(T prefab,Transform transform)
    {
        for (int i = 0; i < _poolTransform.Count; i++)
        {
            if (_poolGameObject[i].activeSelf == false)
            {
                _poolTransform[i].position = transform.position;
                _poolGameObject[i].SetActive(true);
                return (_poolModel[i], _poolGameObject[i]);
            }
        }

        int Count = _poolGameObject.Count;
        onInstantiat.Invoke(prefab,transform);

        if (Count == _poolModel.Count)
            throw new InvalidOperationException("Новый обект недобавлен в PoolObject");

        _poolTransform[_poolTransform.Count - 1].position = transform.position;
        _poolGameObject[_poolGameObject.Count - 1].SetActive(true);
        return(_poolModel[_poolGameObject.Count - 1], _poolGameObject[_poolGameObject.Count - 1]);
    }

    public void Disable(T model)
    {
        for (int i = 0; i < _poolModel.Count; i++)
        {
            if (_poolModel[i].Equals(model))
            {
                _poolGameObject[i].SetActive(false);
                return;
            }
        }

        throw new InvalidOperationException();
    }
}