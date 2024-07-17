using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class ChildCategory : ScriptableObject
{
    public string name;
    public Sprite backgroundImage;
    public List<LevelData> listLevel;

    public ChildCategory(string name)
    {
        this.name = name;
        listLevel = new List<LevelData>();
    }
    public ChildCategory(string name, Sprite bg)
    {
        this.name = name;
        this.backgroundImage = bg;
        listLevel = new List<LevelData>();
    }
}
