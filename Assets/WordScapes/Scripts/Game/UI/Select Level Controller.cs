using DG.Tweening;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class SelectLevelController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject parentPrefabs;
    public GameObject levelButtonPrefabs;

    [Header("Other Transform")]
    public Transform levelContainer;
    public Transform contentScroll;
    public RectTransform contentScrollRect;

    [Header("Component")]
    public ChildCategory childSelected;
    public ParentCategory parentSeleted;

    public Dictionary<ChildCategory, int> dicLevelIdStart = new Dictionary<ChildCategory, int>();
    public Dictionary<ChildCategory, Transform> dicChild = new Dictionary<ChildCategory, Transform>(); // Parent obj transform of child
    
    public Dictionary<ParentCategory, Tuple<int, int>> rangeLevelParent = new Dictionary<ParentCategory, Tuple<int, int>>();

    public Dictionary<int, string> dicLettersOfLevel = new Dictionary<int, string>();


    public void Start()
    {
        ProcessData();
        //SpawnParentCategory();
    }

    private void ProcessData()
    {
        int levelIdStart = 1;
        foreach(var parent in GameManager.Instance.gameData.listParent)
        {
            var parentObj = Instantiate(parentPrefabs, contentScroll);

            int parentStart = levelIdStart;

            foreach (var child in parent.listChild)
            {
                dicChild.Add(child, parentObj.transform);

                dicLevelIdStart.Add(child, levelIdStart);

                levelIdStart += child.listLevelID.Count;
            }

            rangeLevelParent.Add(parent, new Tuple<int,int>(parentStart, levelIdStart - 1));

            parentObj.GetComponent<UIParentCategory>()?.SetParent(parent, parentStart, levelIdStart - 1);
        }
    }

    //private void SpawnParentCategory()
    //{
    //    foreach(var parent in GameManager.Instance.gameData.listParent)
    //    {
    //    }
    //}

    private void SetLevelContainer(ChildCategory child, bool active)
    {
        if (!active)
        {
            levelContainer.gameObject.SetActive(false);
            return;
        } else
        {

            levelContainer.gameObject.SetActive(true);

            int numLevels = child.listLevelID.Count;
            int startIdLevel = dicLevelIdStart[child];


            while (levelContainer.childCount < numLevels)
            {
                var levelBtn = Instantiate(levelButtonPrefabs, levelContainer);
            }

            for (int i = 0; i < levelContainer.childCount; i++)
            {
                levelContainer.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < numLevels; i++)
            {
                var levelId = startIdLevel + i;

                // Add Letters for Levels
                if (!dicLettersOfLevel.ContainsKey(levelId) && levelId <= GameManager.Instance.unlockedLevel)
                {
                    string path = $"Data/Level/{child.listLevelID[i]}";
                    TextAsset fileLevel = Resources.Load<TextAsset>(path);
                    if (fileLevel == null) continue;

                    LevelData levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);

                    dicLettersOfLevel.Add(levelId, levelData.letters);
                }

                var levelBtn = levelContainer.GetChild(i);
                var levelBtnScript = levelBtn.GetComponent<LevelButton>();

                if (levelId <= GameManager.Instance.unlockedLevel)
                {
                    levelBtnScript.SetLevel(levelId, dicLettersOfLevel[levelId]);
                    if (levelId == GameManager.Instance.unlockedLevel)
                    {
                        levelBtnScript.SetCurrentLevel();
                    }
                } else
                {
                    levelBtnScript.SetLevel(levelId);
                }

                levelBtn.gameObject.SetActive(true);
            }

            levelContainer.SetParent(dicChild[child]);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentScrollRect);
    }

    private void OnEnable()
    {
        GameEvent.displayListLevel += SetLevelContainer;
    }

    private void OnDisable()
    {
        GameEvent.displayListLevel -= SetLevelContainer;
    }
}
