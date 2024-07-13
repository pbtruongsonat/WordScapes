using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum FlagDir
{
    // Direction of the letter have (nothing, horizontal, vertical, both horizontal and vertical)
    N, 
    H,
    V,
    HV,
}
public class SortGrid : MonoBehaviour
{
    // Data for level data
    List<Word> words;
    Dictionary<string, Word> dicWords;
    int numRows;
    int numCols;

    Dictionary<char, List<Tuple<int, int>>> charPos;
    FlagDir[,] flagPos;
    char[,] charPlaced;
    int n = 50;

    List<string> listWord;
    private void Start()
    {
        dicWords = new Dictionary<string, Word>();
        charPos = new Dictionary<char, List<Tuple<int, int>>>();
        flagPos = new FlagDir[n, n];
        charPlaced = new char[n, n];
        listWord = new List<string>();
        words = new List<Word>();
    }
    public bool SortGridWords()
    {
        PreSortGrid();
        if (listWord.Count > 0)
        {
            PlaceFirstWord();
            if (PlaceListWord())
            {
                FomatGrid();
                CreatorManager.Instance.levelData.numCol = numCols;
                CreatorManager.Instance.levelData.numRow = numRows;
                CreatorManager.Instance.levelData.words = words;
                return true;
            }
            Debug.LogWarning("No have solution for this list word, please click SortGrid Button Again");
        } else
        {
            Debug.LogWarning("word list is empty, please add more words and try again");
        }
        return false;
    }

    // Processing Pre Sort Grid
    private void PreSortGrid()
    {
        words.Clear();
        dicWords.Clear();
        charPos.Clear();
        listWord.Clear();
        listWord = new List<string>(CreatorManager.Instance.selectedWords); // Load list word for level
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                flagPos[i,j] = FlagDir.N;
                charPlaced[i, j] = ' ';
            }
        }
    }

    // Random word in list and sort first word in the center grid
    private void PlaceFirstWord()
    {
        string word = listWord[UnityEngine.Random.Range(0, listWord.Count)]; // random word
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            TryPlaceWordHorizontal(word, 20, 20);
        }
        else
        {
            TryPlaceWordVertical(word, 20, 20);
        }
    }
    // Set Flag for cell [x,y] in matrix
    private void SetFlag(int row, int col, FlagDir flagType)
    {
        if (flagPos[row, col] == FlagDir.N)
        {
            flagPos[row, col] = flagType;
        } else
        {
            flagPos[row, col] = FlagDir.HV;
        }
    }
    // Try Place Word
    private bool TryPlaceWordHorizontal(string word, int startRow, int startCol)
    {
        // Check length
        if(word.Length + startCol >= n) { return false; }
        for(int i = 0; i < word.Length; i++)
        {
            // Check have letter ?
            if (charPlaced[startRow,startCol+i] != ' ' && charPlaced[startRow,startCol+i] != word[i]) return false;
            // Check dir ?
            if (flagPos[startRow, startCol + i] == FlagDir.H || flagPos[startRow, startCol + i] == FlagDir.HV) return false; 
        }

        Word w = new Word();
        w.word = word;
        w.dir = DirectionType.H;
        w.startRowIndex = startRow;
        w.startColIndex = startCol;

        dicWords[w.word] = w;
        listWord.Remove(word);

        for(int i = 0; i < word.Length; i++)
        {
            // Add charPos
            Tuple<int, int> pos = new Tuple<int, int>(startRow, startCol + i);
            if (charPos.ContainsKey(word[i]))
            {
                charPos[word[i]].Add(pos);
            }
            else
            {
                charPos[word[i]] = new List<Tuple<int, int>> { pos };
            }
            // Add char placed and Flag pos
            charPlaced[startRow, startCol+i] = word[i];
            SetFlag(startRow, startCol + i, FlagDir.H);
            if (i > 0 && i < word.Length - 1)
            {
                if (startRow - 1 >= 0)
                {
                    SetFlag(startRow -1, startCol +i, FlagDir.H);
                }
                if (startRow + 1 < n)
                {
                    SetFlag(startRow + 1, startCol + i, FlagDir.H);
                }
            }
        }
        if (startCol -1 >= 0)
        {
            SetFlag(startRow, startCol-1, FlagDir.H);
        }
        if (startCol + 1 < n)
        {
            SetFlag(startRow, startCol + 1, FlagDir.H);
        }
        return true;
    }
    private bool TryPlaceWordVertical(string word, int startRow, int startCol)
    {
        // Check length
        if (word.Length + startRow >= n) { return false; }
        for (int i = 0; i < word.Length; i++)
        {
            // Check have letter ?
            if (charPlaced[startRow + i, startCol] != ' ' && charPlaced[startRow + i, startCol] != word[i]) return false;
            // Check dir ?
            if (flagPos[startRow + i, startCol] == FlagDir.V || flagPos[startRow + i, startCol] == FlagDir.HV) return false;
        }

        Word w = new Word();
        w.word = word;
        w.dir = DirectionType.V;
        w.startRowIndex = startRow;
        w.startColIndex = startCol;

        dicWords[w.word] = w;
        listWord.Remove(word);

        for (int i = 0; i < word.Length; i++)
        {
            // Add charPos
            Tuple<int, int> pos = new Tuple<int, int>(startRow + i, startCol);
            if (charPos.ContainsKey(word[i]))
            {
                charPos[word[i]].Add(pos);
            }
            else
            {
                charPos[word[i]] = new List<Tuple<int, int>> { pos };
            }
            // Add char placed and Flag pos
            charPlaced[startRow + i, startCol] = word[i];
            SetFlag(startRow +i, startCol, FlagDir.V);
            if(i > 0 && i < word.Length - 1)
            {
                if(startCol-1 >= 0)
                {
                    SetFlag(startRow + i, startCol - 1, FlagDir.V);
                }
                if(startCol+1 < n)
                {
                    SetFlag(startRow + i, startCol + 1, FlagDir.V);
                }
            }
        }
        if(startRow-1 >= 0)
        {
            SetFlag(startRow -1, startCol, FlagDir.V);
        }
        if(startRow +1 < n)
        {
            SetFlag(startRow + 1, startCol, FlagDir.V);
        }
        return true;
    }

    private bool PlaceWord(string word)
    {
        for (int charIndex = 0; charIndex < word.Length; charIndex++)
        {
            char letter = word[charIndex];
            if (charPos.ContainsKey(letter))
            {
                List<Tuple<int, int>> listPos = new List<Tuple<int, int>>(charPos[letter]);
                for (int i = 0; i < listPos.Count; i++)
                {
                    int r = listPos[i].Item1;
                    int c = listPos[i].Item2;
                    if (flagPos[r, c] == FlagDir.HV) continue;
                    if (flagPos[r, c] != FlagDir.H)
                    {
                        if (TryPlaceWordHorizontal(word, r, c - charIndex)) return true;
                    }
                    if (flagPos[r, c] != FlagDir.V)
                    {
                        if(TryPlaceWordVertical(word, r - charIndex, c)) return true;
                    }
                }
            }
        }
        return false;
    }
    private bool PlaceListWord()
    {
        int counter = 0;
        while (listWord.Count > 0)
        {
            string word = listWord[UnityEngine.Random.Range(0, listWord.Count)];
            if (!PlaceWord(word)){
                counter++;
                Debug.Log($"list word: {listWord.Count}    couter: {counter}");
                if (counter >= listWord.Count - 1)
                {
                    return false;
                }
            }
            else
            {
                counter = 0;
            }
        }
        return true;
    }

    private void FomatGrid()
    {
        int firstRow = n - 1;
        int lastRow = 0;
        int firstCol = n - 1;
        int lastCol = 0;

        // Update value firstRow, lastRow, firstCol, lastCol
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (charPlaced[i, j] != ' ')
                {
                    firstRow = Math.Min(firstRow, i);
                    firstCol = Math.Min(firstCol, j);
                    lastRow = Math.Max(lastRow, i);
                    lastCol = Math.Max(lastCol, j);
                }
            }
        }
        numRows = lastRow - firstRow + 1;
        numCols = lastCol - firstCol + 1;

        foreach (var word in dicWords)
        {
            word.Value.startColIndex -= firstCol;
            word.Value.startRowIndex -= firstRow;
            words.Add(word.Value);
        }
    }
}
