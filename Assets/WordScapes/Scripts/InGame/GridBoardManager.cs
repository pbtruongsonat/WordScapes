using System;
using System.Collections.Generic;
using UnityEngine;

public class GridBoardManager : SingletonBase<GridBoardManager>
{
    [Header("Prefabs")]
    public GameObject gridCellPrefab;

    [Header("Properties")]
    public Vector3 firstPosition;
    private List<GameObject> cellList = new List<GameObject>();

    public Dictionary<int, GameObject> cellDic = new Dictionary<int, GameObject>();
    public Dictionary<string, List<int>> wordUnSloved = new Dictionary<string, List<int>>();
    public Dictionary<int, List<string>> cellWord = new Dictionary<int, List<string>>();
    public List<int> indexCellHidden = new List<int>();

    [Header("Data")]
    public LevelData levelData;
    
    public void LoadNewLevel(LevelData levelData)
    {
        this.levelData = levelData;
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
            cellList.Add(gameObject.transform.GetChild(i).gameObject);
        }
        gameObject.transform.localScale = Vector3.one;
        cellDic.Clear();
        wordUnSloved.Clear();
        cellWord.Clear();
        indexCellHidden.Clear();
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

            if (!wordUnSloved.ContainsKey(word.word))
            {
                wordUnSloved[word.word] = new List<int>();
            }
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
                wordUnSloved[word.word].Add(index);
                if (cellDic.ContainsKey(index)) continue;

                if (cellList.Count == 0)
                {
                    var newCell = Instantiate(gridCellPrefab, gameObject.transform);
                    newCell.SetActive(false);
                    cellList.Add(newCell);
                }
                SetCell(index, word.word[i]);

                if (!cellWord.ContainsKey(index))
                {
                    cellWord.Add(index, new List<string>());
                }
                cellWord[index].Add(word.word);
            }
        }

    }
    
    private void SetCell(int index, char letter)
    {
        var cell = cellList[0];
        cellDic.Add(index, cell);
        cellList.Remove(cell);

        //
        int r = index / levelData.numCol;
        int c = index % levelData.numCol;
        cell.transform.localPosition = new Vector3(firstPosition.x + c * 1.5f, firstPosition.y - r * 1.5f, 0f);

        //
        indexCellHidden.Add(index);
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
        wordUnSloved.Remove(word.word);

        int index = word.startRowIndex * levelData.numCol + word.startColIndex;
        int indexIncrease = (word.dir == DirectionType.H) ? 1 : levelData.numCol;
        for (int i = 0; i < word.word.Length; i++)
        {
            cellDic[index].GetComponent<GridCell>()?.OnSloved();

            indexCellHidden.Remove(index);
            foreach (var w in cellWord[index])
            {
                if (wordUnSloved.ContainsKey(w))
                    wordUnSloved[w].Remove(index);
            }

            index += indexIncrease;
        }
    }

    public void VisibleOneCell()
    {
        int index = UnityEngine.Random.Range(0, indexCellHidden.Count);
        int cellindex = indexCellHidden[index];
        var cell = cellDic[cellindex];
        cell.GetComponent<GridCell>()?.OnVisible();
        indexCellHidden.Remove(cellindex);

        foreach(var k in wordUnSloved)
        {
            if (!k.Value.Contains(cellindex)) continue;

            k.Value.Remove(cellindex);
            if (k.Value.Count == 0)
            {
                SlovedNewWord(LevelManager.Instance.wordList[k.Key]);
                LevelManager.Instance.slovedWordList.Add(k.Key);
            }
        }
    }

    public void DisplaySloved()
    {
        foreach (var k in cellDic)
        {
            k.Value.GetComponent<GridCell>().OnSloved();
        }
    }

    public void OnEnable()
    {
        GameEvent.onClickIdea += VisibleOneCell;
    }

    public void OnDisable()
    {
        GameEvent.onClickIdea -= VisibleOneCell;
    }
}
