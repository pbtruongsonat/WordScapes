using UnityEngine;

public class SoundButton : ToggleButtonBase
{
    protected override void ApplyChange()
    {
        Debug.Log($"Sound: " + isOn);
    }
}
