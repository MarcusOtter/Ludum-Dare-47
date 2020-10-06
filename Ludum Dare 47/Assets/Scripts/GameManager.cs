using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    //                                        Main loop
    //                         | -------<-----------<----------<-------
    //                         ⋁                                      |
    public event Action OnPrepareNewLevel;     //                     ⋀
    public event Action OnLevelStart;         //                      |
    public event Action OnLevelFinish;       //                       |
    public event Action<int> OnRewindBegin; //                        |
    public event Action      OnRewindEnd;  //                         ⋀
    //                         |                                      |
    //                         ⋁ ------->----------->---------->-------
    public event Action<int> OnGameOver;

    [Header("Main loop delays")]
    [SerializeField] private int _timeUntilRewindInMs = 3000;
    [SerializeField] private int _rewindDurationInMs = 3000;

    [Header("Countdown audio")]
    [SerializeField] private CustomAudioClip _countdownThreeAudio;
    [SerializeField] private CustomAudioClip _countdownTwoAudio;
    [SerializeField] private CustomAudioClip _countdownOneAudio;
    [SerializeField] private CustomAudioClip _countdownGoAudio;

    private BugType _nextBugTypeToSpawn;
    private float _startTime;
    private bool _lost;
    private int _score;

    private void OnEnable()
    {
        AssertSingleton(this);
    }

    public async Task StartMainLoop()
    {
        _lost = false;

        await Task.Delay(1500);
        OnPrepareNewLevel?.Invoke();
        await Task.Delay(1500);
        await PlayCountdownAudioAsync();

        _startTime = Time.time;
        OnLevelStart?.Invoke();
    }

    public async Task TriggerLevelFinish()
    {
        _score++;
        OnLevelFinish?.Invoke();
        await Task.Delay(_timeUntilRewindInMs);
        OnRewindBegin?.Invoke(_rewindDurationInMs);
        AudioSettings.Instance.BasePitch = -1f;
        await Task.Delay(_rewindDurationInMs);
        OnRewindEnd?.Invoke();
        AudioSettings.Instance.BasePitch = 1;

        await StartMainLoop();
    }

    public void TriggerGameOver()
    {
        _lost = true;
        OnGameOver?.Invoke(_score);
        _score = 0;
    }

    public BugType GetNextBugTypeToSpawn()
    {
        return _nextBugTypeToSpawn;
    }

    public void SetNextBugTypeToSpawn(BugType bugType)
    {
        _nextBugTypeToSpawn = bugType;
    }

    public float GetTimeSinceLevelStart()
    {
        return Time.time - _startTime;
    }

    private async Task PlayCountdownAudioAsync()
    {
        AudioPlayerSpawner.Instance.PlaySoundEffect(_countdownThreeAudio);
        await Task.Delay(1000);
        AudioPlayerSpawner.Instance.PlaySoundEffect(_countdownTwoAudio);
        await Task.Delay(1000);
        AudioPlayerSpawner.Instance.PlaySoundEffect(_countdownOneAudio);
        await Task.Delay(1000);
        AudioPlayerSpawner.Instance.PlaySoundEffect(_countdownGoAudio);
    }
}
