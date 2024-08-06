using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{
    private string leaderBoardDataKey = "leaderBoardDataKey";
    private string fileNames = "leaderboard-default";

    [Serializable]
    public class LeaderBoardData
    {
        public List<RankData> rankDatas = new List<RankData>();
    }

    public class ListNameData
    {
        public List<string> names = new List<string>();
    }

    public LeaderBoardData leaderBoardData
    {
        get
        {
            if (PlayerPrefs.HasKey(leaderBoardDataKey))
            {
                string json = PlayerPrefs.GetString(leaderBoardDataKey);
                return JsonConvert.DeserializeObject<LeaderBoardData>(json);
            } 
            else
            {
                var defaultLeaderBoard = new LeaderBoardData();
                var defaultNames = new ListNameData();

                string json = Resources.Load<TextAsset>(fileNames).text;
                defaultNames = JsonConvert.DeserializeObject<ListNameData>(json);
                int numberPlayers = defaultNames.names.Count;

                int curBrilliance = DataManager.brilliance;
                //defaultLeaderBoard.rankDatas.Add(new RankData("You", curBrilliance));
                
                for (int i = 0; i < numberPlayers; i++)
                {
                    int randBrilliance = curBrilliance + UnityEngine.Random.Range(-200, 5000);
                    if (randBrilliance < 12)
                        randBrilliance = 12;

                    defaultLeaderBoard.rankDatas.Add(new RankData(defaultNames.names[i], randBrilliance));
                }

                defaultLeaderBoard.rankDatas.Sort();

                json = JsonConvert.SerializeObject(defaultLeaderBoard);
                PlayerPrefs.SetString(leaderBoardDataKey, json);

                return defaultLeaderBoard;
            }
        }

        set
        {
            leaderBoardData = value;
            PlayerPrefs.SetString(leaderBoardDataKey, JsonConvert.SerializeObject(leaderBoardData));
        }
    }
}
