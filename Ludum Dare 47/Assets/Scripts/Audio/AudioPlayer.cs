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

    // TODO: implement fading in this class and add a bool for it
    // if bool fading then don't auto assign vol and pitch

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
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
            Destroy(gameObject, delayInSeconds + customAudioClip.GetClip().length + _destroyDelay);
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
        _isFading = true;
        var fadeSettings = fadeSettingsOverride ?? _defaultFadeSettings;



        _isFading = false;
    }

    public void Mute(bool muted)
    {
        _isMuted = muted;
    }

    private void Update()
    {
        if (_customAudioClip == null) { return; }

        if (_isFading || _isMuted) { return; }

        _audioSource.pitch = _customAudioClip.GetPitch() * AudioSettings.Instance.BasePitch;
        _audioSource.volume = _customAudioClip.GetVolume() * AudioSettings.Instance.BaseVolume;
    }
}
