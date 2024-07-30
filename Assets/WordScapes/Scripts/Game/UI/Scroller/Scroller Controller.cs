using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using UnityEngine;

public class ScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public List<ParentViewData> data;

    public EnhancedScroller myScroller;
    public UIParentCategory parentUIPrefab;

    private void Start()
    {
        data = new List<ParentViewData>();
        int levelIdStart = 1;

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

            parentData.indexLevelEnd = levelIdStart -1;

            parentData.indexCateActive = -1;

            data.Add(parentData);
        }

        myScroller.Delegate = this;
        ResizeScroller();
        myScroller.ReloadData();
    }

    public void ResizeScroller()
    {
        var rectTransform = myScroller.GetComponent<RectTransform>();
        var size = rectTransform.sizeDelta;

        rectTransform.sizeDelta = new Vector2(size.x, float.MaxValue);

        myScroller.ReloadData();

        rectTransform.sizeDelta = size;

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
        UIParentCategory parentView = scroller.GetCellView(parentUIPrefab) as UIParentCategory;
        parentView.SetParent(data[dataIndex]);

        return parentView;
    }

    public void Update()
    {
        Canvas.ForceUpdateCanvases();
    }

}
