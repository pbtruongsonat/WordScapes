using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using UnityEngine;

public class BonusWordController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public List<string> datas;

    public EnhancedScroller scroller;

    public BonusWordCell bonusWordCellPrefabs;
    public float bonusWordCellSize = 64;

    private bool delegatedScroller;

    private void Start()
    {
        delegatedScroller = false;
        datas = new List<string>();
    }

    public void AddNewData(string word)
    {
        datas.Add(word);
    }

    private void ReloadDataScroller()
    {
        if (!delegatedScroller)
        {
            scroller.Delegate = this;
        }

        scroller.ReloadData();
    }

    #region Enhanced Scroller
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        BonusWordCell cell = scroller.GetCellView(bonusWordCellPrefabs) as BonusWordCell;

        cell.SetData(datas[dataIndex]);
        cell.name = datas[dataIndex];

        return cell;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return bonusWordCellSize;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return datas.Count;
    }
    #endregion

    private void OnEnable()
    {
        GameEvent.displayBonusWord += ReloadDataScroller;
    }

    private void OnDisable()
    {
        GameEvent.displayBonusWord -= ReloadDataScroller;
    }
}
