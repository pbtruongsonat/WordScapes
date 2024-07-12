using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class LevelData : ScriptableObject
{
    public int numRow;
    public int numCol;
    public string letters;
    public List<Word> words;
}


// Class for convert Json Data to Level Data 
[System.Serializable]
public class LevelDataDTO 
{
    public int numRow;
    public int numCol;
    public string letters;
    public List<Word> words;
}
