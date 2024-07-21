using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintButtonController : MonoBehaviour
{
    public bool isAllowHint = true;
    public void OnBoosterHintClick()
    {
        ActionEvent.onBoosterHint?.Invoke(isAllowHint);
        isAllowHint = !isAllowHint;
    }
}
