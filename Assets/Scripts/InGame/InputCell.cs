using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputCell : Cell
{
    [Header("Component")]
    public GameObject squarePink;
    public GameObject letterWhite;
    public GameObject letterPurple;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetLetter(string _letter)
    {
        letterWhite.GetComponent<TextMeshProUGUI>().text = _letter;
        letterPurple.GetComponent<TextMeshProUGUI>().text = _letter;
    }

}
