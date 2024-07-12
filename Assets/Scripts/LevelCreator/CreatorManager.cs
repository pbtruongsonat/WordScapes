using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreatorManager : MonoBehaviour
{
    private static CreatorManager _instance;
    public static CreatorManager Instance {  get { return _instance; } }

    [Header("Data For Level")]
    public int numRows;
    public int numCols;
    public string listLetter;
    public GameObject objListLetter;
    public LevelData levelData;

    [Header("Input Field")]
    public TMP_InputField inputNumRows;
    public TMP_InputField inputNumCols;
    public TMP_InputField inputMaxLetter;
    public TMP_InputField inputKeyword;

    [Header("Button")]
    public Button searchWordButton;
    public Button resetButton;

    [Header("List Words")]
    public List<string> selectedWords;
    public List<string> availabelWords;
    public ListWordScroll objSelectedWords;
    public ListWordScroll objAvailabelWords;

    [Header("Data")]
    public ExtraWordData extraWordData;

    public void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this);
        } else
        {
            _instance = this;
        }
    }
    public void Start()
    {
        searchWordButton.onClick.AddListener(() => SearchWord());
        resetButton.onClick.AddListener(() => { objSelectedWords.RemoveList(); });
    }

    // Choose the word with the appropriate number of letters
    public void Search()
    {
        int maxLetter;
        if(inputMaxLetter.text != "")
        {
            maxLetter = int.Parse(inputMaxLetter.text);
        } else
        {
            maxLetter = 5;
            inputMaxLetter.text = "5";
        }
        availabelWords.Clear();

        Dictionary<char, int> letterCount = new Dictionary<char, int>();
        foreach(char c in listLetter)
        {
            if (letterCount.ContainsKey(c))
            {
                letterCount[c]++;
            }else
            {
                letterCount[c] = 1;
            }
        }

        Dictionary<char, int> tmp = new Dictionary<char, int>();
        foreach (string word in extraWordData.listWords)
        {
            if (word.Length <= maxLetter && !selectedWords.Contains(word))
            {
                foreach(var k in letterCount)
                {
                    tmp[k.Key] = k.Value;
                }
                int numLetterMiss = 0;
                foreach (char c in word)
                {
                    if (tmp.ContainsKey(c) && tmp[c] > 0)
                    {
                        tmp[c]--;
                    }
                    else
                    {
                        numLetterMiss++;
                    }
                }
                if (numLetterMiss <= maxLetter - listLetter.Length)
                {
                    availabelWords.Add(word);
                }
            }
        }
    }

    public void SearchWord()
    {
        string keyword = (inputKeyword.text).ToUpper();
        var result = new List<string>();
        Search();
        foreach (string word in availabelWords)
        {
            if (word.Contains(keyword))
            {
                result.Add(word);
            }
        }
        availabelWords = result;

        for (int i = 0; i < availabelWords.Count; i++)
        {
            int randIndex = UnityEngine.Random.Range(0, availabelWords.Count - 1);
            string tmp = availabelWords[randIndex];
            availabelWords[randIndex] = availabelWords[i];
            availabelWords[i] = tmp;
        }

        objAvailabelWords.UpdateList();
    }

    public void UpdateListLetter()
    {
        selectedWords = objSelectedWords.wordList; //Update Selected Word

        Dictionary<char, int> letterCount = new Dictionary<char, int>();
        foreach (string word in selectedWords)
        {
            Dictionary<char, int> curLetterCount = new Dictionary<char, int>();
            foreach (char letter in word)
            {
                if(curLetterCount.ContainsKey(letter)){
                    curLetterCount[letter]++;
                } else
                {
                    curLetterCount[letter] = 1;
                }
            }
            foreach (var k in curLetterCount)
            {
                if (letterCount.ContainsKey(k.Key))
                {
                    letterCount[k.Key] = Math.Max(letterCount[k.Key], k.Value);
                } else
                {
                    letterCount[k.Key] = k.Value;
                }
            }
        }
        listLetter = "";
        foreach(var k in letterCount)
        {
            for(int i = 0; i < k.Value; i++)
            {
                listLetter += k.Key;
            }
        }
        objListLetter.GetComponent<TextMeshProUGUI>().text = listLetter;
        SearchWord();
    }

    public void SortGird()
    {
        Dictionary<string, Word> dicWords; // Save Word Struct (word, firstPos, direction) by key is string word
        Dictionary<char, List<Tuple<int, int>>> posChar; // Save List Position of char
        
        


        levelData = CreateNewLevel();
    }

    private LevelData CreateNewLevel()
    {
        LevelData levelDataTmp = ScriptableObject.CreateInstance<LevelData>();
        if (inputNumRows.text != "")
        {
            levelDataTmp.numRow = numRows;
        }
        else levelDataTmp.numRow = 8;
        if (inputNumCols.text != "")
        {
            levelDataTmp.numCol = numCols;
        } else levelDataTmp.numCol = 8;

        levelDataTmp.letters = listLetter;
        return levelData;
    }
}
