using DG.Tweening;
using UnityEngine;

public enum CellState
{
    hidden,
    visible,
    solved
}

public class GridCell : Cell
{
    [Header("Components")]
    public int indexCell;
    public GameObject squarePink;
    public GameObject squareWhite;
    public GameObject squarePress;
    public TMPro.TextMeshPro letterUI;
    public CellState state;

    public Color pinkColor;
    public Color VisibleColor;


    public BoxCollider2D colider;

    public void SetLetter(string _letter, int indexCell)
    {
        this.indexCell = indexCell;
        letterUI.text = _letter;
        OnHidden();
    }

    public void OnHidden()
    {
        state = CellState.hidden;
        squarePink.SetActive(false);
        squareWhite.SetActive(true);
        letterUI.gameObject.SetActive(false);
    }

    public void OnVisible()
    {
        state = CellState.visible;
        letterUI.DOColor(VisibleColor, 0.1f);
        letterUI.gameObject.SetActive(true);
    }

    public void OnSolved()
    {
        state = CellState.solved;
        squarePink.SetActive(true);
        squareWhite.SetActive(false);
        if(!letterUI.gameObject.activeSelf)
            letterUI.gameObject.SetActive(true);

        letterUI.DOColor(Color.white, 0.15f);
        //squareWhite.GetComponent<SpriteRenderer>().DOColor(pinkColor, 0.6f);
        this.gameObject.transform.localScale = Vector3.zero;
        this.gameObject.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
    }

    public void OnMouseDown()
    {
        GameEvent.onPointerHint?.Invoke(false);
        //this.OnVisible();
        GameEvent.visibleCellIndex?.Invoke(indexCell);
        //GridBoardManager.Instance.CheckSlovedWord();
        DataManager.Instance.SpentPointBooster();
    }

    public void ReadyToPoint(bool value)
    {
        if(state == CellState.hidden)
        {
            squarePress.SetActive(value);
            colider.enabled = value;
        }
    }

    private void OnEnableUI(bool isEnable)
    {
        if (isEnable)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.4f);
        }
        else
        {
            transform.DOScale(Vector3.zero, 0.05f);
        }
    }

    private void OnEnable()
    {
        GameEvent.onPointerHint += ReadyToPoint;
        GameEvent.inGameplay += OnEnableUI;
    }
    private void OnDisable()
    {
        GameEvent.onPointerHint -= ReadyToPoint;
        GameEvent.inGameplay -= OnEnableUI;
    }
}
