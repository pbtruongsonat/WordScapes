using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : SingletonBase<LevelManager>
{
    [Header("Other Script")]
    public GridBoardManager boardManager;
    public LetterManager letterBoard;
    public InputHandle inputHandle;
    public LineController lineController;

    public ExtraWordData extraWordData;
    public List<string> extraWordList;

    public LevelData levelData;

    [Header("Text UI")]
    public TextMeshProUGUI textLevelID;
    public TextMeshProUGUI textCategoryOrder;

    [Header("Level")]
    public int curLevel;
    public Dictionary<string, Word> wordList = new Dictionary<string, Word>();
    public List<string> slovedWordList;

    [Header("Bonus Word")]
    public TextMeshPro textWordBonus;
    public RectTransform bonusWordButton;
    public List<string> listBonusWord;

    public Transform lettersMoveContainers;
    public GameObject letterMovePrefab;
    public Word curWord;

    public void Start()
    {
        levelData = new LevelData();
        extraWordList = new List<string>(extraWordData.listWords);
    }

    public void SetLevel(LevelData _levelData)
    {
        if (_levelData == null) return;

        this.levelData = _levelData;
        curLevel = GameManager.Instance.currentLevel;

        var cateOfLevel = DataManager.cateOfLevelID[curLevel];

        textLevelID.text = $"LEVEL {curLevel}";
        textCategoryOrder.text = $"{cateOfLevel.Item1.name} {cateOfLevel.Item2 + 1}";

        wordList.Clear();
        slovedWordList.Clear();

        foreach (var word in levelData.words)
        {
            wordList.Add(word.word, word);
        }

        boardManager.LoadNewLevel(levelData);

        letterBoard.LoadNewLevel(levelData.letters);
        lineController.InitLine(levelData.letters.Length - 1);

        while (lettersMoveContainers.childCount < levelData.letters.Length)
        {
            Instantiate(letterMovePrefab, lettersMoveContainers).SetActive(false);
        }
    }

    public bool CheckWord(string wordstr)
    {
        if (wordList.ContainsKey(wordstr))
        {
            curWord = wordList[wordstr];

            if (!slovedWordList.Contains(wordstr))
            {
                // Slove New Word
                StartCoroutine(CreateMoveLetter());
                NewWordSloved(wordstr);
                
            }
            else
            {
                boardManager.SlovedWordAgain(curWord);
                // Word has been solved
            }
            return true;
        }
        else if (extraWordList.Contains(wordstr))
        {
            if (!listBonusWord.Contains(wordstr))
            {
                // Dont exist word in level, but word is correct => bonus
                listBonusWord.Add(wordstr);
                StartCoroutine(IEBonusWord(wordstr));
            }
            else
            {
                bonusWordButton.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 1);
            }
            return true;
        }

        return false;
    }
    
    public void NewWordSloved(string wordstr)
    {
        slovedWordList.Add(wordstr);
        if (slovedWordList.Count == levelData.words.Count)
        {
            Win();
        }
    }

    IEnumerator IEBonusWord(string wordStr)
    {
        textWordBonus.text = wordStr;
        textWordBonus.transform.position = inputHandle.wordInput.transform.position;
        textWordBonus.transform.localScale = Vector3.one;
        textWordBonus.gameObject.SetActive(true);

        textWordBonus.transform.DOScale(Vector3.one * 0.8f, 0.7f);
        yield return textWordBonus.transform.DOJump(bonusWordButton.position, -0.2f, 0, 0.7f).WaitForCompletion();

        textWordBonus.gameObject.SetActive(false);
        bonusWordButton.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 1);
    }

    IEnumerator CreateMoveLetter()
    {
        int index = curWord.startRowIndex * levelData.numCol + curWord.startColIndex;
        int incrementIndex = (curWord.dir == DirectionType.H) ? 1 : levelData.numCol;
        int wordLength = curWord.word.Length;

        for (int i = 0; i < wordLength; i++)
        {
            var letterMove = lettersMoveContainers.GetChild(i);

            letterMove.GetComponent<LetterMoveController>()?.
                SetLetter(curWord.word[i], inputHandle.wordInput.listPosLetters[i], boardManager.cellDic[index].transform.position);
            
            index += incrementIndex;
            yield return new WaitForSeconds(0.04f);
        }
        yield return new WaitForSeconds(0.3f);
        boardManager.SolvedNewWord(curWord);
    }

    // Use Booster 
    private void UseConvertBooster()
    {
        letterBoard.ConvertLetter();
    }

    private void UseIdeaBooster()
    {
        if (DataManager.Instance.SpentIdeaBooster())
        {
            boardManager.RandomIndexHidden();
        }
        else
        {
            Debug.Log("Open Shop");
        }
    }

    private void UseRocketBooster()
    {
        if (DataManager.Instance.SpentRocketBooster())
        {
            boardManager.VisibleFiveCell();
        }
        else
        {
            Debug.Log("Open Shop");
        }
    }

    private void UsePointBooster()
    {
        if (DataManager.Instance.EnoughPointBooster())
        {
            GameEvent.onPointerHint?.Invoke(true);
        }
        else
        {
            Debug.Log("Open Shop");
        }
    }

    private void Win()
    {
        DOVirtual.DelayedCall(1, () => { GameManager.Instance.WinGame(); });
    }

    private void OnEnable()
    {
        GameEvent.onClickConvertLetters += UseConvertBooster;
        GameEvent.onClickIdea += UseIdeaBooster;
        GameEvent.onClickRocket += UseRocketBooster;
        GameEvent.onClickPoint += UsePointBooster;
    }

    private void OnDisable()
    {
        GameEvent.onClickConvertLetters -= UseConvertBooster;
        GameEvent.onClickIdea -= UseIdeaBooster;
        GameEvent.onClickRocket -= UseRocketBooster;
        GameEvent.onClickPoint -= UsePointBooster;
    }
}
