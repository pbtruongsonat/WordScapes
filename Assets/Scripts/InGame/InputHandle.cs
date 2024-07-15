using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandle : MonoBehaviour
{
    public string word;
    public TextMeshProUGUI inputWord;
    public List<GameObject> cellSelected;
    public Transform mouseTransform;

    public LineController lineController;

    private void Awake()
    {
        lineController = gameObject.GetComponentInChildren<LineController>();
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (rayHit.collider != null)
            {
                InputCell cell = rayHit.collider.gameObject.GetComponent<InputCell>();
                if (cell != null)
                {
                    DetectCell(cell.gameObject);
                }
            }
        } else
        {
            if (word != string.Empty)
            {
                lineController.RemovePoint();
                LevelManager.Instance.CheckWord(word);
                word = string.Empty;
                inputWord.text = word;
                foreach (GameObject cell in cellSelected)
                {
                    cell.GetComponent<InputCell>().SelectedLetter(false);
                }
                cellSelected.Clear();
            }
        }
    }

    public void DetectCell(GameObject cell)
    {
        if (!cellSelected.Contains(cell))
        {
            InputCell inputCell = cell.GetComponent<InputCell>();
            inputCell.SelectedLetter(true);
            cellSelected.Add(cell);
            word += inputCell.letter;
            inputWord.text = word;
            lineController.AddPoint(cell.transform.position);
        }
        else if (cellSelected.Count > 1 && cellSelected[cellSelected.Count - 2] == cell)
        {
            GameObject cellRemove = cellSelected[cellSelected.Count - 1];
            cellRemove.GetComponent<InputCell>().SelectedLetter(false);
            cellSelected.Remove(cellRemove);
            word = word.Remove(word.Length - 1, 1);
            inputWord.text = word;
        }
    }

}
