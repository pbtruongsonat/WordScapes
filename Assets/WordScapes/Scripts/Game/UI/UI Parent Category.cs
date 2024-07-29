using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIParentCategory : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI textNameParent;
    public TextMeshProUGUI textLevelsRange;
    public GameObject onCompleted;

    public Transform listChildrenContainer;
    public Transform listLevelsContainer;

    public GameObject childCatePrefabs;

    public ParentCategory parent;
    public int levelStart;
    public List<ChildCategoryButton> listChildButton = new List<ChildCategoryButton>();

    public void SetParent(ParentCategory parent, int levelStart, int levelEnd)
    {
        this.parent = parent;
        this.levelStart = levelStart;
        textNameParent.text = this.parent.name;
        textLevelsRange.text = $"Levels {levelStart} - {levelEnd}";

        onCompleted.SetActive(GameManager.Instance.unlockedLevel > levelEnd);

        SpawnListChild();
    }

    private void SpawnListChild()
    {
        int numChildren = parent.listChild.Count;

        while (listChildrenContainer.childCount < numChildren)
        {
            var childButton = Instantiate(childCatePrefabs, listChildrenContainer);
            listChildButton.Add(childButton.GetComponent<ChildCategoryButton>());
        }

        foreach(var childButton in listChildButton)
        {
            childButton.gameObject.SetActive(false);
        }

        int levelCounter = 0;

        for(int i = 0; i < numChildren; i++)
        {
            var childButton = listChildButton[i];

            if(levelStart + levelCounter <= GameManager.Instance.unlockedLevel)
            {
                childButton.SetChild(parent.listChild[i]);
            } else
            {
                childButton.SetChild(null);
            }

            childButton.gameObject.SetActive(true);

            levelCounter += parent.listChild[i].listLevelID.Count;
        }
    }
}
