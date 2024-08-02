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
        yield return new WaitForSeconds(0.7f);

        popupRankUp.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);

        popupWin.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        popupWin.OnDisablePopup();

        popupWinReward.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
    }
}
