using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public EnhancedScroller scroller;
    public List<RankData> rankDatas;

    public RankCellView rankCellViewPrefab;

    public RankData playerRank;
    public RankCellView playerRankCell;

    public LeaderBoardManager leaderBoardManager;

    // Start is called before the first frame update
    void Start()
    {
        leaderBoardManager = GetComponent<LeaderBoardManager>();
        scroller.Delegate = this;

        LoadData();
    }

    
    private void LoadData()
    {
        rankDatas = new List<RankData>();
        rankDatas = leaderBoardManager.leaderBoardData.rankDatas;

        //Player Data
        playerRank = new RankData("You", DataManager.brilliance);
        rankDatas.Add(playerRank);

        rankDatas.Sort();
        playerRankCell.SetData(playerRank, rankDatas.IndexOf(playerRank));

        scroller.ReloadData();
    }

    private void ReloadData()
    {
        if (rankDatas.Count > 0)
        {
            int index = rankDatas.IndexOf(playerRank);

            rankDatas[index].totalBrilliance = DataManager.brilliance;
            rankDatas.Sort();

            index = rankDatas.IndexOf(playerRank);
            playerRankCell.SetData(rankDatas[index], index);

            scroller.ReloadData();
        }
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        RankCellView cellView = scroller.GetCellView(rankCellViewPrefab) as RankCellView;

        cellView.name = $"Rank {dataIndex}";

        cellView.SetData(rankDatas[dataIndex], dataIndex);

        return cellView;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 164;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return rankDatas.Count;
    }

    private void OnEnable()
    {
        ReloadData();
    }
}
