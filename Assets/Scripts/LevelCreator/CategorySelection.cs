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

    private void Start()
    {
        parentCategory.onValueChanged.AddListener((value) => UpdateChildList());
        //parentCategory.OnSelect.AddListener(() => UpdateChildList());
    }

    private void UpdateChildList()
    {
        //childCategory.options.Add
    }
}
