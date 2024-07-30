using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class UIParentCategory : EnhancedScrollerCellView
{
    [Header("Information")]
    ParentViewData parentData;

    [Header("Component")]
    public TextMeshProUGUI textNameParent;
    public TextMeshProUGUI textLevelsRange;
    public GameObject onCompleted;

    public Transform listChildrenContainer;

    public GameObject childCatePrefabs;

    public List<ChildCategory> childCategories;
    public List<ChildCategoryButton> listChildButton = new List<ChildCategoryButton>();


    public void SetParent(ParentViewData _parentData)
    {
        parentData = _parentData;
        childCategories = new List<ChildCategory>(parentData.parent.listChild);

        textNameParent.text = parentData.parentName;
        textLevelsRange.text = $"Levels {parentData.indexLevelStart} - {parentData.indexLevelEnd}";

        onCompleted.SetActive(GameManager.Instance.unlockedLevel > parentData.indexLevelEnd);

        SpawnListChild();

        // Set Size 
        parentData.cellSize = gameObject.GetComponent<RectTransform>().rect.height;
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

            if ( + levelCounter <= GameManager.Instance.unlockedLevel)
            {
                childButton.SetChild(childCategories[i], parentData.index, i);
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
            childButton.OnActiveLevel();
        }
    }

    private void OnDisable()
    {
        if (parentData.indexCateActive >= 0)
        {
            GameEvent.hiddenLevelContainer?.Invoke();
        }
    }

}
