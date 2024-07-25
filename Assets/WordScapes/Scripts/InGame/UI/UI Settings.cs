using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettings : MonoBehaviour
{
    public GameObject settingsPanel;

    public void OpenPanel()
    {
        settingsPanel.transform.localScale = Vector3.zero;
        settingsPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    private void OnEnable()
    {
        OpenPanel();
    }
    private void OnDisable()
    {
        
    }
}
