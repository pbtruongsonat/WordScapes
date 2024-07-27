using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIMainMenu : MonoBehaviour
{
    [Header("UI Component")]
    public Image diamondButton;
    public Image giftButton;
    public Image dictionaryButton;
    public Image playButton;
    public GameObject bottomComponent;

    IEnumerator OpenUI()
    {
        diamondButton.rectTransform.DOPivotY(0.5f, 0.5f).SetEase(Ease.OutExpo);
        giftButton.rectTransform.DOPivotX(0.5f, 0.5f).SetEase(Ease.OutExpo);
        dictionaryButton.rectTransform.DOPivotX(0.5f, 0.5f).SetEase(Ease.OutExpo);
        playButton.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        yield return null;

    }

    IEnumerator CloseUI()
    {
        diamondButton.rectTransform.DOPivotY(-5f, 0.5f).SetEase(Ease.OutExpo);
        giftButton.rectTransform.DOPivotX(-5f, 0.5f).SetEase(Ease.OutExpo);
        dictionaryButton.rectTransform.DOPivotX(5f, 0.5f).SetEase(Ease.OutExpo);
        playButton.rectTransform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InSine);

        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    public void PlayAnimation(string k)
    {
        StartCoroutine(k);
    }


    private void OnEnable()
    {
        StartCoroutine("OpenUI");
    }

    private void OnDisable()
    {

    }
}
