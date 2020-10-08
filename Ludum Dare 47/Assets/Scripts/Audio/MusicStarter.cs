using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private MusicFadeSettings _fadeSettings;
    [SerializeField] private CustomAudioClip _musicToPlayOnStart;

    private void Start()
    {
        if (_musicToPlayOnStart == null) { return; }

        PlayNewMusicAsync(_musicToPlayOnStart, _fadeSettings);
    }

    public void PlayNewMusicAsync(CustomAudioClip music, MusicFadeSettings overrideFadeSettings = null)
    {
        AudioPlayerSpawner.Instance.PlayNewMusicAsync(music, overrideFadeSettings);
    }
}
