using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordInput : MonoBehaviour
{
    public string wordString;
    public TextMeshPro inputWord;

    public SpriteRenderer bgRenderer;
    public List<Vector3> listPosLetters = new List<Vector3>();

    private void Start()
    {
        ResetWord();
    }

    public void AddNewLetter(string letter)
    {
        wordString += letter;
        SetDisplayWord();
    }

    public void RemoveLastLetter()
    {
        wordString = wordString.Remove(wordString.Length - 1, 1);
        SetDisplayWord();
    }

    private void SetDisplayWord()
    {
        inputWord.text = wordString;

        float minWidth = 1.3f;
        bgRenderer.size = new Vector2(Mathf.Max(inputWord.preferredWidth * 2f + 0.5f, minWidth), 1.16f);
        bgRenderer.gameObject.SetActive(true);
    }

    public void FinishWord()
    {
        listPosLetters.Clear();
        for (int i = 0; i < inputWord.textInfo.characterCount; i++)
        {
            var charInfo = inputWord.textInfo.characterInfo[i];
            float posX = (charInfo.bottomLeft.x + charInfo.bottomRight.x) / 2f;
            listPosLetters.Add(new Vector3(posX, inputWord.transform.position.y, 0f));
        }

        if (LevelManager.Instance.CheckWord(wordString))
        {
            transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 1);
        }
        else
        {
            transform.DOPunchPosition(Vector3.right * 0.3f, 0.3f, 22, 1);
        }
        DOVirtual.DelayedCall(0.3f, () => { ResetWord(); });
    }

    public void ResetWord()
    {
        transform.localScale = Vector3.one;
        wordString = string.Empty;
        inputWord.text = wordString;
        bgRenderer.gameObject.SetActive(false);
    }
}
