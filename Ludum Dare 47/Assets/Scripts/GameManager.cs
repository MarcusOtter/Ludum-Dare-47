using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : SingletonBehaviour<GameManager>
{
    public event Action OnLevelStarted;
    public event Action OnLevelCompleted;
    public event Action<int> OnGameOver;


    private readonly List<ReplayInputs> _replayInputs = new List<ReplayInputs>();
    private float _startTime;
    private bool _lost = false; 

    [SerializeField] private PlayerMovement[] _bugPrefabs;
    [SerializeField] private Vector3 _defaultSpawnPosition = new Vector3();


    [SerializeField] private GameMode _gameMode;


    private void OnEnable()
    {
        AssertSingleton(this);
    }

    public void EndGame()
    {
        _lost = true;
        OnGameOver?.Invoke(GetScore());
        //LeanTween.value(1.0f, 0.01f, 0.5f).setOnUpdate(SetTime);
    }
    public GameMode GetGameMode()
    {
        return _gameMode;
    }

    private void SetTime(float time)
    {
        Time.timeScale = time;
    }

    public int GetScore()
    {
        return _replayInputs.Count;
    }

    private void Update()
    {
        //for testing purposes
        if (Input.GetKeyDown(KeyCode.O))
        {
            _lost = false;
            _replayInputs.Clear();
            StartLevel();
            _defaultSpawnPosition.y = 0f;
        }
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

    public void SetGameMode(GameMode mode)
    {
        _gameMode = mode;
    }

    private void StartLevel()
    {
        if (_lost) return;
        _startTime = Time.time;

        PlayerMovement bugToInstantiate;// = _bugPrefabs[Random.Range(0, _bugPrefabs.Length)];

        switch(_gameMode)
        {
            case GameMode.Ants:
                bugToInstantiate = _bugPrefabs[0];
                break;
            case GameMode.LadyBirds:
                bugToInstantiate = _bugPrefabs[1];
                break;
            case GameMode.Caterpillars:
                bugToInstantiate = _bugPrefabs[2];
                break;
            default:
                bugToInstantiate = _bugPrefabs[Random.Range(0, _bugPrefabs.Length)];
                break;
        }

        //TODO fix this shit
        
        var spawnPosition = _replayInputs.Count == 0
            ? _defaultSpawnPosition
            : _replayInputs[_replayInputs.Count - 1].StartPosition.Add(y: -1.5f);

        var spawnRotation = Quaternion.Euler(0, 0, 0);
        var spawnedBug = Instantiate(bugToInstantiate, spawnPosition, spawnRotation);
        OnLevelStarted?.Invoke();
    }
}
[System.Serializable]
public enum GameMode
{
    Ants,
    LadyBirds,
    Caterpillars,
    Random
}
