using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private float _destroyDelay;

    private bool _isFading;
    private bool _isMuted;

    private AudioSource _audioSource;
    private CustomAudioClip _customAudioClip;
    private readonly MusicFadeSettings _defaultFadeSettings = new MusicFadeSettings();

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_isMuted)
        {
            _audioSource.volume = 0;
            return;
        }

        if (_customAudioClip == null) { return; }
        if (_isFading) { return; }

        _audioSource.pitch = _customAudioClip.GetPitch() * AudioSettings.Instance.BasePitch;
        _audioSource.volume = _customAudioClip.GetVolume() * AudioSettings.Instance.BaseVolume;
    }

    public void Mute(bool muted)
    {
        _isMuted = muted;
    }

    public void Play(CustomAudioClip customAudioClip, ulong delayInSeconds = 0L)
    {
        _customAudioClip = customAudioClip;

        _audioSource.clip = customAudioClip.GetClip();
        _audioSource.loop = customAudioClip.IsMusic;
        _audioSource.volume = customAudioClip.GetVolume();
        _audioSource.pitch = customAudioClip.GetPitch();

        _audioSource.PlayDelayed(delayInSeconds);

        if (!customAudioClip.IsMusic)
        {
            StartCoroutine(ReturnToPoolDelayed(delayInSeconds + customAudioClip.GetClip().length + _destroyDelay));
        }
    }

    public async Task FadeOutAsync(MusicFadeSettings fadeSettingsOverride = null)
    {
        _isFading = true;
        var fadeSettings = fadeSettingsOverride ?? _defaultFadeSettings;

        if (fadeSettings.FadeTimeInSeconds != 0)
        {
            var startingVolume = _audioSource.volume;

            while (_audioSource.volume > 0f)
            {
                await Task.Delay((int)(fadeSettings.UpdateDelayInSeconds * 1000f));
                _audioSource.volume -= startingVolume * (fadeSettings.UpdateDelayInSeconds / fadeSettings.FadeTimeInSeconds);
            }
        }

        _audioSource.volume = 0f;
        _isMuted = true;
        _isFading = false;
    }

    public async Task FadeInAsync(MusicFadeSettings fadeSettingsOverride = null)
    {
        if (_customAudioClip == null)
        {
            Debug.LogWarning("Tried to fade in music but no music was playing");
            return;
        }

        _isMuted = false;
        _isFading = true;
        var fadeSettings = fadeSettingsOverride ?? _defaultFadeSettings;

        var targetVolume = _customAudioClip.GetVolume() * AudioSettings.Instance.BaseVolume;

        if (fadeSettings.FadeTimeInSeconds != 0)
        {
            while (_audioSource.volume < targetVolume)
            {
                await Task.Delay((int)(fadeSettings.UpdateDelayInSeconds * 1000f));
                _audioSource.volume += targetVolume * (fadeSettings.UpdateDelayInSeconds / fadeSettings.FadeTimeInSeconds);
            }
        }

        _audioSource.volume = targetVolume;
        _isFading = false;
    }

    private IEnumerator ReturnToPoolDelayed(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        AudioPlayerPool.Instance.AddToPool(this);
    }
}
