using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : SingletonBehaviour<GameManager>
{
    public event Action OnLevelStarted;
    public event Action OnLevelCompleted;

    private readonly List<ReplayInputs> _replayInputs = new List<ReplayInputs>();
    private float _startTime;

    [SerializeField] private PlayerMovement[] _bugPrefabs;
    [SerializeField] private Vector3 _defaultSpawnPosition = new Vector3();

    private void OnEnable()
    {
        AssertSingleton(this);
    }

    private void Start()
    {
        //StartLevel();
    }

    public float GetTimeSinceLevelStart()
    {
        return Time.time - _startTime;
    }

    public void EndLevel(ReplayInputs replayInputs)
    {
        _replayInputs.Add(replayInputs);
        OnLevelCompleted?.Invoke();
        StartLevel();
    }

    private void StartLevel()
    {
        _startTime = Time.time;

        var bugToInstantiate = _bugPrefabs[Random.Range(0, _bugPrefabs.Length)];

        var spawnPosition = _replayInputs.Count == 0
            ? _defaultSpawnPosition
            : _replayInputs[_replayInputs.Count - 1].StartPosition.Add(y: -1.5f);

        var spawnRotation = Quaternion.Euler(0, 0, 0);

        var spawnedBug = Instantiate(bugToInstantiate, spawnPosition, spawnRotation);
        OnLevelStarted?.Invoke();
    }
}
