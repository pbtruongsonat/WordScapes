using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : SingletonBase<LevelManager>
{
    public ExtraWordData extraWordData;
    public List<string> extraWordList;

    public LevelData levelData;
    public GameObject background;

    [Header("Level")]
    public int curLevel;
    public Dictionary<string, Word> wordList;
    public List<string> slovedWordList;

    [Header("Bonus Word")]
    public List<string> listBonusWord;

    public void Start()
    {
        levelData = new LevelData();
        LoadLevelData(9);
        extraWordList = new List<string>(extraWordData.listWords);
    }

    public void LoadLevelData(int parentIndex, int childIndex, int levelIndex)
    {
        string path = $"Data/Level/9";
        TextAsset fileLevel = Resources.Load<TextAsset>(path);
        if (fileLevel == null) return;

        levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);
        wordList = new Dictionary<string, Word>();
        foreach (var word in levelData.words)
        {
            wordList.Add(word.word, word);
        }
        GridBoardManager.Instance.LoadNewLevel(levelData);
        LetterManager.Instance.LoadNewLevel();
    }

    public void LoadLevelData(int levelIndex)
    {
        //string path = $"/Data/Level/{levelIndex}.json";
        string path = $"Data/Level/{levelIndex}";
        TextAsset fileLevel = Resources.Load<TextAsset>(path);
        if (fileLevel == null) return;

        Debug.Log(fileLevel.text);
        levelData = JsonConvert.DeserializeObject<LevelData>(fileLevel.text);
        wordList = new Dictionary<string, Word>();
        foreach (var word in levelData.words)
        {
            wordList.Add(word.word, word);
        }
        GridBoardManager.Instance.LoadNewLevel(levelData);
        LetterManager.Instance.LoadNewLevel();
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
                if (slovedWordList.Count == levelData.words.Count)
                {
                    Win();
                }
            } else
            {
                // Word has been solved
                Debug.Log($"{wordstr} is sloved");
            }
        } else if (extraWordList.Contains(wordstr) && !listBonusWord.Contains(wordstr))
        {
            // Dont exist word in level, but word is correct => bonus
            listBonusWord.Add(wordstr);
            Debug.Log($"bonus word: {wordstr}");
        }
    }
    
    private void Win()
    {
        Debug.Log("win");
    }
}
