using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Category : MonoBehaviour
{
    [Header("DropDown Menu Select")]
    public TMP_Dropdown parentCategory;
    public TMP_Dropdown childCategory;

    [Header("Category")]
    public Dictionary<string, ParentCategory> dicParent;
    public Dictionary<string, ChildCategory> dicChild;
    public List<string> listNameParent;

    private void Start()
    {
        parentCategory.onValueChanged.AddListener(delegate { UpdateChildList(); });
        childCategory.onValueChanged.AddListener(delegate { childChangeValue(); });

        dicParent = new Dictionary<string, ParentCategory>();
        dicChild = new Dictionary<string, ChildCategory>();

        foreach(var category in CategoryManager.Instance.gameData.listParent)
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
        dicChild.Clear();
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
        childChangeValue();
    }

    public void childChangeValue()
    {
        var parent = CategoryManager.Instance.gameData.listParent[parentCategory.value];
        CategoryManager.Instance.UpdateChildSelected(parent.listChild[childCategory.value]);
    }
}
