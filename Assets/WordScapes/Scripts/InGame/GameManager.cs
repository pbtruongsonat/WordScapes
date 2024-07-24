using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    [Header("Component")]
    public LevelManager levelManager;

    void Start()
    {
        LoadDataPlayer();
        LoadDataGame();
        OnGamePlay(unlockedLevel);
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

        LevelData levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);
        levelManager.SetLevel(levelData);
    }

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
}
