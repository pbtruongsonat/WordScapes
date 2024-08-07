using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonBase<GameManager>
{
    [Header("Data Game")]
    public GameData gameData;
    public Dictionary<int, Sprite> listBackground = new Dictionary<int, Sprite>();

    [Header("Component")]
    public int currentLevel;
    public LevelManager levelManager;
    public UIManager uiManager;
    public PopupManager popupManager;

    public Image backgroundGamePlay;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;   
    }
    void Start()
    {
        GameEvent.inMainMenu?.Invoke(true);
    }


    public void WinGame()
    {
        if(currentLevel == DataManager.unlockedLevel)
        {
            ++ DataManager.unlockedLevel;
            uiManager.CloseAllUI();
            // Win new level in the Game
            popupManager.StartDisplayPopup();
        } else
        {
            // Re-win a previously completed level
            StartCoroutine(Wait1Second());
        }
        
    }

    IEnumerator Wait1Second()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.DisplayMainMenu();
    }

    public void OnGamePlay(int levelNumber)
    {
        currentLevel = levelNumber;

        string path = $"Data/Level/{DataManager.listLevelID[levelNumber - 1]}";
        TextAsset fileLevel = Resources.Load<TextAsset>(path);
        if (fileLevel == null) return;

        LevelData levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);

        var cateOfLevel = DataManager.cateOfLevelID[levelNumber];
        backgroundGamePlay.sprite = cateOfLevel.Item1.backgroundImage;
        levelManager.SetLevel(levelData);
    }

    // 

    private void OnEnable()
    {
        GameEvent.playLevel += OnGamePlay;
    }
    private void OnDisable()
    {
        GameEvent.playLevel -= OnGamePlay;
    }
}
