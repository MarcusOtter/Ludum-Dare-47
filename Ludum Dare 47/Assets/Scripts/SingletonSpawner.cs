using UnityEngine;

public class SingletonSpawner : MonoBehaviour
{
    [SerializeField] private GameManager _gameManagerPrefab;
    [SerializeField] private AudioPlayerSpawner _audioPlayerSpawnerPrefab;
    [SerializeField] private AudioSettings _audioSettingsPrefab;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            var gameObjectToSpawn = Instantiate(_gameManagerPrefab);
        }

        if (AudioPlayerSpawner.Instance == null)
        {
            var gameObjectToSpawn = Instantiate(_audioPlayerSpawnerPrefab);
        }

        if (AudioSettings.Instance == null)
        {
            var gameObjectToSpawn = Instantiate(_audioPlayerSpawnerPrefab);
        }

        // Self-destruct after spawning the missing singletons
        Destroy(gameObject);
    }
}
