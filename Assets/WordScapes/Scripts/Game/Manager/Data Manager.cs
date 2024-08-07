using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonBase<DataManager>
{
    [Header("Game Data")]
    public GameData gameData;
    public ExtraWordData extraWordData;
    [Space]
    public static List<string> extraWords;
    public static List<int> listLevelID;
    public static Dictionary<int,Tuple<ChildCategory, int> > cateOfLevelID;

    public string stateCurLvKey = "stateCurrentLevel";

    [Header("Player Data")]
    public static int unlockedLevel;
    public static int coin;
    public static int diamond;
    public static int brilliance;
    [Space]
    public string unlockedLevelKey = "unlockedLevel";
    public string coinKey = "coin";
    public string diamondKey = "diamond";
    public string brillianceKey = "brilliance";

    [Header("Booster Data")]
    public static int numIdea;
    public static int numPoint;
    public static int numRocket;
    [Space]
    public string numIdeaKey = "numIdea";
    public string numPointKey = "numPoint";
    public string numRocketKey = "numRocket";
    [Space]
    public int costIdea = 100;
    public int costPoint = 200;
    public int costRocket = 300;

    public void Start()
    {
        listLevelID = new List<int>();
        cateOfLevelID = new Dictionary<int,Tuple<ChildCategory, int>>();

        LeaderBoardManager.Load();
        LoadNProcessGameData();
        LoadPlayerData();

        GameEvent.coinChanged?.Invoke(coin);
        GameEvent.diamondChanged?.Invoke(diamond);
    }

    private void LoadNProcessGameData()
    {
        extraWords = new List<string>(extraWordData.listWords);

        List<ParentCategory> parents = new List<ParentCategory>(gameData.listParent);
        int numParents = parents.Count;

        foreach(ParentCategory parent in parents)
        {
            foreach(ChildCategory child in parent.listChild)
            {
                for(int i = 0; i < child.listLevelID.Count; i++)
                {
                    listLevelID.Add(child.listLevelID[i]);
                    cateOfLevelID.Add(listLevelID.Count, new Tuple<ChildCategory, int>(child, i));
                }
            }
        }
    }

    private void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey(unlockedLevelKey))
            unlockedLevel = PlayerPrefs.GetInt(unlockedLevelKey);
        else 
            unlockedLevel = 1;

        if (PlayerPrefs.HasKey(coinKey))
            coin = PlayerPrefs.GetInt(coinKey);
        else
            coin = 50;

        if (PlayerPrefs.HasKey(diamondKey))
            diamond = PlayerPrefs.GetInt(diamondKey);
        else diamond = 0;

        if (PlayerPrefs.HasKey(brillianceKey))
            brilliance = PlayerPrefs.GetInt(brillianceKey);
        else brilliance = 0;

        if (PlayerPrefs.HasKey(numIdeaKey))
            numIdea = PlayerPrefs.GetInt(numIdeaKey);
        else numIdea = 1;

        if (PlayerPrefs.HasKey(numPointKey))
            numPoint = PlayerPrefs.GetInt(numPointKey);
        else numPoint = 1;

        if (PlayerPrefs.HasKey(numRocketKey))
            numRocket = PlayerPrefs.GetInt(numRocketKey);
        else numRocket = 1;
    }

    private void SavePlayerData()
    {
        PlayerPrefs.SetInt(unlockedLevelKey, unlockedLevel);
        PlayerPrefs.SetInt(coinKey, coin);
        PlayerPrefs.SetInt(diamondKey, diamond);
        PlayerPrefs.SetInt(brillianceKey, brilliance);

        PlayerPrefs.SetInt(numIdeaKey, numIdea);
        PlayerPrefs.SetInt(numPointKey, numPoint);
        PlayerPrefs.SetInt(numRocketKey, numRocket);
    }

    #region ResourcesManager 
    public static void RankUp(int increase)
    {
        brilliance += increase;
        LeaderBoardManager.UpdateRankData();
    }

    public static void EarnCoin(int amount)
    {
        coin += amount;
        GameEvent.coinChanged.Invoke(coin);
    }

    public static bool SpentCoint(int amount)
    {
        if (amount > coin) return false;

        coin -= amount;
        GameEvent.coinChanged?.Invoke(coin);
        return true;
    }

    public static void EarnDiamond(int amount)
    {
        diamond += amount;
        GameEvent.diamondChanged?.Invoke(diamond);
    }

    public static bool SpentDiamond(int amount)
    {
        if (amount > coin) return false;

        diamond -= amount;
        GameEvent.diamondChanged?.Invoke(diamond);
        return true;
    }

    #endregion

    #region Booster Manager
    public void EarnIdeaBooster(int num)
    {
        numIdea += num;
        GameEvent.amountIdeaChanged?.Invoke(numIdea);
    }

    public bool SpentIdeaBooster()
    {
        if(numIdea > 0)
        {
            numIdea--;
            GameEvent.amountIdeaChanged?.Invoke(numIdea);
            return true;
        }

        return SpentCoint(costIdea);
    }

    public void EarnPointBooster(int num)
    {
        numPoint += num;

        GameEvent.amountPointChanged?.Invoke(numPoint);
    }


    public bool EnoughPointBooster()
    {
        if(numPoint > 0 || coin >= costPoint)
        {
            return true;
        }
        return false;
    }

    public void SpentPointBooster()
    {
        if(numPoint > 0)
        {
            numPoint--;
            GameEvent.amountPointChanged?.Invoke(numPoint);
        }
        else
        {
            SpentCoint(costPoint);
        }
    }

    public void EarnRocketBooster(int num)
    {
        numRocket += num;
        GameEvent.amountRocketChanged?.Invoke(numRocket);
    }

    public bool SpentRocketBooster()
    {
        if(numRocket > 0)
        {
            numRocket--;
            GameEvent.amountRocketChanged?.Invoke(numRocket);
            return true;
        }

        return SpentCoint(costRocket);
    }

    #endregion

    public StateCurrentLevel LoadStateCurLevel()
    {
        if (PlayerPrefs.HasKey(stateCurLvKey))
        {
            string json = PlayerPrefs.GetString(stateCurLvKey);
            return JsonConvert.DeserializeObject<StateCurrentLevel>(json);
        }
        return null;
    }

    public void SaveStateCurLevel(StateCurrentLevel stateCurrentLevel)
    {
        string json = JsonConvert.SerializeObject(stateCurrentLevel);
        PlayerPrefs.SetString(stateCurLvKey, json);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        SavePlayerData();
    }
}


public class StateCurrentLevel
{
    public int levelIndex;
    public List<int> indexVisible;
}