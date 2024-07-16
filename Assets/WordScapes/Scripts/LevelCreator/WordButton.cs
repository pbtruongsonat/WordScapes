using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordButton : MonoBehaviour
{
    public string word;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnWordClick());
    }
    public void SetWord(string _word)
    {
        this.word = _word;
        gameObject.GetComponent<TextMeshProUGUI>().text = word;
    }

    private void OnWordClick() 
    {
        GetComponentInParent<ListWordScroll>()?.RemoveWord(gameObject);
    }
}
