using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlagDir
{
    H,
    V,
    HV,
}
public class SortGrid : MonoBehaviour
{
    Dictionary<string, Word> dicWords = new Dictionary<string, Word>();
    Dictionary<char, List<Tuple<int, int>>> charPos = new Dictionary<char, List<Tuple<int, int>>>();
    FlagDir[,] flagPos = new FlagDir[25, 25];

    List<string> listWord;

    public void SortGridWords()
    {
        listWord = CreatorManager.Instance.selectedWords;
        StartSort();
    }

    public void StartSort()
    {
        int randIndex = UnityEngine.Random.Range(0, listWord.Count - 1);

    }

    //public bool SortWord(string word)
    //{

    //}
}
