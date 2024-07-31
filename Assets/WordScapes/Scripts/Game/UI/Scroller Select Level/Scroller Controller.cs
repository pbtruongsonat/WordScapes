using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
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

    private void Start()
    {
        rectScroller = myScroller.GetComponent<RectTransform>();
        //myScroller.Delegate = this;

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

            parentData.index = data.Count;
            parentData.parent = parent;
            parentData.parentName = parent.name;

            parentData.indexLevelStart = levelIdStart;

            foreach (var child in parent.listChild)
            {
                levelIdStart += child.listLevelID.Count;
            }

            parentData.indexLevelEnd = levelIdStart - 1;

            parentData.indexCateActive = -1;

            data.Add(parentData);
        }

        data.Add(new FooterViewData(footerHeight));

        myScroller.Delegate = this;
        ResizeScroller();
        myScroller.ReloadData();
    }

    public void ResizeScroller()
    {
        var size = rectScroller.sizeDelta;

        rectScroller.sizeDelta = new Vector2(size.x, float.MaxValue);

        myScroller.ReloadData();

        rectScroller.sizeDelta = size;

        myScroller.ReloadData();
    }

    public void ResizeScroller(float valueChange)
    {
        myScroller.ResizeContent(valueChange);
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return data[dataIndex].cellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        UIBlockScroll blockView;

        if (data[dataIndex] is ParentViewData)
        {
            blockView = myScroller.GetCellView(parentUIPrefab) as UIParentCategory;
        }
        else if (data[dataIndex] is HeaderViewData)
        {
            blockView = myScroller.GetCellView(headerUIPrefab) as UIHeader;
        }
        else
        {
            blockView = myScroller.GetCellView(footerUIPrefab) as UIFooter;
        }

        blockView.SetData(data[dataIndex]);
        return blockView;
    }

    //public void Update()
    //{
    //    Canvas.ForceUpdateCanvases();
    //}

}
