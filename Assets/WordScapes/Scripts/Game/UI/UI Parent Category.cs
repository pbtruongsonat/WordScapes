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
    public List<ChildCategoryButton> listChildButton = new List<ChildCategoryButton>();

    public void SetParent(ParentCategory parent)
    {
        this.parent = parent;
        textNameParent.text = this.parent.name;

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

        for(int i = 0; i < numChildren; i++)
        {
            var childButton = listChildButton[i];
            childButton.SetChild(parent.listChild[i]);
            childButton.gameObject.SetActive(true);
        }
    }
}
