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
    public int startRowIndex;
    public int startColIndex;
    public DirectionType dir;
}
