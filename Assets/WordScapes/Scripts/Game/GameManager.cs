using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
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
    public LevelManager levelManager;
    public UIManager uiManager;

    public Image background;

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
        unlockedLevel++;
        StartCoroutine("Wait2Second");
    }

    IEnumerator Wait2Second()
    {
        yield return new WaitForSeconds(2f); 
        OnGamePlay(unlockedLevel);
    }

    public void OnGamePlay(int levelNumber)
    {
        string path = $"Data/Level/{listLevelID[levelNumber - 1]}";
        TextAsset fileLevel = Resources.Load<TextAsset>(path);
        if (fileLevel == null) return;

        uiManager.OpenGamePlay();
        LevelData levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);
        background.sprite = listBackground[levelNumber-1];
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
