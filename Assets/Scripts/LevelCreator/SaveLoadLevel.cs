using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SFB;
using System.IO;

public class SaveLoadLevel : MonoBehaviour
{
    public LevelData levelData;

    public void SaveFileLevel()
    {
        levelData = GridBoardManager.Instance.levelData;
        var extensions = new[] { new ExtensionFilter("Json File", "json") };
        var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "level1", extensions);
        string json = JsonUtility.ToJson(levelData, true);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }
    public void OpenFileLevel()
    {
        var extensions = new[] { new ExtensionFilter("Json File", "json") };
        var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, true);
        if (path.Length>0 && File.Exists(path[0]))
        {
            string json = File.ReadAllText(path[0]);
            levelData = JsonUtility.FromJson<LevelData>(json);
            Debug.Log(levelData.ToString());
            GridBoardManager.Instance.levelData = levelData;
            GridBoardManager.Instance.LoadLevelGrid();
        }
    }

}
