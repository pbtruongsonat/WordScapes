
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIPopupReward : UIPopupBase
{
    public TextMeshProUGUI textLevelID;
    public TextMeshProUGUI textComplete;
    public TextMeshProUGUI valueReward;


    public override void OnEnablePopup()
    {
        base.OnEnablePopup();

    }

    public override void OnDisablePopup()
    {
        base.OnDisablePopup();
        DOVirtual.DelayedCall(0.1f, () => { gameObject.SetActive(false); });
    }

    private void OnEnable()
    {
        OnEnablePopup();
    }
}
