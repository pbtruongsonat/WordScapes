public class ProfileButton : BottomButtonBase
{

    protected override void OnClick()
    {
        throw new System.NotImplementedException();
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
