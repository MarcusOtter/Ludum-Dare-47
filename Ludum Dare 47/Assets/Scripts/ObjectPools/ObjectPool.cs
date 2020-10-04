using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    private readonly Queue<T> _pool;

    [SerializeField] private int _initialSize = 8;
    [SerializeField] private T _prefab;

    protected ObjectPool()
    {
        _pool = new Queue<T>(_initialSize);
    }

    private void OnEnable()
    {
        AddObjectsToPool(_initialSize);
    }

    public T Get()
    {
        if (_pool.Count == 0)
        {
            AddObjectsToPool(_initialSize);
        }

        return _pool.Dequeue();
    }

    public void AddToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }

    private void AddObjectsToPool(int amount)
    {
        for (var i = 0; i < amount; i++)
        {
            var obj = Instantiate(_prefab, transform);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}
