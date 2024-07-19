using DG.Tweening;
using UnityEngine;

public class GridCell : Cell
{
    [Header("Components")]
    public GameObject squarePink;
    public GameObject squareWhite;
    public TMPro.TextMeshPro letterUI;
    public Color pinkColor;
    
    public void SetLetter(string _letter)
    {
        letterUI.text = _letter;
    }
    public void OnSloved()
    {
        squarePink.SetActive(true);
        squareWhite.SetActive(false);
        letterUI.gameObject.SetActive(true);

        squareWhite.GetComponent<SpriteRenderer>().DOColor(pinkColor, 0.6f);
        this.gameObject.transform.localScale = Vector3.zero;
        this.gameObject.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
    }
}
