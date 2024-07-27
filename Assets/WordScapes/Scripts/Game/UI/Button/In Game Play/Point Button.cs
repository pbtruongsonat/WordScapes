using UnityEngine;

public class PointButton : LeftButtonBase
{
    public bool onPointButton = true;
    public GameObject pointIcon;
    public GameObject pointCancel;

    protected override void OnClick()
    {
        GameEvent.onPointerHint?.Invoke(onPointButton);
    }

    public void SetStateButton(bool state)
    {
        onPointButton = !state;
        pointIcon.SetActive(!state);
        pointCancel.SetActive(state);
    }

    private void OnEnable()
    {
        GameEvent.inGameplay += base.OnEnableButton;
        GameEvent.onPointerHint += SetStateButton;
    }

    private void OnDisable()
    {
        GameEvent.inGameplay -= base.OnEnableButton;
        GameEvent.onPointerHint -= SetStateButton;
    }
}
