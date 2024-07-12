using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ListWordScroll : MonoBehaviour
{
    public GameObject prefabWordButton;

    [Header("this List Word Scroll")]
    public Transform thisContentTransform;

    [Header("other List Word Scroll")]
    public ListWordScroll other;
    public Transform otherContentTransform;

    //update ListWord
    public void UpdateList()
    {
        while (thisContentTransform.childCount < 50)
        {
            Instantiate(prefabWordButton, thisContentTransform);
        }
        // Set word to wordButton
        for (int i = 0; i < thisContentTransform.childCount; i++)
        {
            var child = thisContentTransform.GetChild(i);
            if(i < CreatorManager.Instance.availabelWords.Count)
            {
                child.GetComponent<WordButton>().SetWord(CreatorManager.Instance.availabelWords[i]);
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    
    //Remove Word
    public void RemoveWord(GameObject wordbtn) 
    {
        var wordText = wordbtn.GetComponent<TextMeshProUGUI>().text;
        wordbtn.transform.SetParent(otherContentTransform, false);
        CreatorManager.Instance.UpdateListLetter();
    }
}
