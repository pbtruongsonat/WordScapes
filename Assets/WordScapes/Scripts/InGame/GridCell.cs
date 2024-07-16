using UnityEngine;

public class GridCell : Cell
{
    [Header("Components")]
    public GameObject squarePink;
    public GameObject squareWhite;
    public TMPro.TextMeshPro letterUI;
    
    public void SetLetter(string _letter)
    {
        letterUI.text = _letter;
    }
    public void OnSloved()
    {
        squarePink.SetActive(true);
        squareWhite.SetActive(false);
        letterUI.gameObject.SetActive(true);
    }
}
