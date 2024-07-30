using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameEvent;

public class SelectLevelController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject parentPrefabs;
    public GameObject levelButtonPrefabs;

    [Header("Other Transform")]
    public RectTransform levelContainer;

    public Dictionary<ChildCategory, int> dicLevelIdStart = new Dictionary<ChildCategory, int>();
    public Dictionary<ChildCategory, Transform> dicChild = new Dictionary<ChildCategory, Transform>(); // Parent obj transform of child
    
    public Dictionary<ParentCategory, Tuple<int, int>> rangeLevelParent = new Dictionary<ParentCategory, Tuple<int, int>>();

    public Dictionary<int, string> dicLettersOfLevel = new Dictionary<int, string>();

    [Header("Other Script")]
    public ScrollerController scroller;

    public void Start()
    {
        ProcessData();
    }

    private void ProcessData()
    {
        /// Move to GameData ...

        int levelIdStart = 1;
        foreach (var parent in GameManager.Instance.gameData.listParent)
        {
            //var parentObj = Instantiate(parentPrefabs, contentScroll);

            int parentStart = levelIdStart;

            foreach (var child in parent.listChild)
            {
                //dicChild.Add(child, parentObj.transform);

                dicLevelIdStart.Add(child, levelIdStart);

                levelIdStart += child.listLevelID.Count;
            }

            //rangeLevelParent.Add(parent, new Tuple<int, int>(parentStart, levelIdStart - 1));

            //parentObj.GetComponent<UIParentCategory>()?.SetParent(parent, parentStart, levelIdStart - 1);
        }
    }

    private void SetLevelContainer(int indexParent, int indexChild, Transform parentTransform, bool active)
    {
        ChildCategory child = scroller.data[indexParent].parent.listChild[indexChild];

        if(levelContainer.gameObject.activeSelf)
        {
            float spacingElement = 8;
            float valueChanged = levelContainer.rect.height + spacingElement;
            scroller.ResizeScroller(-valueChanged);
        }

        if (!active)
        {
            levelContainer.gameObject.SetActive(false);
            scroller.data[indexParent].indexCateActive = -1;
        } else
        {
            levelContainer.gameObject.SetActive(true);
            scroller.data[indexParent].indexCateActive = indexChild;

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

            levelContainer.SetParent(parentTransform);


            StartCoroutine(ResetSize());
        }

    }

    IEnumerator ResetSize()
    {
        yield return new WaitForEndOfFrame();

        float spacingElement = 8;
        float valueChanged = levelContainer.rect.height + spacingElement;
        scroller.ResizeScroller(valueChanged);
    }

    public void HiddenLevelContainer()
    {
        float spacingElement = 8;
        float valueChanged = levelContainer.rect.height + spacingElement;

        levelContainer.gameObject.SetActive(false);
        scroller.ResizeScroller(-valueChanged);
    }

    private void OnEnable()
    {
        GameEvent.displayListLevel += SetLevelContainer;
        GameEvent.hiddenLevelContainer += HiddenLevelContainer;
    }

    private void OnDisable()
    {
        GameEvent.displayListLevel -= SetLevelContainer;
        GameEvent.hiddenLevelContainer -= HiddenLevelContainer;
    }
}
