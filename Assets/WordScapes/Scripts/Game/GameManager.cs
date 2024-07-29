using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonBase<GameManager>
{
    [Header("Data Player")]
    public int unlockedLevel;
    public int coin;
    public int diamond;
    public int brilliance;

    [Header("Data Game")]
    public GameData gameData;
    public List<int> listLevelID = new List<int>();
    public Dictionary<int, Sprite> listBackground = new Dictionary<int, Sprite>();

    [Header("Component")]
    public int currentLevel;
    public LevelManager levelManager;
    public UIManager uiManager;

    public Image backgroundGamePlay;

    void Start()
    {
        LoadDataPlayer();
        LoadDataGame();
        //OnGamePlay(unlockedLevel);
        GameEvent.inMainMenu?.Invoke(true);
    }

    private void LoadDataPlayer()
    {
        if (PlayerPrefs.HasKey("unlockedLevel"))
        {
            unlockedLevel = PlayerPrefs.GetInt("unlockedLevel");
        } 
        else
        {
            unlockedLevel = 1;
            PlayerPrefs.SetInt("unlockedLevel", unlockedLevel);
        }

        if (PlayerPrefs.HasKey("coin"))
        {
            coin = PlayerPrefs.GetInt("coin");
        }
        else
        {
            coin = 50;
            PlayerPrefs.SetInt("coin", coin);
        }

        if (PlayerPrefs.HasKey("diamond"))
        {
            diamond = PlayerPrefs.GetInt("diamond");
        }
        else
        {
            diamond = 0;
            PlayerPrefs.SetInt("diamond", diamond);
        }

        if (PlayerPrefs.HasKey("brilliance"))
        {
            brilliance = PlayerPrefs.GetInt("brilliance");
        }
        else
        {
            brilliance = 0;
            PlayerPrefs.SetInt("brilliance", brilliance);
        }
    }

    private void LoadDataGame()
    {
        foreach (var parent in gameData.listParent)
        {
            foreach (var child in parent.listChild)
            {
                foreach(var idLevel in child.listLevelID)
                {
                    listLevelID.Add(idLevel);
                    listBackground.Add(listLevelID.Count - 1, child.backgroundImage);
                }
            }
        }
    }

    public void WinGame()
    {
        if(currentLevel == unlockedLevel)
        {
            ++ unlockedLevel;
            // Win new level in the Game
        } else
        {
            // Re-win a previously completed level
        }
        StartCoroutine(Wait1Second());
    }

    IEnumerator Wait1Second()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.DisplayMainMenu();
    }

    public void OnGamePlay(int levelNumber)
    {
        currentLevel = levelNumber;

        string path = $"Data/Level/{listLevelID[levelNumber - 1]}";
        TextAsset fileLevel = Resources.Load<TextAsset>(path);
        if (fileLevel == null) return;

        //uiManager.DisplayGamePlay();
        LevelData levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);
        backgroundGamePlay.sprite = listBackground[levelNumber-1];
        levelManager.SetLevel(levelData);
    }

    private void OnEnable()
    {
        GameEvent.playLevel += OnGamePlay;
    }
    private void OnDisable()
    {
        GameEvent.playLevel -= OnGamePlay;
    }
}
