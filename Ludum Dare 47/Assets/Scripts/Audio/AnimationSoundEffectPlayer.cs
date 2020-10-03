using System.Threading.Tasks;
using UnityEngine;

public class AnimationSoundEffectPlayer : MonoBehaviour
{
    // Triggered by animation event 
    public void PlayCustomAudioClip(CustomAudioClip customAudioClip)
    {
        AudioPlayer.Instance.PlaySoundEffect(customAudioClip);
    }
}
