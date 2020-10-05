using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private CustomAudioClip _playerSpawnPlopSound;
    [SerializeField] private PlayerMovement[] _playerBugPrefabs;
    [SerializeField] private Transform[] _allSpawnPoints;

    private List<Transform> _availableSpawnPoints = new List<Transform>();

    private void Awake()
    {
        if (_allSpawnPoints.Length == 0)
        {
            Debug.LogError("You need to populate the Transform[] _allSpawnPoints");
            return;
        }

        _availableSpawnPoints = _allSpawnPoints.ToList();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnPrepareNewLevel += SpawnNewPlayer;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPrepareNewLevel -= SpawnNewPlayer;
    }

    private void SpawnNewPlayer()
    {
        var bugType = GameManager.Instance.GetNextBugTypeToSpawn();
        var bugToInstantiate = GetPlayerPrefabForBugType(bugType);
        var spawnPoint = GetAvailableSpawnPoint();

        var newPlayer = Instantiate(bugToInstantiate, spawnPoint.position, spawnPoint.rotation);
        newPlayer.SetSpawnPoint(spawnPoint);

        AudioPlayerSpawner.Instance.PlaySoundEffect(_playerSpawnPlopSound);
    }

    private Transform GetAvailableSpawnPoint()
    {
        // Hope we never run out of spawn points! :)
        var spawnPoint = _availableSpawnPoints[0];
        _availableSpawnPoints.RemoveAt(0);
        return spawnPoint;
    }

    private PlayerMovement GetPlayerPrefabForBugType(BugType bugType)
    {
        switch (bugType)
        {
            case BugType.Random: return _playerBugPrefabs[Random.Range(0, _playerBugPrefabs.Length)];
            default:             return _playerBugPrefabs.FirstOrDefault(x => x.GetBugType() == bugType);
        }
    }
}

