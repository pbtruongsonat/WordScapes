using EnhancedUI.EnhancedScroller;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIParentCategory : UIBlockScroll
{
    [SerializeField] private Tween tween;
    [SerializeField] private LayoutElement layoutElement;

    public Action<int, int, float, float> updateTween;
    public Action<int, int> endTween;

    [Header("Information")]
    private ParentViewData parentData;

    [Header("Component")]
    public TextMeshProUGUI textNameParent;
    public TextMeshProUGUI textLevelsRange;
    public Transform listLevel;
    public GameObject onCompleted;

    public Transform listChildrenContainer;

    public GameObject childCatePrefabs;

    public List<ChildCategory> childCategories;
    public List<ChildCategoryButton> listChildButton = new List<ChildCategoryButton>();



    public override void SetData(ScrollViewData data, int dataIndex, Action<int, int, float, float> updateTween, Action<int, int> endTween)
    {
        base.SetData(data, dataIndex, updateTween, endTween);
        this.dataIndex = dataIndex;

        this.updateTween = updateTween;
        this.endTween = endTween;

        parentData = data as ParentViewData;
        childCategories = new List<ChildCategory>(parentData.parent.listChild);

        textNameParent.text = parentData.parentName;
        textLevelsRange.text = $"Levels {parentData.indexLevelStart} - {parentData.indexLevelEnd}";

        onCompleted.SetActive(DataManager.unlockedLevel > parentData.indexLevelEnd);

        SpawnListChild();

        if (parentData.indexCateActive != -1)
        {
            GameEvent.setTransformLevel?.Invoke(transform);
            listChildButton[parentData.indexCateActive].OnSelectChild(true);
        } else
        {
            
        }
    }

    private void SpawnListChild()
    {
        int numChildren = childCategories.Count;

        while (listChildrenContainer.childCount < numChildren)
        {
            GameObject childButton = Instantiate(childCatePrefabs, listChildrenContainer);
            listChildButton.Add(childButton.GetComponent<ChildCategoryButton>());
        }

        foreach (var childButton in listChildButton)
        {
            childButton.gameObject.SetActive(false);
        }

        int levelCounter = 0;

        for (int i = 0; i < numChildren; i++)
        {
            var childButton = listChildButton[i];

            if (parentData.indexLevelStart + levelCounter <= DataManager.unlockedLevel)
            {
                childButton.SetChild(childCategories[i], dataIndex, cellIndex, i);
            }
            else
            {
                childButton.SetChild();
            }

            childButton.gameObject.SetActive(true);

            levelCounter += childCategories[i].listLevelID.Count;
        }

        // Display Level Container
        if(parentData.indexCateActive >= 0 && parentData.indexCateActive < listChildButton.Count)
        {
            var childButton = listChildButton[parentData.indexCateActive];
            childButton.onSelect = true;
            //childButton.OnActiveLevel();
        }
    }

    // Tween 
    public void BeginTween()
    {

        GameEvent.setDisplayLevel?.Invoke(false);

        if (parentData.indexCateActive == -1)
        {
            layoutElement.minHeight = parentData.expandedSize;

            StartCoroutine(tween.TweenPosition(parentData.tweenType, parentData.tweenTimeCollapse, parentData.expandedSize, parentData.collapsedSize, TweenUpdated, TweenCompleted));
        }
        else
        {
            layoutElement.minHeight = parentData.collapsedSize;

            StartCoroutine(tween.TweenPosition(parentData.tweenType, parentData.tweenTimeExpand, parentData.collapsedSize, parentData.expandedSize, TweenUpdated, TweenCompleted));
        }
    }


    private void TweenUpdated(float newValue, float delta)
    {
        layoutElement.minHeight += delta;

        if (updateTween != null)
        {
            updateTween(dataIndex, cellIndex, newValue, delta);
        }
    }


    private void TweenCompleted()
    {
        if(parentData.indexCateActive != -1)
        {
            GameEvent.setTransformLevel?.Invoke(transform);
        }
        if(endTween != null)
        {
            endTween(dataIndex, cellIndex);
        }
    }



    private void OnDisable()
    {
        if (parentData.indexCateActive >= 0)
        {
            GameEvent.setDisplayLevel?.Invoke(false);
        }
    }

}
