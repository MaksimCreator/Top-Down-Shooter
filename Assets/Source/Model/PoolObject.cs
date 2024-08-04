using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class PoolObject<T> where T : class
{
    private readonly List<T> _poolModel = new();
    private readonly List<GameObject> _poolGameObject = new();
    private readonly List<Transform> _poolTransform = new();

    protected void AddObject(T model, GameObject prefab)
    {
        prefab.SetActive(false);
        _poolModel.Add(model);
        _poolGameObject.Add(prefab);
        _poolTransform.Add(prefab.transform);
    }

    protected T Enable(Vector3 position)
    {
        for (int i = 0; i < _poolTransform.Count; i++)
        {
            if (_poolGameObject[i].activeSelf == false)
            {
                _poolTransform[i].position = position;
                _poolGameObject[i].SetActive(true);
                return _poolModel[i];
            }
        }

        (T,GameObject) pair = Instantiat();
        AddObject(pair.Item1, pair.Item2);

        _poolTransform[_poolTransform.Count - 1].position = position;
        _poolGameObject[_poolGameObject.Count - 1].SetActive(true);
        return _poolModel[_poolGameObject.Count - 1];
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

    protected abstract (T,GameObject) Instantiat(); 
}