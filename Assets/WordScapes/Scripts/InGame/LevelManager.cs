using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : SingletonBase<LevelManager>
{
    [Header("Other Script")]
    public LetterManager letterBoard;
    public InputHandle inputHandle;
    public LineManager lineManager;

    public ExtraWordData extraWordData;
    public List<string> extraWordList;

    public LevelData levelData;
    public GameObject background;

    [Header("Level")]
    public int curLevel;
    public Dictionary<string, Word> wordList = new Dictionary<string, Word>();
    public List<string> slovedWordList;

    [Header("Bonus Word")]
    public List<string> listBonusWord;

    public Transform lettersMoveContainers;
    public GameObject letterMovePrefab;
    public Word curWord;

    public void Start()
    {
        levelData = new LevelData();
        //LoadLevelData(24);
        extraWordList = new List<string>(extraWordData.listWords);
    }

    public void SetLevel(LevelData _levelData)
    {
        if (_levelData == null) return;

        this.levelData = _levelData;

        wordList.Clear();
        slovedWordList.Clear();

        foreach (var word in levelData.words)
        {
            wordList.Add(word.word, word);
        }

        GridBoardManager.Instance.LoadNewLevel(levelData);
        letterBoard.LoadNewLevel(levelData.letters);
        lineManager.InitLine(levelData.letters.Length - 1);

        while (lettersMoveContainers.childCount < levelData.letters.Length)
        {
            Instantiate(letterMovePrefab, lettersMoveContainers).SetActive(false);
        }
    }

    public void CheckWord(string wordstr)
    {
        if (wordList.ContainsKey(wordstr))
        {
            if (!slovedWordList.Contains(wordstr))
            {
                // Slove New Word
                //CreateMoveLetter(wordList[wordstr]);
                curWord = wordList[wordstr];
                StartCoroutine("CreateMoveLetter");
                //GridBoardManager.Instance.SlovedNewWord(curWord);
               NewWordSloved(wordstr);
                
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
    
    public void NewWordSloved(string wordstr)
    {
        slovedWordList.Add(wordstr);
        if (slovedWordList.Count == levelData.words.Count)
        {
            Win();
        }
    }

    IEnumerator CreateMoveLetter()
    {
        int index = curWord.startRowIndex * levelData.numCol + curWord.startColIndex;
        int incrementIndex = (curWord.dir == DirectionType.H) ? 1 : levelData.numCol;
        for(int i = 0; i < curWord.word.Length; i++)
        {
            var letterMove = lettersMoveContainers.GetChild(i);

            letterMove.GetComponent<LetterMoveController>()?.
                SetLetter(curWord.word[i], inputHandle.listPosLetters[i], GridBoardManager.Instance.cellDic[index].transform.position);
            
            index += incrementIndex;
            yield return new WaitForSeconds(0.04f);
        }
        yield return new WaitForSeconds(0.3f);
        GridBoardManager.Instance.SlovedNewWord(curWord);
    }
    private void Win()
    {
        Debug.Log("win");
        GameManager.Instance.WinGame();
    }
}
