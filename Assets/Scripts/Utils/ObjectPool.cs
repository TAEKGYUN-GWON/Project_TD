using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : ObjectBase
{

    private Transform _owner;

    private T _prefab;
    public T Prefab
    {
        get { return _prefab; }
    }

    public int PoolCount { get { return _spawnedPool.Count + _despawnedQueue.Count; } }

    protected List<T> _spawnedPool = new List<T>();
    protected Queue<T> _despawnedQueue = new Queue<T>();

    private int _defaultPoolSize;
    private bool _increasementable = false;
    private int _increasementSize = 0;


    public void InitPool(Transform owner, T prefab, int defaultPoolSize, bool increasementable, int increasementSize)
    {
        _owner = owner;
        _prefab = prefab;
        _defaultPoolSize = defaultPoolSize;
        _increasementable = increasementable;
        _increasementSize = increasementSize;

        IncreasePoolInstance(_defaultPoolSize);
    }

    public virtual T Spawn()
    {
        // 1. 디스폰 풀이 비어있다면//
        if (0 >= _despawnedQueue.Count)
        {
            // 2. 풀이 증가로 설정되어 있다면 //
            if (true == _increasementable)
            {
                IncreasePoolInstance(_increasementSize);
            }
            else
            {
                Debug.LogErrorFormat("[ERROR] 증가 할 수 없는 Pool 입니다. : {0}", _owner.name);
                return null;
            }
        }

        T ret = _despawnedQueue.Dequeue();

        // 마지막 원소로 집어넣는다.//
        _spawnedPool.Add(ret);
        ret.gameObject.SetActive(true);

        return ret;
    }

    public void IncreasePoolInstance(int count)
    {
        T instance = null;
        for (int i = 0; i < count; ++i)
        {
            instance = GameObject.Instantiate<T>(Prefab);
            Despawn(instance);
        }
    }

    public void Despawn(T instance)
    {
        if (_spawnedPool.Contains(instance))
        {
            _spawnedPool.Remove(instance);
        }

        _AddToDespawnQueue(instance);
    }

    private void _AddToDespawnQueue(T instance)
    {
        if (instance == null)
            return;

        if (instance.gameObject == null)
            return;

        if (instance.transform.parent != _owner)
        {
            instance.transform.SetParent(_owner);

            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;
        }

        instance.gameObject.SetActive(false);

        if (_despawnedQueue.Contains(instance) == false)
        {
            _despawnedQueue.Enqueue(instance);
            instance.OnDespawn();
        }
        else
        {
            Debug.LogErrorFormat(
                "[ERROR] Fail AddToDespawnQueue instance : {0} _despawnedQueue : {1}"
                , instance.name
                , _despawnedQueue.Count
            );
        }
    }

    public void DespawnAll(Action<T> eachAction = null)
    {
        foreach (var instance in _spawnedPool)
        {
            if (eachAction != null)
                eachAction(instance);

            _AddToDespawnQueue(instance);
        }

        _spawnedPool.Clear();
    }

    public void RemovePool()
    {
        DespawnAll();

        T despawnedObj = null;
        while (_despawnedQueue.Count > 0)
        {
            despawnedObj = _despawnedQueue.Dequeue();
            if (null != despawnedObj)
            {
                GameObject.DestroyImmediate(despawnedObj.gameObject);
            }
        }
    }

}
