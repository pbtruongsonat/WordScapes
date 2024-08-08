using DG.Tweening;
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
    public Dictionary<string, List<int>> wordUnSolved = new Dictionary<string, List<int>>();

    public List<int> indexCellVisible = new List<int>();
    public List<int> indexCellHidden = new List<int>();

    [Header("Data")]
    public LevelData levelData;

    [Header("Position + Scale")]
    public RectTransform topAnchor;  
    public RectTransform botAnchor;  
    public RectTransform leftAnchor; 
    public RectTransform rightAnchor;


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
        cellList.Clear();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
            cellList.Add(gameObject.transform.GetChild(i).gameObject);
        }
        gameObject.transform.localScale = Vector3.one;

        cellDic.Clear();
        wordUnSolved.Clear();
        indexCellHidden.Clear();
        indexCellVisible.Clear();
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

            if (!wordUnSolved.ContainsKey(word.word))
            {
                wordUnSolved[word.word] = new List<int>();
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
                wordUnSolved[word.word].Add(index);
                if (cellDic.ContainsKey(index)) continue;

                if (cellList.Count == 0)
                {
                    var newCell = Instantiate(gridCellPrefab, gameObject.transform);
                    newCell.SetActive(false);
                    cellList.Add(newCell);
                }
                SetCell(index, word.word[i]);
            }
        }

        if (GameManager.Instance.currentLevel == DataManager.unlockedLevel)
        {
            LoadLevelState();
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
        cell.GetComponent<GridCell>().SetLetter(letter.ToString(), index);
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
        Canvas.ForceUpdateCanvases();

        var offset = (topAnchor.position + botAnchor.position) / 2f;
        gameObject.transform.position = new Vector3(offset.x, offset.y, 0f);

        float sizeOfCell = 1.5f;

        var maxX = levelData.numCol * sizeOfCell / 2f;
        var maxY = levelData.numRow * sizeOfCell / 2f;

        var top = maxY - topAnchor.position.y + offset.y;
        var right = maxX - rightAnchor.position.x + offset.x;
        var max = Mathf.Max(top, right);

        var scaleOffsetV = (topAnchor.position.y - offset.y) / maxY;
        var scaleOffsetH = (rightAnchor.position.x - offset.x) / maxX;

        float maxScaleOffset = 0.7f;
        var scaleOffset = Mathf.Min(scaleOffsetV, scaleOffsetH, maxScaleOffset);

        gameObject.transform.localScale = Vector3.one * scaleOffset;
    }

    // --------------------- Solved New Word ------------------
    public void SolvedNewWord(Word word)
    {
        wordUnSolved.Remove(word.word);

        int index = word.startRowIndex * levelData.numCol + word.startColIndex;
        int indexIncrease = (word.dir == DirectionType.H) ? 1 : levelData.numCol;
        for (int i = 0; i < word.word.Length; i++)
        {
            cellDic[index].GetComponent<GridCell>()?.OnSolved();

            indexCellVisible.Add(index);
            indexCellHidden.Remove(index);
            index += indexIncrease;
        }
    }

    public void SlovedWordAgain(Word word)
    {
        int index = word.startRowIndex * levelData.numCol + word.startColIndex;
        int indexIncrease;
        Vector3 dirPunch;

        if (word.dir == DirectionType.H)
        {
            indexIncrease = 1;
            dirPunch = Vector3.right * 0.3f;
        }
        else
        {
            indexIncrease = levelData.numCol;
            dirPunch = Vector3.down * 0.3f;
        }

        for (int i = 0; i < word.word.Length; i++)
        {
            cellDic[index].transform.DOPunchPosition(dirPunch, 0.6f, 10);
            index += indexIncrease;
        }
    }

    public void RandomIndexHidden()
    {
        int index = UnityEngine.Random.Range(0, indexCellHidden.Count);
        int indexCell = indexCellHidden[index];
        VisibleCellIndex(indexCell);

        CheckSlovedWord();
    }

    public void VisibleCellIndex(int indexCell)
    {
        var cell = cellDic[indexCell];
        cell.GetComponent<GridCell>()?.OnVisible();

        indexCellHidden.Remove(indexCell);
        indexCellVisible.Add(indexCell);
    }

    public void VisibleFiveCell()
    {
        for(int i = 0; i < 5; i++)
        {
            if (indexCellHidden.Count <= 0) break;
            RandomIndexHidden();
        }
    }

    public void DisplaySloved()
    {
        int cellCount = 0;
        foreach (var k in cellDic)
        {
            k.Value.GetComponent<GridCell>().OnSolved();
            cellCount++;
        }
        if(levelData != null)
            levelData.brilliancePoint = cellCount;
    }

    public void CheckSlovedWord()
    {
        Dictionary<string, List<int>> tmp = new Dictionary<string, List<int>>(wordUnSolved);
        List<string> wordsToRemove = new List<string>();
        Dictionary<string, List<int>> indicesToRemove = new Dictionary<string, List<int>>();

        foreach (var word in tmp)
        {
            List<int> indices = new List<int>();

            foreach (var index in word.Value)
            {
                if (cellDic[index].GetComponent<GridCell>().state != CellState.hidden)
                {
                    indices.Add(index);
                }
            }

            if (indices.Count > 0)
            {
                if (!indicesToRemove.ContainsKey(word.Key))
                {
                    indicesToRemove[word.Key] = new List<int>();
                }
                indicesToRemove[word.Key].AddRange(indices);
            }

            if (wordUnSolved[word.Key].Count == indices.Count)
            {
                wordsToRemove.Add(word.Key);
            }
        }

        // Remove indices
        foreach (var word in indicesToRemove)
        {
            foreach (var index in word.Value)
            {
                wordUnSolved[word.Key].Remove(index);
            }
        }

        // Remove words
        foreach (var word in wordsToRemove)
        {
            wordUnSolved.Remove(word);
            SolvedNewWord(LevelManager.Instance.wordList[word]);
            LevelManager.Instance.NewWordSloved(word);
        }
    }


    public void SaveLevelState()
    {
        if (GameManager.Instance.currentLevel == DataManager.unlockedLevel)
        {
            StateCurrentLevel stateData = new StateCurrentLevel();
            stateData.levelIndex = GameManager.Instance.currentLevel;
            stateData.indexVisible = indexCellVisible;

            DataManager.Instance.SaveStateCurLevel(stateData);
        }
    }

    public void LoadLevelState()
    {
        StateCurrentLevel stateData = DataManager.Instance.LoadStateCurLevel();
        if (stateData != null && stateData.levelIndex == GameManager.Instance.currentLevel)
        {
            foreach (int cellIndex in stateData.indexVisible)
            {
                VisibleCellIndex(cellIndex);
            }
            CheckSlovedWord();
        }
    }

    private void OnEnable()
    {
        GameEvent.visibleCellIndex += VisibleCellIndex;
    }

    private void OnDisable()
    {
        SaveLevelState();
        GameEvent.visibleCellIndex -= VisibleCellIndex;
    }
}
