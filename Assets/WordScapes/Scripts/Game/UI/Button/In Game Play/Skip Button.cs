public class SkipButton : ButtonBase
{

    protected override void OnClick()
    {

        base.OnClick();

        UIManager.Instance.DisplayMainMenu();
    }
}
