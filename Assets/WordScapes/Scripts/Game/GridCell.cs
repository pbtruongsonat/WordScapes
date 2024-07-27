using DG.Tweening;
using System;
using UnityEngine;

public enum CellState
{
    hidden,
    visible,
    sloved
}

public class GridCell : Cell
{
    [Header("Components")]
    public GameObject squarePink;
    public GameObject squareWhite;
    public GameObject squarePress;
    public TMPro.TextMeshPro letterUI;
    public CellState state;

    public Color pinkColor;
    public Color VisibleColor;


    public BoxCollider2D colider;

    public void SetLetter(string _letter)
    {
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

    public void OnSloved()
    {
        state = CellState.sloved;
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
        this.OnVisible();
        GridBoardManager.Instance.CheckSlovedWord();
    }

    public void ReadyToPoint(bool value)
    {
        if(state == CellState.hidden)
        {
            squarePress.SetActive(value);
            colider.enabled = value;
        }
    }

    private void OnEnable()
    {
        GameEvent.onPointerHint += ReadyToPoint;
    }
    private void OnDisable()
    {
        GameEvent.onPointerHint -= ReadyToPoint;
    }
}
