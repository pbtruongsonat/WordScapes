using UnityEngine;

public class MusicButton : ToggleButtonBase
{
    protected override void ApplyChange()
    {
        if(isOn)
        {
            Debug.Log("On Music");
        } else
        {
            Debug.Log("Off Music");
        }
    }
}
