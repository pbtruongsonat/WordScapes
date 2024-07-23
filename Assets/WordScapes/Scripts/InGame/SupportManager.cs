using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SupportManager : MonoBehaviour
{
    [Header("Button")]
    public Button convertButton;
    public Button ideaButton;
    public Button pointButton;
    public Button rocketButton;

    public bool inPointSupport;

    public List<Vector3> newPosition;

    public void Start()
    {
        convertButton.onClick.AddListener(() =>
        {
            GameEvent.onClickConvertLetters?.Invoke();
        });

        ideaButton.onClick.AddListener(() => { 
            GameEvent.onClickIdea?.Invoke(); 
        });

        pointButton.onClick.AddListener(() =>
        {
            GameEvent.onClickRocket?.Invoke();
        });

        rocketButton.onClick.AddListener(() => 
        {
            GameEvent.onClickRocket?.Invoke();
        });
    }
    private void PointLetter()
    {
        inPointSupport = !inPointSupport;
    }
}
