using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridBoardManager : MonoBehaviour
{
    private static GridBoardManager _instance;

    [Header("Prefabs")]
    public GameObject tilePrefab;

    [Header("Component")]
    public GridLayoutGroup gridLayout;

    public GridBoardManager Instance { get { return _instance; } }

    [Header("Properties")]
    public LevelData leveldata;
    private int numRow;
    private int numCol;


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
        numRow = leveldata.numRow;
        numCol = leveldata.numCol;
        gridLayout.constraintCount = numCol;
        CreateEmptyGrid();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateEmptyGrid()
    {
        for(int i = 0; i < numRow*numCol; i++)
        {
            var tmp = Instantiate(tilePrefab, this.gameObject.transform);
            tmp.transform.localScale = Vector3.one;
        }
    }
}
