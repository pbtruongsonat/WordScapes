using System;
using System.Collections.Generic;
using UnityEngine;

public class GridBoardManager : SingletonBase<GridBoardManager>
{
    [Header("Prefabs")]
    public GameObject gridCellPrefab;

    [Header("Properties")]
    public Vector3 firstPosition;
    private List<GameObject> _cellList = new List<GameObject>();

    private Dictionary<int, GameObject> cellDic = new Dictionary<int, GameObject>();
    public List<int> listLetterUnsloved = new List<int>();

    [Header("Data")]
    public LevelData levelData;
    
    public void LoadNewLevel(LevelData levelData)
    {
        this.levelData = levelData;
        LoadLevelGrid();
    }

    public void LoadLevelGrid()
    {
        if (levelData != null)
        {
            ResetGridBoard();
            firstPosition = GetFirstPosition();
            SpawnGridCell();
            ScaleGridBoard();
        }
    }

    public void ResetGridBoard()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
            _cellList.Add(gameObject.transform.GetChild(i).gameObject);
        }
        gameObject.transform.localScale = Vector3.one;
        cellDic.Clear();
        listLetterUnsloved.Clear();
    }

    private void SpawnGridCell()
    {
        int numRows = levelData.numRow;
        int numCols = levelData.numCol;

        foreach (Word word in levelData.words)
        {
            int startCol = word.startColIndex;
            int startRow = word.startRowIndex;
            int startIndex = startRow * numCols + startCol;
            int indexIncrease;

            if (word.dir == DirectionType.H)
            {
                indexIncrease = 1; // Horizontal
            }
            else
            {
                indexIncrease = numCols; // V
            }

            for (int i = 0; i < word.word.Length; i++)
            {
                int index = startIndex + i * indexIncrease;
                if (cellDic.ContainsKey(index)) continue;

                if(_cellList.Count == 0)
                {
                    var newCell = Instantiate(gridCellPrefab, gameObject.transform);
                    newCell.SetActive(false);
                    _cellList.Add(newCell);
                }
                SetCell(index, word.word[i]);
            }
        }

    }
    
    private void SetCell(int index, char letter)
    {
        var cell = _cellList[0];
        cellDic.Add(index, cell);
        listLetterUnsloved.Add(index);
        _cellList.Remove(cell);

        //
        int r = index / levelData.numCol;
        int c = index % levelData.numCol;
        cell.transform.localPosition = new Vector3(firstPosition.x + c * 1.5f, firstPosition.y - r * 1.5f, 0f);

        //
        cell.GetComponent<GridCell>().SetLetter(letter.ToString());
        cell.SetActive(true);
    }

    private Vector3 GetFirstPosition()
    {
        Vector3 firstPos = Vector3.zero;
        firstPos.x = (-1.5f * (levelData.numCol - 1))/ 2f;
        firstPos.y = (1.5f * (levelData.numRow - 1))/ 2f;
        return firstPos;
    }

    private void ScaleGridBoard()
    {
        float width = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float height = Camera.main.orthographicSize * 2;
        float numcell = Mathf.Min(width / 1.5f, height*0.4f /1.5f);

        float scaleOffset = Mathf.Min(numcell / (levelData.numCol), numcell / (levelData.numRow));
        gameObject.transform.localScale = new Vector3(scaleOffset, scaleOffset, 1f);
    }

    // --------------------- Sloved New Word ------------------
    public void SlovedNewWord(Word word)
    {
        int startIndexWord = word.startRowIndex * levelData.numCol + word.startColIndex;
        int indexIncrease = (word.dir == DirectionType.H) ? 1 : levelData.numCol;
        for (int i = 0; i < word.word.Length; i++)
        {
            SlovedNewLetter(startIndexWord + indexIncrease * i);
        }
    }

    public void SlovedNewLetter(int index)
    {
        if (cellDic.ContainsKey(index))
        {
            cellDic[index].GetComponent<GridCell>()?.OnSloved();
            listLetterUnsloved.Remove(index);
        }
    }

    public void DisplaySloved()
    {
        foreach (var k in cellDic)
        {
            k.Value.GetComponent<GridCell>().OnSloved();
        }
    }
}
