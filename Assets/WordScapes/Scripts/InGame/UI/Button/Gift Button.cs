

public class GiftButton : LeftButtonBase
{
    private bool active = true;
    protected override void OnClick()
    {
        GameEvent.inGameplay?.Invoke(active);
        active = !active;
    }
}
