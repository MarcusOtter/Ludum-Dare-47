using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    internal static SoundEffectPlayer Instance { get; private set; }

    private void OnEnable()
    {
        AssertSingleton();
    }

    public void Play(SoundEffect soundEffect, float delay = 0f)
    {
        var clip = soundEffect.GetClip();
        var volume = soundEffect.GetVolume();
        var pitch = soundEffect.GetPitch();

        var gameObjectToSpawn = new GameObject($"{clip.name} player", typeof(AudioSource));
        var spawnedAudioSource = Instantiate(gameObjectToSpawn, transform.position, Quaternion.identity).GetComponent<AudioSource>();

        spawnedAudioSource.clip = clip;
        spawnedAudioSource.volume = volume;
        spawnedAudioSource.pitch = pitch;

        spawnedAudioSource.Play();

        Destroy(spawnedAudioSource.gameObject, delay + clip.length + 0.1f);
    }

    private void AssertSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
