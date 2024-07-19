using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryManager : SingletonBase<CategoryManager>
{
    [Header("Game Data")]
    public GameData gameData;
    public List<int> availabelIdLevel = new List<int>();

    public ChildCategory childSelected;

    [Header("UI")]
    public ListLevelScroll availableLevel;
    public ListLevelScroll selectedLevel;

    public void UpdateChildSelected(ChildCategory child)
    {
        childSelected = child;
        selectedLevel.listIdLevel = childSelected.listLevelID;
        selectedLevel.UpdateList();
        FindAvailabelIdLevel();
        UpdateLevelInCate();
    }

    public void FindAvailabelIdLevel()
    {
        availabelIdLevel = SaveLoadLevel.Instance.listIDLevels;
        foreach(var parent in gameData.listParent)
        {
            foreach(var child in parent.listChild)
            {
                foreach(var levelID in child.listLevelID)
                {
                    availabelIdLevel.Remove(levelID);
                }
            }
        }
        availableLevel.listIdLevel = availabelIdLevel;
        availableLevel.UpdateList();
    }

    public void UpdateLevelInCate()
    {
        childSelected.listLevelID = selectedLevel.listIdLevel;
    }
}
