using System.Collections;
using UnityEngine;

public class UIManager : SingletonBase<UIManager>
{
    public GameObject mainMenuUI;
    public GameObject gamePlayUI;

    public void OpenGamePlay()
    {
        StartCoroutine("OpenGamePlayAnim");
    }

    IEnumerator OpenGamePlayAnim()
    {
        GameEvent.inMainMenu?.Invoke(false);
        yield return new WaitForSeconds(0.1f);
        gamePlayUI.SetActive(true);
        GameEvent.inGameplay?.Invoke(true);

        yield return new WaitForSeconds(0.2f);
        mainMenuUI.SetActive(false);
    }

    public void OpenMainMenu()
    {
        StartCoroutine("OpenMainMenuAnim");
    }

    IEnumerator OpenMainMenuAnim()
    {
        GameEvent.inGameplay?.Invoke(false);
        yield return new WaitForSeconds(0.1f);

        mainMenuUI.SetActive(true);
        GameEvent.inMainMenu?.Invoke(true);

        yield return new WaitForSeconds(0.2f);
        gamePlayUI.SetActive(false);
    }

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
            
    }
}
