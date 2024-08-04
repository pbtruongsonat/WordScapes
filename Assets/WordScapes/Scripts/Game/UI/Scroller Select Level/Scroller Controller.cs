using EnhancedUI.EnhancedScroller;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Tayx.Graphy.Advanced;
using UnityEngine;

public class ScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public List<ScrollViewData> data;

    public EnhancedScroller myScroller;
    public RectTransform rectScroller;

    public EnhancedScrollerCellView headerUIPrefab;
    public EnhancedScrollerCellView parentUIPrefab;
    public EnhancedScrollerCellView footerUIPrefab;

    [Header("Height of header and footer")]
    public float headerHeight;
    public float footerHeight;

    //
    private bool _lastPadderActive;
    private float _lastPadderSize;


    private void Start()
    {
        Application.targetFrameRate = 60;

        rectScroller = myScroller.GetComponent<RectTransform>();
        myScroller.Delegate = this;

        myScroller.lookAheadAfter = 1000f;
        myScroller.lookAheadBefore = 1000f;

        LoadData();
    }

    private void LoadData()
    {
        data = new List<ScrollViewData>();
        int levelIdStart = 1;

        data.Add(new HeaderViewData(headerHeight));

        foreach (var parent in GameManager.Instance.gameData.listParent)
        {
            ParentViewData parentData = new ParentViewData();

            parentData.parent = parent;
            parentData.parentName = parent.name;

            parentData.indexLevelStart = levelIdStart;

            foreach (var child in parent.listChild)
            {
                levelIdStart += child.listLevelID.Count;
            }

            parentData.indexLevelEnd = levelIdStart - 1;

            parentData.indexCateActive = -1;
            parentData.expandedSize = 472;
            parentData.collapsedSize = 472;

            parentData.tweenType = Tween.TweenType.easeInOutSine;
            parentData.tweenTimeCollapse = 0.5f;
            parentData.tweenTimeExpand = 0.5f;

            data.Add(parentData);
        }

        data.Add(new FooterViewData(footerHeight));

        myScroller.ReloadData();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return data.Count;
    }


    // ------ Action Tween -------
    public void InitializeTween(int dataIndex, int cellViewIndex)
    {
        ParentViewData parentData = data[dataIndex] as ParentViewData;

        if (parentData == null) return;



        var cellPosition = myScroller.GetScrollPositionForCellViewIndex(cellViewIndex, EnhancedScroller.CellViewPositionEnum.Before);

        var tweenCellOffset = cellPosition - myScroller.ScrollPosition;

        myScroller.IgnoreLoopJump(true);

        myScroller.ReloadData();

        cellPosition = myScroller.GetScrollPositionForCellViewIndex(cellViewIndex, EnhancedScroller.CellViewPositionEnum.Before);

        myScroller.SetScrollPositionImmediately(cellPosition - tweenCellOffset);

        myScroller.IgnoreLoopJump(false);

        // Set Last Padder
        _lastPadderActive = myScroller.LastPadder.IsActive();
        _lastPadderSize = myScroller.LastPadder.minHeight;

        if(parentData.indexCateActive == -1)
        {
            myScroller.LastPadder.minHeight -= parentData.SizeDifference;
        }
        else
        {
            myScroller.LastPadder.minHeight += parentData.SizeDifference;
        }

        myScroller.LastPadder.gameObject.SetActive(true);

        var uiParent = myScroller.GetCellViewAtDataIndex(dataIndex) as UIParentCategory;
        uiParent.BeginTween();
    }


    private void TweenUpdated(int dataIndex, int cellViewIndex, float newValue, float delta)
    {
        myScroller.LastPadder.minHeight -= delta;
    }


    private void TweenEnd(int dataIndex, int cellViewIndex)
    {
        myScroller.LastPadder.gameObject.SetActive(_lastPadderActive);
        myScroller.LastPadder.minHeight = _lastPadderSize;
    }



    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        ParentViewData parentData = data[dataIndex] as ParentViewData;

        if (parentData != null)
        {
            return parentData.Size;
        }
        else
        {
            return data[dataIndex].cellSize;
        }
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        UIBlockScroll blockView;

        if (data[dataIndex] is ParentViewData)
        {
            blockView = myScroller.GetCellView(parentUIPrefab) as UIParentCategory;
            blockView.SetData(data[dataIndex], dataIndex, TweenUpdated, TweenEnd);
        }
        else if (data[dataIndex] is HeaderViewData)
        {
            blockView = myScroller.GetCellView(headerUIPrefab) as UIHeader;
            blockView.SetData(data[dataIndex]);
        }
        else
        {
            blockView = myScroller.GetCellView(footerUIPrefab) as UIFooter;
            blockView.SetData(data[dataIndex]);
        }

        return blockView;
    }

}
