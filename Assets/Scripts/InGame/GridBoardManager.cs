using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class GridBoardManager : MonoBehaviour
{
    private static GridBoardManager _instance;

    [Header("Prefabs")]
    public GameObject gridCellPrefab;

    [Header("Properties")]
    public Vector3 firstPosition;
    private List<GameObject> _cellList = new List<GameObject>();

    [Header("Data")]
    public LevelData levelData;

    public static GridBoardManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        } else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
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
    public void LoadNewLevel(LevelDataDTO newlevelData)
    {
        if(levelData == null) levelData = ScriptableObject.CreateInstance<LevelData>();
        levelData.numCol = newlevelData.numCol;
        levelData.numRow = newlevelData.numRow;
        levelData.letters = newlevelData.letters;
        levelData.words = newlevelData.words;
        LoadLevelGrid();
    }

    public void ResetGridBoard()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        gameObject.transform.localScale = Vector3.one;
        _cellList.Clear();
    }

    private void SpawnGridCell()
    {
        int totalCell = levelData.numCol * levelData.numRow;
        while (gameObject.transform.childCount < totalCell)
        {
            GameObject cell = Instantiate(gridCellPrefab, gameObject.transform);
            cell.SetActive(false );
        }

        for (int r = 0; r < levelData.numRow; r++)
        {
            for(int c = 0;  c < levelData.numCol; c++)
            {
                int index = r * levelData.numCol + c;
                _cellList.Add(gameObject.transform.GetChild(index).gameObject);

                var cell = _cellList[index];
                cell.transform.localPosition = GetCellPosition(r, c);
                cell.SetActive(false);
            }
        }
        SetGridCell();
    }

    private void SetGridCell()
    {
        int numRows = levelData.numRow;
        int numCols = levelData.numCol;

        foreach (Word word in levelData.words)
        {
            int startCol = word.startColIndex;
            int startRow = word.startRowIndex;
            int index = startRow * numCols + startCol;
            int indexIncrease;
            if(word.dir == DirectionType.H)
            {
                indexIncrease = 1; // Horizontal
            }
            else
            {
                indexIncrease = numCols; // V
            }
            for (int i = 0; i < word.word.Length; i++)
            {
                SetCell(index, word.word[i]);
                index += indexIncrease;
            }
        }
    }
    
    private void SetCell(int index, char letter)
    {
        var cell = _cellList[index];
        if (!cell.activeSelf)
        {
            cell.GetComponent<GridCell>().SetLetter(letter.ToString());
            cell.GetComponent<GridCell>().OnSloved();
            cell.SetActive(true);
        }
    }

    private Vector3 GetSquareScale()
    {
        Vector3 finalScale = Vector3.one;
        

        return finalScale;
    }
    private Vector3 GetFirstPosition()
    {
        Vector3 firstPos = Vector3.zero;
        firstPos.x = (-1.5f * (levelData.numCol - 1))/ 2f;
        firstPos.y = (1.5f * (levelData.numRow - 1))/ 2f;
        return firstPos;
    }
    private Vector3 GetCellPosition(int r, int c)
    {
        Vector3 cellPos = firstPosition;
        cellPos.x = firstPosition.x + c * 1.5f;
        cellPos.y = firstPosition.y - r * 1.5f;
        return cellPos;
    }

    private void ScaleGridBoard()
    {
        float scaleOffet = Mathf.Min(3.3f / (levelData.numCol), 3.3f/ (levelData.numRow));
        gameObject.transform.localScale = new Vector3(scaleOffet, scaleOffet, 1f);
    }
}
