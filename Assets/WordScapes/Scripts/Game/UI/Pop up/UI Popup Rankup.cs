using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupRankup : UIPopupBase
{
    public Image congraImage;
    public GameObject brillianceObj;
    public TextMeshProUGUI textBrillianceValue;
    public int value;

    public UIPopupWin uiPopupWin;

    IEnumerator IECounter()
    {
        int numBrillianceReward = 5 + Random.Range(1, 10);
        DataManager.brilliance += numBrillianceReward;
        value = int.Parse(textBrillianceValue.text);

        yield return new WaitForSeconds(0.66f);
        for(int i = 0; i < numBrillianceReward; i++)
        {
            value++;
            textBrillianceValue.text = value.ToString();
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);

        uiPopupWin.gameObject.SetActive(true);
        OnDisablePopup();
    }

    public override void OnEnablePopup()
    {
        base.OnEnablePopup();
        congraImage.transform.localScale = Vector3.zero;
        textBrillianceValue.text = DataManager.brilliance.ToString();

        congraImage.DOFade(1, 0.15f);
        congraImage.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.OutBack);
        brillianceObj.transform.DOScale(1, 0.1f);

        StartCoroutine(IECounter());
    }

    public override void OnDisablePopup()
    {
        base.OnDisablePopup();
        congraImage.DOFade(0, 0.1f);
        brillianceObj.transform.DOScale(0, 0.1f);
        DOVirtual.DelayedCall(0.1f, () => { gameObject.SetActive(false); });
    }

    private void OnEnable()
    {
        OnEnablePopup();
    }
}
