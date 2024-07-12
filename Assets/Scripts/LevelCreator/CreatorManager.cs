using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Input Field")]
    public TMP_InputField inputNumRows;
    public TMP_InputField inputNumCols;
    public TMP_InputField inputMaxLetter;
    public TMP_InputField inputKeyword;

    [Header("Button")]
    public Button searchWordButton;

    [Header("List Words")]
    public List<string> selectedWords;
    public List<string> availabelWords;
    public GameObject objSelectedWords;
    public GameObject objAvailabelWords;

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
    }

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

        foreach (string word in extraWordData.listWords)
        {
            if (word.Length <= maxLetter)
            {
                availabelWords.Add(word);
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

        UpdateAvailabelWordView();
    }

    public void UpdateListLetter()
    {

    }
    public void UpdateAvailabelWordView()
    {
        objAvailabelWords.GetComponent<ListWordScroll>().UpdateList();
    }
}
