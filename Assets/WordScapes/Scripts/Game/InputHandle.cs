using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class InputHandle : MonoBehaviour
{
    public string word;
    public TMPro.TextMeshPro inputWord;
    public List<GameObject> cellSelected;

    public List<InputCell> inputCellSelected;

    public LineManager lineManager;
    public SpriteRenderer bgRenderer;

    public List<Vector3> listPosLetters = new List<Vector3>();

    public RectTransform positionAnchor; 

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
                listPosLetters.Clear();
                for(int i = 0; i < inputWord.textInfo.characterCount; i++)
                {
                    var charInfo = inputWord.textInfo.characterInfo[i];
                    float posX = (charInfo.bottomLeft.x + charInfo.bottomRight.x) /2f;
                    listPosLetters.Add(new Vector3(posX, inputWord.transform.position.y, 0f));
                }

                LevelManager.Instance.CheckWord(word);
               
                word = string.Empty;
                inputWord.text = word;
                bgRenderer.gameObject.SetActive(false);

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
            SetBGInputWord();

            lineManager.AddNewLine(cell.transform.position);
        }
        else if (cellSelected.Count > 1 && cellSelected[cellSelected.Count - 2] == cell)
        {
            GameObject cellRemove = cellSelected[cellSelected.Count - 1];
            cellRemove.GetComponent<InputCell>().SelectedLetter(false);
            cellSelected.Remove(cellRemove);

            word = word.Remove(word.Length - 1, 1);
            inputWord.text = word;
            SetBGInputWord();

            lineManager.RemoveLine();
        }
    }

    private void SetBGInputWord() {
        float minWidth = 1.3f;
        bgRenderer.size = new Vector2(Mathf.Max(inputWord.preferredWidth * 2f + 0.5f, minWidth), 1.16f);
        bgRenderer.gameObject.SetActive(true);
    }


     //--------------------------------------------------------------------

    private void OnEnableUI(bool isEnable)
    {
        if (isEnable)
        {
            transform.localPosition = new Vector3(positionAnchor.position.x, positionAnchor.position.y, 0);
            float scaleOffset = 1.1f * Mathf.Min(1, Camera.main.aspect / (0.5625f)); 
            // 0.5625 = (9/16) is the original screen ratio
            // 1.1 is the correction factor for larger sizes closer to the booster Button

            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(new Vector3(scaleOffset, scaleOffset, 1), 0.5f).SetEase(Ease.OutBack);
        }
        else
        {
            gameObject.transform.DOScale(Vector3.zero, 0.1f).SetEase(Ease.InFlash);
        }
    }

    private void OnEnable()
    {
        GameEvent.inGameplay += OnEnableUI;
    }

    private void OnDisable()
    {
        GameEvent.inGameplay -= OnEnableUI;
    }
}
