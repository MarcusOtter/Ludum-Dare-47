using System.Collections;
using UnityEngine;

public class AudioPlayerSpawner : SingletonBehaviour<AudioPlayerSpawner>
{
    private readonly MusicFadeSettings _defaultMusicFadeSettings = new MusicFadeSettings();
    private AudioPlayer _currentMusicPlayer;

    private void OnEnable()
    {
        AssertSingleton(this);
    }

    public void PlayNewMusicAsync(CustomAudioClip newMusic, MusicFadeSettings overrideFadeSettings = null)
    {
        if (!newMusic.IsMusic)
        {
            Debug.LogError($"{newMusic.name} does not have IsMusic = true");
            return;
        }

        StartCoroutine(PlayNewMusicCoroutine(newMusic, overrideFadeSettings));
    }

    private IEnumerator PlayNewMusicCoroutine(CustomAudioClip newMusic, MusicFadeSettings overrideFadeSettings = null)
    {
        var fadeSettings = overrideFadeSettings ?? _defaultMusicFadeSettings;

        if (_currentMusicPlayer != null)
        {
            StopCurrentMusic(fadeSettings);
            yield return new WaitForSeconds(_defaultMusicFadeSettings.DelayBetweenSongsInSeconds);
        }

        StartNewMusic(newMusic, fadeSettings);
    }

    public void PlaySoundEffect(CustomAudioClip customAudioClip, ulong delayInSeconds = 0L)
    {
        if (customAudioClip == null) { return; }

        if (customAudioClip.IsMusic)
        {
            Debug.LogError($"{customAudioClip.name} does not have IsMusic = false");
            return;
        }

        AudioPlayerPool.Instance.GetNewAndEnable()?.Play(customAudioClip, delayInSeconds);
    }

    private void StopCurrentMusic(MusicFadeSettings fadeSettings)
    {
        _currentMusicPlayer.FadeOutAsync(fadeSettings);
        AudioPlayerPool.Instance.ReturnAndDisable(_currentMusicPlayer);
        _currentMusicPlayer = null;
    }

    private void StartNewMusic(CustomAudioClip customAudioClip, MusicFadeSettings fadeSettings)
    {
        var audioPlayer = AudioPlayerPool.Instance.GetNewAndEnable();
        audioPlayer.Play(customAudioClip);
        _currentMusicPlayer = audioPlayer;
        audioPlayer.FadeInAsync(fadeSettings);
    }
}
