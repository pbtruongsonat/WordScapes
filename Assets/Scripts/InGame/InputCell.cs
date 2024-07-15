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

    public string letter;

    public void SetLetter(string _letter)
    {
        letter = _letter;
        letterWhite.GetComponent<TextMeshProUGUI>().text = _letter;
        letterPurple.GetComponent<TextMeshProUGUI>().text = _letter;
    }

    public void SelectedLetter(bool selected)
    {

        squarePink.SetActive(selected);
        letterPurple.SetActive(!selected);
    }
}
