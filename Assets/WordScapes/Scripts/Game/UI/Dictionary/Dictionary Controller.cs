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


    void Start()
    {
        scroller.Delegate = this;

        _datas = new List<MeaningWordData>();

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
        return 873;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _datas.Count;
    }

    #endregion
}
