using UnityEngine;

public class BonusWordButton : LeftButtonBase
{
    protected override void OnClick()
    {
        Debug.Log("Bonus Word");
    }
    private void OnEnable()
    {
        GameEvent.inGameplay += base.OnEnableButton;
    }
    private void OnDisable()
    {
        GameEvent.inGameplay -= base.OnEnableButton;
    }
}
