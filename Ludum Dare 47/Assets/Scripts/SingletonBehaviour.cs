using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    internal static T Instance { get; private set; }

    protected void AssertSingleton(T instance)
    {
        if (Instance == null)
        {
            Instance = instance;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }
}

