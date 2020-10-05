using UnityEngine;

public class PlayOnEvent : MonoBehaviour
{
    private ParticleSystem _particles;
    private CustomAudioClip _clip;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnLevelFinish += PlayStuff;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnLevelFinish += PlayStuff;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            PlayStuff();
        }
    }

    private void PlayStuff()
    {
        if(_particles != null) _particles.Play();
        if (_clip != null) AudioPlayerSpawner.Instance.PlaySoundEffect(_clip);
    }
}
