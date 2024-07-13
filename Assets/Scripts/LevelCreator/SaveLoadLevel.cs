using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class SaveLoadLevel : MonoBehaviour
{
    // Save Level From Creator
    public void SaveFileLevel()
    {
        if(GridBoardManager.Instance.levelData == null)
        {
            Debug.LogWarning("Dont have valid data level");
            return;
        }
        LevelData levelData = GridBoardManager.Instance.levelData;
        var extensions = new[] { new ExtensionFilter("Json File", "json") };
        try
        {
            string nameFile = CreatorManager.Instance.nameLevel.text;
            var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", nameFile, extensions);
            string json = JsonConvert.SerializeObject(levelData, Formatting.Indented);
            File.WriteAllText(path, json);
        }
        catch (ArgumentException ex) {
            Debug.Log(ex.Message);
        }
    }

    public LevelData GetLevelDataFromCreator()
    {
        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

        // Get values of the attributes


        return levelData;
    }




    // Load Level To Creator
    public void OpenFileLevel()
    {
        var extensions = new[] { new ExtensionFilter("Json File", "json") };
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        if (path.Length>0 && File.Exists(path[0]))
        {
            string json = File.ReadAllText(path[0]);
            GridBoardManager.Instance.levelData = LevelDataFromJson(json);
            GridBoardManager.Instance.LoadLevelGrid();
        }
    }

    public LevelData LevelDataFromJson(string json)
    {
        LevelDataDTO levelDTO = JsonConvert.DeserializeObject<LevelDataDTO>(json);

        LevelData levelData = ScriptableObject.CreateInstance<LevelData>();

        levelData.numRow = levelDTO.numRow;
        levelData.numCol = levelDTO.numCol;
        levelData.words = levelDTO.words;

        return levelData;
    }

}
