using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

public class SelectLevelController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject parentPrefabs;
    public GameObject levelButtonPrefabs;

    [Header("Other Transform")]
    public Transform levelContainer;
    public Transform contentScroll;

    [Header("Component")]
    public ChildCategory childSelected;
    public ParentCategory parentSeleted;

    public Dictionary<ChildCategory, int> dicLevelIdStart = new Dictionary<ChildCategory, int>();
    public Dictionary<ChildCategory, ParentCategory> dicChild = new Dictionary<ChildCategory, ParentCategory>();

    public Dictionary<int, string> dicLettersOfLevel = new Dictionary<int, string>();


    public void Start()
    {
        ProcessData();
        SpawnParentCategory();
    }

    private void ProcessData()
    {
        int levelIdStart = 1;
        foreach(var parent in GameManager.Instance.gameData.listParent)
        {
            foreach(var child in parent.listChild)
            {
                dicChild.Add(child, parent);

                dicLevelIdStart.Add(child, levelIdStart);

                levelIdStart += child.listLevelID.Count;
            }
        }
    }

    private void SpawnParentCategory()
    {
        foreach(var parent in GameManager.Instance.gameData.listParent)
        {
            var parentObj = Instantiate(parentPrefabs, contentScroll);
            parentObj.GetComponent<UIParentCategory>()?.SetParent(parent);
        }
    }

    private void SetLevelContainer(ChildCategory child)
    {
        int numLevels = child.listLevelID.Count;
        int startIdLevel = dicLevelIdStart[child];


        while(levelContainer.childCount < numLevels)
        {
            var levelBtn = Instantiate(levelButtonPrefabs, levelContainer);
        }

        for(int i = 0; i < levelContainer.childCount; i++)
        {
            levelContainer.GetChild(i).gameObject.SetActive(false);
        }

        for(int i = 0; i < numLevels; i++)
        {
            var levelId = startIdLevel + i;

            // Add Letters for Levels
            if (!dicLettersOfLevel.ContainsKey(levelId))
            {
                string path = $"Data/Level/{child.listLevelID[i]}";
                TextAsset fileLevel = Resources.Load<TextAsset>(path);
                if (fileLevel == null) continue;

                LevelData levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);

                dicLettersOfLevel.Add(levelId, levelData.letters);
            }

            var levelBtn = levelContainer.GetChild(i);
            levelBtn.GetComponent<LevelButton>().SetLevel(levelId, dicLettersOfLevel[levelId]);
            levelBtn.gameObject.SetActive(true);
        }

        Canvas.ForceUpdateCanvases();
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
