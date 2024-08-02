using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UIPopupWin : UIPopupBase
{
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textProcessCate;

    public GameObject impressiveObj;
    public List<GameObject> listStars = new List<GameObject>();
    public List<GameObject> probarReward = new List<GameObject>();
    public Slider sliderProbar;



    IEnumerator StarsAnimation()
    {
        foreach (GameObject star in listStars)
        {
            star.transform.localScale = Vector3.zero;
        }
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject obj in listStars)
        {
            obj.transform.DOScale(Vector3.one, 1.2f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(0.5f);
        }

        sliderProbar.DOValue(DataManager.cateOfLevelID[GameManager.Instance.currentLevel].Item2 + 1, 2f, false);
    }

    public override void OnEnablePopup()
    {
        base.OnEnablePopup();
        transform.localScale = Vector3.one;

        var cateOfLevel = DataManager.cateOfLevelID[GameManager.Instance.currentLevel];
        sliderProbar.maxValue = cateOfLevel.Item1.listLevelID.Count;
        sliderProbar.value = cateOfLevel.Item2;

        textLevel.text = $"LEVEL {GameManager.Instance.currentLevel}";
        textProcessCate.text = $"{cateOfLevel.Item1.name} {cateOfLevel.Item2 + 1}/{sliderProbar.maxValue}";

        impressiveObj.transform.localScale = Vector3.zero;

        impressiveObj.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);

        StartCoroutine(StarsAnimation());
    }

    public override void OnDisablePopup()
    {
        base.OnDisablePopup();
        transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InOutQuart);
        DOVirtual.DelayedCall(0.1f, () => { gameObject.SetActive(false); });

    }
    private void OnEnable()
    {
        OnEnablePopup();
    }
    public void OnDisable()
    {
    }
}
