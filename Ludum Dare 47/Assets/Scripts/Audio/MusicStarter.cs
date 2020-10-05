using System.Threading.Tasks;
using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private MusicFadeSettings _fadeSettings;
    [SerializeField] private CustomAudioClip _musicToPlayOnStart;

    private async void Start()
    {
        if (_musicToPlayOnStart == null) { return; }

        await PlayNewMusicAsync(_musicToPlayOnStart, _fadeSettings);
    }

    public async Task PlayNewMusicAsync(CustomAudioClip music, MusicFadeSettings overrideFadeSettings = null)
    {
        await AudioPlayerSpawner.Instance.PlayNewMusicAsync(music, overrideFadeSettings);
    }
}
