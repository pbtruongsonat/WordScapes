using UnityEngine;
using SFB;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class SaveLoadLevel : SingletonBase<SaveLoadLevel>
{
    public string pathLevel = "Assets/WordScapes/Resources/Data/Level/";
    public List<int> listIDLevels = new List<int>();
    
    [Header("Creator Button")]
    public Button saveLevelButton;
    public Button browserLevelButton;
    public Button loadLevelButton;

    [Header("Creator Text")]
    public TextMeshProUGUI idLevelSave;
    public TMP_InputField inputIdLevel;

    public void Start()
    {
        saveLevelButton.onClick.AddListener(() => SaveFileLevel());
        browserLevelButton.onClick.AddListener(() => BrowserFileLevel());
        loadLevelButton.onClick.AddListener(() => OpenFileLevel());

        LoadAllLevelID();
    }
    // Save Level From Creator
    public void SaveFileLevel()
    {
        if(GridBoardManager.Instance.levelData == null)
        {
            #if UNITY_EDITOR
            Debug.LogWarning("Dont have valid data level, press SortGrid button and try again");
            #endif
            return;
        }
        LevelData levelData = GridBoardManager.Instance.levelData;
        var extensions = new[] { new ExtensionFilter("Json File", "json") };
        try
        {
            //var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", nameFile, extensions);

            string levelId = idLevelSave.text;

            var path = $"{pathLevel}{levelId}.json";
            //var path = Application.dataPath + "/" + nameFile;
            string json = JsonConvert.SerializeObject(levelData, Formatting.Indented);
            File.WriteAllText(path, json);
            CreatorManager.Instance.ResetData();
            LoadAllLevelID();
        }
        catch (ArgumentException ex) {
            Debug.Log(ex.Message);
        }
    }


    // Load Level To Creator
    public void BrowserFileLevel()
    {
        var extensions = new[] { new ExtensionFilter("Json File", "json") };
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        if (paths.Length>0)
        {
            LoadFileLevel(paths[0]);
        }
    }

    public void OpenFileLevel()
    {
        if(inputIdLevel.text == "")
        {
            Debug.LogWarning("Please enter a valid level ID to open the file");
            return;
        }
        var path = $"Assets/WordScapes/Resources/Data/Level/{inputIdLevel.text}.json";
        LoadFileLevel(path);
    }

    public void LoadFileLevel(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GridBoardManager.Instance.levelData = JsonConvert.DeserializeObject<LevelData>(json);
            GridBoardManager.Instance.LoadLevelGrid();
            GridBoardManager.Instance.DisplaySloved();
        } else
        {
#if UNITY_EDITOR
            Debug.LogWarning("Level with this ID not found, please check again");
#endif
        }
    }
    public void LoadAllLevelID()
    {
        string[] files = Directory.GetFiles(pathLevel, "*.json");
        foreach (string file in files)
        {
            int idLevel = int.Parse(Path.GetFileNameWithoutExtension(file));
            if (!listIDLevels.Contains(idLevel))
            {
                listIDLevels.Add(idLevel);
            }
        }
        listIDLevels.Sort();
        int id = 1;
        while (listIDLevels.Contains(id))
        {
            id++;
        }
        idLevelSave.text = id.ToString();
    }
}
