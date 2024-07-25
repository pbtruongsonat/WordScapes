using System;
using UnityEngine;

public class GameEvent
{
    // ----------- Support ------------------
    public static Action onClickConvertLetters;
    public static Action onClickIdea;
    public static Action onClickRocket;
    public static Action<bool> onPointerHint;

    // ----------  Resource  ----------------
    public delegate void ValueChanged(int newValue);
    public static ValueChanged coinChanged;
    public static ValueChanged diamondChanged;


    // ----------  Game  ---------------
    public static Action<bool> inGameplay;
}
