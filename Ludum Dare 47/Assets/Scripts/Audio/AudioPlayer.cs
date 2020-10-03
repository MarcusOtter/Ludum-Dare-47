using System;
using System.Threading.Tasks;
using UnityEngine;

public class AudioPlayer : SingletonBehaviour<AudioPlayer>
{
    /// <summary>The time to wait after the audio clip has ended before destroying the audio source.</summary>
    private const float SoundEffectDestroyDelayInSeconds = 0.5f;
    private readonly Type[] _customAudioClipTypes = {typeof(AudioSource), typeof(PitchVolumeUpdater)};
    private readonly MusicFadeSettings _defaultMusicFadeSettings = new MusicFadeSettings();

    private AudioSource _currentMusicSource;

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

        if (_currentMusicSource != null)
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

        var spawnedAudioSource = SpawnAudioSource(customAudioClip);
        var spawnedPitchVolumeUpdater = spawnedAudioSource.GetComponent<PitchVolumeUpdater>();

        spawnedPitchVolumeUpdater.SetTargetClip(customAudioClip);
        spawnedAudioSource.PlayDelayed(delayInSeconds);

        Destroy(spawnedAudioSource.gameObject, delayInSeconds + customAudioClip.GetClip().length + SoundEffectDestroyDelayInSeconds);
    }

    private AudioSource SpawnAudioSource(CustomAudioClip customAudioClip)
    {
        var clip = customAudioClip.GetClip();
        var volume = customAudioClip.GetVolume();
        var pitch = customAudioClip.GetPitch();

        var spawnedGameObject = new GameObject($"\"{customAudioClip.name}\" audio player", _customAudioClipTypes);
        spawnedGameObject.transform.SetParent(transform);

        var spawnedAudioSource = spawnedGameObject.GetComponent<AudioSource>();

        spawnedAudioSource.playOnAwake = false;
        spawnedAudioSource.clip = clip;
        spawnedAudioSource.loop = customAudioClip.IsMusic;
        spawnedAudioSource.volume = volume;
        spawnedAudioSource.pitch = pitch;

        return spawnedAudioSource;
    }

    private async Task StopCurrentMusic(MusicFadeSettings fadeSettings)
    {
        _currentMusicSource.GetComponent<PitchVolumeUpdater>().enabled = false;

        // Fade out volume
        if (fadeSettings.FadeTimeInSeconds != 0)
        {
            var startingVolume = _currentMusicSource.volume;

            while (_currentMusicSource.volume > 0f)
            {
                await Task.Delay((int) (fadeSettings.UpdateDelayInSeconds * 1000f));
                _currentMusicSource.volume -= startingVolume * (fadeSettings.UpdateDelayInSeconds / fadeSettings.FadeTimeInSeconds);
            }
        }

        _currentMusicSource.volume = 0f;
        Destroy(_currentMusicSource.gameObject);
        _currentMusicSource = null;
    }

    private async Task StartNewMusic(CustomAudioClip customAudioClip, MusicFadeSettings fadeSettings)
    {
        // Start new music
        var spawnedAudioSource = SpawnAudioSource(customAudioClip);
        var spawnedPitchVolumeUpdater = spawnedAudioSource.GetComponent<PitchVolumeUpdater>();
        spawnedPitchVolumeUpdater.enabled = false;

        spawnedAudioSource.volume = 0f;
        _currentMusicSource = spawnedAudioSource;
        _currentMusicSource.Play();

        var targetVolume = customAudioClip.GetVolume() * AudioSettings.Instance.BaseVolume;

        // Fade in new music
        if (fadeSettings.FadeTimeInSeconds != 0)
        {
            while (_currentMusicSource.volume < targetVolume)
            {
                await Task.Delay((int) (fadeSettings.UpdateDelayInSeconds * 1000f));
                _currentMusicSource.volume += targetVolume * (fadeSettings.UpdateDelayInSeconds / fadeSettings.FadeTimeInSeconds);
            }
        }

        _currentMusicSource.volume = targetVolume;
        spawnedPitchVolumeUpdater.enabled = true;
        spawnedPitchVolumeUpdater.SetTargetClip(customAudioClip);
    }
}
