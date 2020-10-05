using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : SingletonBehaviour<ObjectPool<T>> where T : MonoBehaviour
{
    private readonly Queue<T> _pool;

    [SerializeField] private int _initialSize = 8;
    [SerializeField] private T _prefab;

    protected ObjectPool()
    {
        _pool = new Queue<T>(_initialSize);
    }

    private void Awake()
    {
        AssertSingleton(this);
    }

    private void OnEnable()
    {
        AddObjectsToPool(_initialSize);
    }

    public T GetNewAndEnable()
    {
        if (_pool.Count == 0)
        {
            AddObjectsToPool(_initialSize);
        }

        var obj = _pool.Dequeue();

        if (obj == null)
        {
            // Idk editor gives me errors when I stop playing if I don't have this
            return null;
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void ReturnAndDisable(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }

    private void AddObjectsToPool(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            if (transform == null) { return; } // dear lord
            var obj = Instantiate(_prefab, transform);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}
