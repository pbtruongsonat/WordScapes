using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayButton : ButtonBase
{
    public TextMeshProUGUI levelNumberText;
    protected override void OnEnableButton(bool enable)
    {
        if (enable)
        {
            UpdateTextContent();
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        } 
        else
        {
            transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InFlash);
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        levelNumberText = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void OnClick()
    {
        UIManager.Instance.DisplayGamePlay();
        GameEvent.playLevel?.Invoke(GameManager.Instance.unlockedLevel);
    }

    public void UpdateTextContent()
    {
        levelNumberText.text = $"LEVEL {GameManager.Instance.unlockedLevel}";
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
