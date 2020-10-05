public class AudioSettings : SingletonBehaviour<AudioSettings>
{
    internal float BasePitch { get; set; } = 1f;
    internal float BaseVolume { get; set; } = 0.5f;

    private void OnEnable()
    {
        AssertSingleton(this);
    }

    // Maybe scroll to adjust volume or something
    // Could have an event for when the volume/pitch is updated and then have a UI listen to it to show the new volume number (between 0-100)
}
