using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridCell : Cell
{
    [Header("Components")]
    public GameObject squarePink;
    public GameObject squareWhite;
    public GameObject letterWhite;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void SetLetter(string _letter)
    {
        letterWhite.GetComponent<TextMeshProUGUI>().text = _letter;
    }
    public void OnSloved()
    {
        squarePink.SetActive(true);
        squareWhite.SetActive(false);
        letterWhite.SetActive(true);
    }
}
