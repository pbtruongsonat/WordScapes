using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelController : MonoBehaviour
{
    [SerializeField] private float padding = 48;
    [SerializeField] private float spacingLevel = 64;
    [SerializeField] private float heightLevel = 180;

    private int indexOldParent = -1;

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
    public ScrollerController scrollerController;

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
        }
    }

    private void SetLevelContainer(int indexParent, int indexCellParent, int indexChild, bool active)
    {
        levelContainer.gameObject.SetActive(false);

        ParentViewData parentViewData = scrollerController.datas[indexParent] as ParentViewData;
        if (parentViewData == null) return;

        ChildCategory child = parentViewData.parent.listChild[indexChild];

        if (indexOldParent != -1 && indexOldParent != indexParent)
        {
            var parentOldData = scrollerController.datas[indexOldParent] as ParentViewData;
            var uiOldParent = scrollerController.scroller.GetCellViewAtDataIndex(indexOldParent) as UIParentCategory;

            if (parentOldData != null && uiOldParent != null)
            {
                parentOldData.indexCateActive = -1;
                scrollerController.InitializeTween(uiOldParent.dataIndex, uiOldParent.cellIndex);
            }
        }

        if (!active)
        {
            parentViewData.indexCateActive = -1;
            levelContainer.gameObject.SetActive(false);
        } 
        else
        {
            parentViewData.indexCateActive = indexChild;

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
                if (!dicLettersOfLevel.ContainsKey(levelId) && levelId <= DataManager.unlockedLevel)
                {
                    string path = $"Data/Level/{child.listLevelID[i]}";
                    TextAsset fileLevel = Resources.Load<TextAsset>(path);
                    if (fileLevel == null) continue;

                    LevelData levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);

                    dicLettersOfLevel.Add(levelId, levelData.letters);
                }

                var levelBtn = levelContainer.GetChild(i);
                var levelBtnScript = levelBtn.GetComponent<LevelButton>();

                if (levelId <= DataManager.unlockedLevel)
                {
                    levelBtnScript.SetLevel(levelId, dicLettersOfLevel[levelId]);
                    if (levelId == DataManager.unlockedLevel)
                    {
                        levelBtnScript.SetCurrentLevel();
                    }
                } else
                {
                    levelBtnScript.SetLevel(levelId);
                }

                levelBtn.gameObject.SetActive(true);
            }

            float numRowLevel = Mathf.Ceil(numLevels / 4f);
            float expandedValue = numRowLevel * heightLevel + padding + (numRowLevel - 1) * spacingLevel + 8;

            parentViewData.expandedSize = parentViewData.collapsedSize + expandedValue;

        }

        if(indexParent != indexOldParent)
        {
            DOVirtual.DelayedCall(0.15f, () => { scrollerController.InitializeTween(indexParent, indexCellParent); });
            indexOldParent = indexParent;
        } 
        else
        {
            scrollerController.InitializeTween(indexParent, indexCellParent);
        }
    }

    IEnumerator ClosePanel(int oldIndexData, int oldIndexCell)
    {
        scrollerController.InitializeTween(oldIndexData, oldIndexCell);
        yield return null;
    }

    private void SetTransformLevel(Transform transformParent)
    {
        levelContainer.SetParent(transformParent);
        levelContainer.gameObject.SetActive(true);
    }

    private void DisplayLevelContainer(bool isActive)
    {
        levelContainer.gameObject.SetActive(isActive);
    }

    private void OnEnable()
    {
        GameEvent.setListLevel += SetLevelContainer;
        GameEvent.setTransformLevel += SetTransformLevel;
        GameEvent.setDisplayLevel += DisplayLevelContainer;
    }

    private void OnDisable()
    {
        GameEvent.setListLevel -= SetLevelContainer;
        GameEvent.setTransformLevel -= SetTransformLevel;
        GameEvent.setDisplayLevel -= DisplayLevelContainer;
    }
}
