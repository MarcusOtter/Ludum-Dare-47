using System.Threading.Tasks;
using UnityEngine;

public class AudioPlayerSpawner : SingletonBehaviour<AudioPlayerSpawner>
{
    private readonly MusicFadeSettings _defaultMusicFadeSettings = new MusicFadeSettings();
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

        AudioPlayerPool.Instance.Get().Play(customAudioClip, delayInSeconds);
    }

    private async Task StopCurrentMusic(MusicFadeSettings fadeSettings)
    {
        await _currentMusicPlayer.FadeOutAsync(fadeSettings);
        AudioPlayerPool.Instance.AddToPool(_currentMusicPlayer);
        _currentMusicPlayer = null;
    }

    private async Task StartNewMusic(CustomAudioClip customAudioClip, MusicFadeSettings fadeSettings)
    {
        var audioPlayer = AudioPlayerPool.Instance.Get();
        audioPlayer.Play(customAudioClip);
        _currentMusicPlayer = audioPlayer;
        await audioPlayer.FadeInAsync(fadeSettings);
    }
}
