using UnityEngine;

public class IdeaButton : LeftButtonBase
{
    protected override void OnClick()
    {
        GameEvent.onClickIdea?.Invoke();
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
