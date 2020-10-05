using UnityEngine;

public class SingletonSpawner : MonoBehaviour
{
    [Header("Audio prefabs")]
    [SerializeField] private AudioPlayerSpawner _audioPlayerSpawnerPrefab;
    [SerializeField] private AudioSettings _audioSettingsPrefab;
    [SerializeField] private AudioPlayerPool _audioPlayerPoolPrefab;

    [Header("Other prefabs")]
    [SerializeField] private GameManager _gameManagerPrefab;

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(_gameManagerPrefab);
        }

        if (AudioPlayerSpawner.Instance == null)
        {
            Instantiate(_audioPlayerSpawnerPrefab);
        }

        if (AudioSettings.Instance == null)
        {
            Instantiate(_audioSettingsPrefab);
        }

        if (AudioPlayerPool.Instance == null)
        {
            Instantiate(_audioPlayerPoolPrefab);
        }

        // Self-destruct after spawning the missing singletons
        Destroy(gameObject);
    }
}
