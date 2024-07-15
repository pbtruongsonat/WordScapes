using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class LevelManager : SingletonBase<LevelManager>
{
    public ExtraWordData extraWordData;
    public List<string> extraWordList;

    public LevelData levelData;
    public GameObject background;

    [Header("Level")]
    public Dictionary<string, Word> wordList;
    public List<string> slovedWordList;

    public void Start()
    {
        extraWordList = new List<string>(extraWordData.listWords);
        wordList = new Dictionary<string, Word>();
        foreach (var word in levelData.words)
        {
            wordList.Add(word.word, word);
        }
    }

    public void CheckWord(string wordstr)
    {
        if (wordList.ContainsKey(wordstr))
        {
            if (!slovedWordList.Contains(wordstr))
            {
                // Slove New Word
                GridBoardManager.Instance.SlovedNewWord(wordList[wordstr]);
                slovedWordList.Add(wordstr);
            } else
            {
                // Word has been solved
                Debug.Log($"{wordstr} is sloved");
            }
        } else if (extraWordList.Contains(wordstr))
        {
            // Dont exist word in level, but word is correct => bonus
            Debug.Log($"bonus word: {wordstr}");
        }
    }
}
