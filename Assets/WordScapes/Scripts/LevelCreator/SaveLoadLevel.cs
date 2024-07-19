using UnityEngine;
using SFB;
using System.IO;
using Newtonsoft.Json;
using System;

public class SaveLoadLevel : MonoBehaviour
{
    // Save Level From Creator
    public void SaveFileLevel()
    {
        if(GridBoardManager.Instance.levelData == null)
        {
            #if UNITY_EDITOR
            Debug.LogWarning("Dont have valid data level");
            #endif
            return;
        }
        LevelData levelData = GridBoardManager.Instance.levelData;
        var extensions = new[] { new ExtensionFilter("Json File", "json") };
        try
        {
            string nameFile = CreatorManager.Instance.nameLevel.text;
            var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", nameFile, extensions);
            //var path = Application.dataPath + "/" + nameFile;
            string json = JsonConvert.SerializeObject(levelData, Formatting.Indented);
            File.WriteAllText(path, json);
        }
        catch (ArgumentException ex) {
            Debug.Log(ex.Message);
        }
    }


    // Load Level To Creator
    public void OpenFileLevel()
    {
        var extensions = new[] { new ExtensionFilter("Json File", "json") };
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        //var path = Application.dataPath + "/" + "Level_0_0_0";
        if (path.Length>0 && File.Exists(path[0]))
        {
            string json = File.ReadAllText(path[0]);
            GridBoardManager.Instance.levelData = JsonConvert.DeserializeObject<LevelData>(json);
            GridBoardManager.Instance.LoadLevelGrid();
            GridBoardManager.Instance.DisplaySloved();
        }
    }
}
