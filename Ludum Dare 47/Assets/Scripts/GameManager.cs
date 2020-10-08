using System;
using System.Collections;
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

    public void StartMainLoop()
    {
        StartCoroutine(StartMainLoopCoroutine());
    }

    private IEnumerator StartMainLoopCoroutine()
    {
        _lost = false;
        yield return new WaitForSeconds(1.5f);
        OnPrepareNewLevel?.Invoke();
        yield return new WaitForSeconds(1.5f);
        PlayCountdownAudio();
        yield return new WaitForSeconds(3.5f);

        _startTime = Time.time;
        OnLevelStart?.Invoke();
    }

    public void TriggerLevelFinish()
    {
        StartCoroutine(TriggerLevelFinishCoroutine());
    }

    private IEnumerator TriggerLevelFinishCoroutine()
    {
        _score++;
        OnLevelFinish?.Invoke();
        yield return new WaitForSeconds(_timeUntilRewindInMs / 1000f);
        OnRewindBegin?.Invoke(_rewindDurationInMs);
        //AudioSettings.Instance.BasePitch = -1f; // Does not work in web build...
        yield return new WaitForSeconds(_rewindDurationInMs / 1000f);
        OnRewindEnd?.Invoke();
        AudioSettings.Instance.BasePitch = 1;

        StartMainLoop();
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

    private void PlayCountdownAudio()
    {
        StartCoroutine(PlayCountdownAudioCoroutine());
    }

    private IEnumerator PlayCountdownAudioCoroutine()
    {
        AudioPlayerSpawner.Instance.PlaySoundEffect(_countdownThreeAudio);
        yield return new WaitForSeconds(1f);
        AudioPlayerSpawner.Instance.PlaySoundEffect(_countdownTwoAudio);
        yield return new WaitForSeconds(1f);
        AudioPlayerSpawner.Instance.PlaySoundEffect(_countdownOneAudio);
        yield return new WaitForSeconds(1f);
        AudioPlayerSpawner.Instance.PlaySoundEffect(_countdownGoAudio);
    }
}
