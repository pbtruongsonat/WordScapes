using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIMainMenu : MonoBehaviour
{
    public Image background;

    private void OnEnableUI(bool enable)
    {
        if (enable)
        {
            background.DOFade(1, 0.35f);
        } else
        {
            background.DOFade(0, 0.25f);
        }
    }

    private void OnEnable()
    {
        GameEvent.inMainMenu += OnEnableUI;
    }

    private void OnDisable()
    {
        GameEvent.inMainMenu -= OnEnableUI;
    }
}
