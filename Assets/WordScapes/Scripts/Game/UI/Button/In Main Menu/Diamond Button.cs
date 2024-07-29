
public class DiamondButton : TopButtonBase
{
    protected override void OnClick()
    {
    }

    private void OnEnable()
    {
        GameEvent.inMainMenu += OnEnableButton;
    }
    private void OnDisable()
    {
        GameEvent.inMainMenu -= OnEnableButton;
    }
}
