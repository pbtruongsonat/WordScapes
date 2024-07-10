using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelData : ScriptableObject
{
    public int numRow;
    public int numCol;
    public string letters;
    public List<Word> words;
}
