using UnityEngine;

public class AnimationSoundEffectPlayer : MonoBehaviour
{
    // Triggered by animation event 
    public void PlayCustomAudioClip(CustomAudioClip customAudioClip)
    {
        AudioPlayerSpawner.Instance.PlaySoundEffect(customAudioClip);
    }
}
