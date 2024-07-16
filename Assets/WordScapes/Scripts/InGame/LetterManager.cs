using System.Collections.Generic;
using UnityEngine;

public class LetterManager : SingletonBase<LetterManager>
{
    [Header("Letter")]
    public GameObject letterCellPrefabs;
    public List<GameObject> listLetterCell;
    public string letters;

    [Header("Circle Properties")]
    public float radius;
    public int numLetter;
    public float angle;
    [Space]
    private Vector3 scaleLetter;


    public void LoadNewLevel()
    {
        letters = LevelManager.Instance.levelData.letters;

        numLetter = letters.Length;
        angle = (Mathf.PI * 2) / numLetter;
        float scaleValue = (numLetter > 5) ? 0.45f : 0.5f;
        scaleLetter = new Vector3(scaleValue, scaleValue, 1f);
        SpawnLetter();
    }

    private void SpawnLetter()
    {
        for (int i = 0; i < letters.Length; i++)
        {
            var cell = Instantiate(letterCellPrefabs,this.transform);
            SetCell(cell, i);
            listLetterCell.Add(cell);
        }
    }

    private void SetCell(GameObject cell, int index)
    {
        Vector3 position = new Vector3(Mathf.Cos(angle * index + Mathf.PI / 2), Mathf.Sin(angle * index + Mathf.PI / 2), 0) * radius;
        cell.transform.localPosition = position;
        cell.transform.localRotation = Quaternion.identity;
        cell.transform.localScale = scaleLetter;
        cell.GetComponent<InputCell>()?.SetLetter(letters[index].ToString());
    }
}
