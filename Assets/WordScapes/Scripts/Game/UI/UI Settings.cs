using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UISettings : MonoBehaviour
{
    public GameObject settingsPanel;

    public void OpenPanel()
    {
        settingsPanel.transform.localScale = Vector3.zero;
        settingsPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    public void DisablePanel()
    {
        settingsPanel.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.OutFlash);
        StartCoroutine("WaitDisable");
    }

    IEnumerator WaitDisable()
    {
        yield return new WaitForSeconds(0.15f);
        gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        OpenPanel();
    }
    private void OnDisable()
    {
        
    }
}
