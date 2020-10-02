using UnityEngine;


[CreateAssetMenu(menuName = "Sound Effect")]
public class SoundEffect : ScriptableObject
{
    [Header("Volume")]
    [SerializeField, Range(0, 1)] private float _baseVolume = 0.5f;
    [SerializeField] private bool _hasRandomVolume;
    [SerializeField, Range(0, 1)] private float _maxVolume;
    [SerializeField, Range(0, 1)] private float _minVolume;

    [Header("Audio clips")]
    [SerializeField] private bool _hasRandomClip;
    [SerializeField] private AudioClip[] _audioClips;

    [Header("Pitch")]
    [SerializeField, Range(0, 4)] private float _basePitch = 1f;
    [SerializeField] private bool _hasRandomPitch;
    [SerializeField, Range(0, 1)] private float _maxPitch;
    [SerializeField, Range(0, 1)] private float _minPitch;

    public AudioClip GetClip()
    {
        if (_audioClips.Length == 0) { return null; }

        return _hasRandomClip
            ? _audioClips[Random.Range(0, _audioClips.Length)]
            : _audioClips[0];
    }

    public float GetVolume()
    {
        return _hasRandomVolume
            ? Random.Range(_minVolume, _maxVolume)
            : _baseVolume;
    }

    public float GetPitch()
    {
        return _hasRandomPitch
            ? Random.Range(_minPitch, _maxPitch)
            : _basePitch;
    }
}
