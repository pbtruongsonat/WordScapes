using System.Collections;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public UIPopupBase popupRankUp;
    public UIPopupBase popupWin;
    public UIPopupBase popupWinReward;


    public void StartDisplayPopup()
    {
        StartCoroutine(IEDisplayPopup());
    }

    IEnumerator IEDisplayPopup()
    {
        yield return new WaitForSeconds(1);

        popupRankUp.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        popupWin.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        popupWin.OnDisablePopup();

        popupWinReward.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        popupWinReward.OnDisablePopup();

        UIManager.Instance.DisplayMainMenu();
    }
}
