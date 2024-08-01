using UnityEngine;

public class SkipButton : ButtonBase
{
    protected override void OnEnableButton(bool enable)
    {

    }

    protected override void OnClick()
    {
        base.OnClick();
        UIManager.Instance.DisplayMainMenu();
    }
}
