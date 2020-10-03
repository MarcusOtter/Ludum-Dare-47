using UnityEngine;

public class SingletonSpawner : MonoBehaviour
{
    private void Awake()
    {
        if (AudioPlayerSpawner.Instance == null)
        {
            var gameObjectToSpawn = new GameObject(nameof(AudioPlayerSpawner), typeof(AudioPlayerSpawner));
            Instantiate(gameObjectToSpawn);
        }

        if (AudioSettings.Instance == null)
        {
            var gameObjectToSpawn = new GameObject(nameof(AudioSettings), typeof(AudioSettings));
            Instantiate(gameObjectToSpawn);
        }

        // Self-destruct after spawning the missing singletons
        Destroy(gameObject);
    }
}
