using System;

public class GameEvent
{
    // ----------- Select Level -------------
    public static Action<bool> changeChildSelect;
    public static Action<ChildCategory, bool> displayListLevel;


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
    public static Action<bool> inMainMenu;
    public static Action<bool> inSelectLevel;
    public static Action<int> playLevel;
}
