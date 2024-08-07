using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryController : MonoBehaviour, IEnhancedScrollerDelegate
{
    private List<MeaningWordData> _datas;

    public EnhancedScroller scroller;

    public MeaningWordCell meaningWordCellPrefab;
    public WordnikAPI wordnikAPI;

    public FlickSnapScroller flickSnap;
    public float cellViewSize = 873; 
    public float calculateStartCellBias = 0f;

    void Start()
    {
        _datas = new List<MeaningWordData>();

        scroller.Delegate = this;
        scroller.CalculateStartCellBias = calculateStartCellBias;

        //LoadData();
    }

    public void AddNewData(string wordStr)
    {
        wordStr = wordStr.ToLower();

        wordnikAPI.findMeanWord(wordStr);
        //_datas.Add(wordnikAPI.fin(wordStr));

        //scroller.ReloadData();
    }

    public void AddElement(MeaningWordData data)
    {
        _datas.Add(data);

        flickSnap.MaxDataElements = _datas.Count;

        scroller.ReloadData();
    }

    #region EnhancedScroll

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        MeaningWordCell meaningCell = scroller.GetCellView(meaningWordCellPrefab) as MeaningWordCell;

        meaningCell.SetData(_datas[dataIndex]);
        meaningCell.name = $"{_datas[dataIndex].word}";

        return meaningCell;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return cellViewSize;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _datas.Count;
    }

    #endregion
}
