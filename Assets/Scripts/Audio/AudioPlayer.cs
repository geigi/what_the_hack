using Base;
using UE.Events;

/// <summary>
/// Play sound effects from anywhere.
/// </summary>
public class AudioPlayer : Singleton<AudioPlayer>
{
    public GameEvent SelectEvent, DeselectEvent;

    /// <summary>
    /// Play the select sound.
    /// </summary>
    public void PlaySelect()
    {
        SelectEvent.Raise();
    }

    /// <summary>
    /// Play the deselect sound.
    /// </summary>
    public void PlayDeselect()
    {
        DeselectEvent.Raise();
    }
}