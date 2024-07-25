public class RocketButton : LeftButtonBase
{
    protected override void OnClick()
    {
        GameEvent.onClickRocket?.Invoke();
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
