using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PitchVolumeUpdater : MonoBehaviour
{
    private AudioSource _audioSource;

    private CustomAudioClip _targetClip;

    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetTargetClip(CustomAudioClip targetClip)
    {
        _targetClip = targetClip;
    }

    private void Update()
    {
        _audioSource.pitch = _targetClip.GetPitch() * AudioSettings.Instance.BasePitch;
        _audioSource.volume = _targetClip.GetVolume() * AudioSettings.Instance.BaseVolume;
    }
}
