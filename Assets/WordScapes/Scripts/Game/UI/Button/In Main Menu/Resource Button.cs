using System.Collections;
using TMPro;
using UnityEngine;

public class ResourceButton : ButtonBase
{
    public TextMeshProUGUI textValue;
    public int value;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        textValue = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnValueChange(int newValue)
    {
        StartCoroutine(IECounter(newValue));
    }

    IEnumerator IECounter(int newValue)
    {
        int increase;

        if (value < newValue)
        {
            increase = 1;
            while (value < newValue)
            {
                value += increase;
                textValue.text = value.ToString();
                yield return null;
            }
        }
        else
        {
            increase = -1;
            while (value > newValue)
            {
                value += increase;
                textValue.text = value.ToString();
                yield return null;
            }
        }

    }


}
