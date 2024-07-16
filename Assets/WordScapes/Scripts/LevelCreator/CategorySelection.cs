using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Category : MonoBehaviour
{
    [Header("DropDown Menu Select")]
    public TMP_Dropdown parentCategory;
    public TMP_Dropdown childCategory;

    [Header("Data Category")]
    public List<ParentCategory> listParentCategory;
    public Dictionary<string, ParentCategory> dicParent;
    public List<string> listNameParent;

    private void Start()
    {
        parentCategory.onValueChanged.AddListener(delegate { UpdateChildList(); });
        childCategory.onValueChanged.AddListener(delegate { UpdateLevelName(); });

        dicParent = new Dictionary<string, ParentCategory>();
        foreach(var category in listParentCategory)
        {
            dicParent.Add(category.name, category);
            listNameParent.Add(category.name);
        }
        childCategory.ClearOptions();
        parentCategory.ClearOptions();
        parentCategory.AddOptions(listNameParent);
        UpdateChildList();
    }

    private void UpdateChildList()
    {
        List<string> listNameChildCategory = new List<string>();
        string nameParent = parentCategory.options[parentCategory.value].text;
        if (listNameParent.Contains(nameParent))
        {
            foreach(var category in dicParent[nameParent].listChild)
            {
                listNameChildCategory.Add(category.name);
            }
            childCategory.ClearOptions();
            childCategory.AddOptions(listNameChildCategory);
        }
        UpdateLevelName();
    }
    private void UpdateLevelName()
    {
        int levelCount = 0;
        string nameParent = parentCategory.options[parentCategory.value].text;
        if (listNameParent.Contains(nameParent))
        {
            ParentCategory parent = dicParent[nameParent];
            string nameChild = childCategory.options[childCategory.value].text;
            foreach(var child in parent.listChild)
            {
                if(child.name == nameChild)
                {
                    levelCount = child.listLevel.Count;
                    break;
                }
            }
        }
        CreatorManager.Instance.nameLevel.text = $"Level_{parentCategory.value}_{childCategory.value}_{levelCount}";
    }
}
