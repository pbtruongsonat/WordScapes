using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class SupportManager : MonoBehaviour
{
    [Header("Button")]
    public Button convertButton;
    public Button ideaButton;
    public Button pointButton;
    public Button rocketButton;

    public List<Vector3> newPosition;

    public void Start()
    {
        convertButton.onClick.AddListener(() => ConvertLetter());
        ideaButton.onClick.AddListener(() => IdeaLetter());
        pointButton.onClick.AddListener(() => PointLetter());
        rocketButton.onClick.AddListener(() => RocketLetter());
    }

    private void ConvertLetter()
    {
        if (newPosition.Count == 0)
        {
            newPosition = new List<Vector3>();
            foreach (var letter in LetterManager.Instance.listLetterCell)
            {
                newPosition.Add(letter.transform.position);
            }
        }

        //int numConvert = UnityEngine.Random.Range(1, LetterManager.Instance.listLetterCell.Count +1);
        int numConvert = LetterManager.Instance.listLetterCell.Count;
        while (numConvert > 0)
        {
            int index1 = UnityEngine.Random.Range(0,LetterManager.Instance.listLetterCell.Count);
            int index2 = UnityEngine.Random.Range(0, LetterManager.Instance.listLetterCell.Count);

            Vector3 tmp = newPosition[index1];
            newPosition[index1] = newPosition[index2];
            newPosition[index2] = tmp;

            numConvert--;
        }

        for(int i = 0; i < LetterManager.Instance.listLetterCell.Count; i++)
        {
            if (LetterManager.Instance.listLetterCell[i].transform.position == newPosition[i]) continue;

            LetterManager.Instance.listLetterCell[i].transform.DOJump(newPosition[i], 0.2f, 0, 0.25f, false);
        }
    }
    private void IdeaLetter()
    {
        int indexLetter = UnityEngine.Random.Range(0, GridBoardManager.Instance.listLetterUnsloved.Count);
        GridBoardManager.Instance.SlovedNewLetter(GridBoardManager.Instance.listLetterUnsloved[indexLetter]);
    }
    private void PointLetter()
    {
        
    }
    private void RocketLetter()
    {
        for (int i = 0; i < 5; i++)
        {
            if (GridBoardManager.Instance.listLetterUnsloved.Count == 0) break;
            IdeaLetter();
        }
    }
}
