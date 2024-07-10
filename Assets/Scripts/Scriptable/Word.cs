using System;

public enum DirectionType
{
    H, //Horizontal
    V  //Vertical
}
[Serializable]
public class Word
{
    public string word;
    public int startColIndex;
    public int startRowIndex;
    public DirectionType dir;
}
