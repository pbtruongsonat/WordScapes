using System.Collections.Generic;
using UnityEngine;

public class InputHandle : MonoBehaviour
{
    public string word;
    public TMPro.TextMeshPro inputWord;
    public List<GameObject> cellSelected;

    public List<InputCell> inputCellSelected;

    public LineManager lineManager;

    private void Awake()
    {
        lineManager = gameObject.GetComponentInChildren<LineManager>();
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (rayHit.collider == null) return;
            if (rayHit.collider.gameObject.TryGetComponent<InputCell>(out var cell))
            {
                DetectCell(cell.gameObject);
            }
        } else 
        {
            if (word != string.Empty)
            {
                lineManager.ClearLine();

                LevelManager.Instance.CheckWord(word);
                word = string.Empty;
                inputWord.text = word;
                foreach (var inputCell in inputCellSelected)
                {
                    inputCell.SelectedLetter(false);
                }
                cellSelected.Clear();
                inputCellSelected.Clear();
            }
        }
    }

    public void DetectCell(GameObject cell)
    {
        if (!cellSelected.Contains(cell))
        {
            InputCell inputCell = cell.GetComponent<InputCell>();
            inputCell.SelectedLetter(true);
            inputCellSelected.Add(inputCell);
            cellSelected.Add(cell);

            word += inputCell.letter;
            inputWord.text = word;

            lineManager.AddNewLine(cell.transform.position);
        }
        else if (cellSelected.Count > 1 && cellSelected[cellSelected.Count - 2] == cell)
        {
            GameObject cellRemove = cellSelected[cellSelected.Count - 1];
            cellRemove.GetComponent<InputCell>().SelectedLetter(false);
            cellSelected.Remove(cellRemove);

            word = word.Remove(word.Length - 1, 1);
            inputWord.text = word;

            lineManager.RemoveLine();
        }
    }

}
