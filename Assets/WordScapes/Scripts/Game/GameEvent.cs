using System;
using UnityEngine;

public class GameEvent
{
    // ----------- Select Level -------------
    public static Action<bool> changeChildSelect;
    public static Action<int, int,Transform, bool> displayListLevel;
    public static Action hiddenLevelContainer;


    // ----------- Support ------------------
    public static Action onClickConvertLetters;
    public static Action onClickIdea;
    public static Action onClickRocket;
    public static Action onClickPoint;
    public static Action<bool> onPointerHint;

    public static Action<int> amountIdeaChanged;
    public static Action<int> amountPointChanged;
    public static Action<int> amountRocketChanged;

    // ----------  Resource  ----------------
    public delegate void ValueChanged(int newValue);
    public static ValueChanged coinChanged;
    public static ValueChanged diamondChanged;

    // ----------  Game  ---------------
    public static Action<bool> inGameplay;
    public static Action<bool> inMainMenu;
    public static Action<bool> inSelectLevel;
    public static Action<int> playLevel;

    // ----------  Popup  ---------------

}
