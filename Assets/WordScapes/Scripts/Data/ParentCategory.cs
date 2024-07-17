using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class ParentCategory : ScriptableObject
{
    public string name;
    public List<ChildCategory> listChild;

    public ParentCategory(string name)
    {
        this.name = name;
        listChild = new List<ChildCategory>();
    }
}
