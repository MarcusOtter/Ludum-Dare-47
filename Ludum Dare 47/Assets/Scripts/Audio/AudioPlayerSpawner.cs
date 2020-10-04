using System;
using System.Threading.Tasks;
using UnityEngine;

public class AudioPlayerSpawner : SingletonBehaviour<AudioPlayerSpawner>
{
    /// <summary>The time to wait after the audio clip has ended before destroying the audio source.</summary>
    private const float SoundEffectDestroyDelayInSeconds = 0.5f;
    private readonly Type[] _customAudioClipTypes = {typeof(AudioSource), typeof(AudioPlayer)};
    private readonly MusicFadeSettings _defaultMusicFadeSettings = new MusicFadeSettings();

    [SerializeField] private AudioPlayer _audioPlayerPrefab;
    private AudioPlayer _currentMusicPlayer;

    private void OnEnable()
    {
        AssertSingleton(this);
    }

    public async Task PlayNewMusicAsync(CustomAudioClip newMusic, MusicFadeSettings overrideFadeSettings = null)
    {
        if (!newMusic.IsMusic)
        {
            Debug.LogError($"{newMusic.name} does not have IsMusic = true");
            return;
        }

        var fadeSettings = overrideFadeSettings ?? _defaultMusicFadeSettings;

        if (_currentMusicPlayer != null)
        {
            await StopCurrentMusic(fadeSettings);
            await Task.Delay((int) (_defaultMusicFadeSettings.DelayBetweenSongsInSeconds * 1000f));
        }

        await StartNewMusic(newMusic, fadeSettings);
    }

    public void PlaySoundEffect(CustomAudioClip customAudioClip, ulong delayInSeconds = 0L)
    {
        if (customAudioClip.IsMusic)
        {
            Debug.LogError($"{customAudioClip.name} does not have IsMusic = false");
            return;
        }

        Instantiate(_audioPlayerPrefab, transform).Play(customAudioClip, delayInSeconds);
    }

    private async Task StopCurrentMusic(MusicFadeSettings fadeSettings)
    {
        await _currentMusicPlayer.FadeOutAsync(fadeSettings);
        Destroy(_currentMusicPlayer.gameObject);
        _currentMusicPlayer = null;
    }

    private async Task StartNewMusic(CustomAudioClip customAudioClip, MusicFadeSettings fadeSettings)
    {
        //// Start new music
        //var spawnedAudioSource = SpawnAudioSource(customAudioClip);
        //var spawnedPitchVolumeUpdater = spawnedAudioSource.GetComponent<AudioPlayer>();
        //spawnedPitchVolumeUpdater.enabled = false;

        //spawnedAudioSource.volume = 0f;
        //_currentMusicPlayer = spawnedAudioSource;
        //_currentMusicPlayer.Play();

        //var targetVolume = customAudioClip.GetVolume() * AudioSettings.Instance.BaseVolume;

        //// Fade in new music
        //if (fadeSettings.FadeTimeInSeconds != 0)
        //{
        //    while (_currentMusicPlayer.volume < targetVolume)
        //    {
        //        await Task.Delay((int) (fadeSettings.UpdateDelayInSeconds * 1000f));
        //        _currentMusicPlayer.volume += targetVolume * (fadeSettings.UpdateDelayInSeconds / fadeSettings.FadeTimeInSeconds);
        //    }
        //}

        //_currentMusicPlayer.volume = targetVolume;
        //spawnedPitchVolumeUpdater.enabled = true;
        //spawnedPitchVolumeUpdater.Play(customAudioClip);
    }
}
