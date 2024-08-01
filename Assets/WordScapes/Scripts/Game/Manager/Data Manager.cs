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

    public void Start()
    {
        listLevelID = new List<int>();
        cateOfLevelID = new Dictionary<int,Tuple<ChildCategory, int>>();

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
    public static void EarnCoin(int amount)
    {
        coin += amount;
    }

    public static bool SpentCoint(int amount)
    {
        if (amount > coin) return false;

        coin -= amount;
        return true;
    }

    public static void EarnDiamond(int amount)
    {
        diamond += amount;
    }

    public static bool SpentDiamond(int amount)
    {
        if (amount > coin) return false;

        diamond -= amount;
        return true;
    }

    #endregion

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        SavePlayerData();
    }
}
