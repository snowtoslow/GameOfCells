using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Instatiator : MonoBehaviour
{
    public static bool IsGameStart;
    public static float CellSize;
    
    
    
    public PlayerController playerController;
    public int gridHeight;
    public GameObject cellTemplate;
    public GameObject gridCellTemplate;
    public float generationInterval = 0.3f;
    [Header("Set Number Of Dead Zones")]
    public int countDeadZone = 300;
    
    
    private GameObject[,] _cellsArray;
    private GridCell[,] _gridCellsArray;
    private int _gridWidth;



    private void Start()
    {
        SetInitalArray();
        RenderGrid();
        
        playerController.SetIfUserCanInput(true, GetArrayOfCellPositions());
    }
    
    private void Update()
    {
        if (IsGameStart)
        {
            StartGame();
            IsGameStart = false;
            playerController.SetIfUserCanInput(false, GetArrayOfCellPositions());
        }
    }

    private void SetInitalArray()
    {
        int enviornment = 0;
        int count = 0;
        int currentLimit = (gridHeight * _gridWidth)/4;
        Debug.Log(currentLimit);
        _gridWidth = Mathf.RoundToInt(gridHeight * Camera.main.aspect);
        _cellsArray = new GameObject[gridHeight,_gridWidth];
        _gridCellsArray = new GridCell[gridHeight, _gridWidth];


        CellSize = (Camera.main.orthographicSize * 2) / gridHeight;//size of a cell

        print("DIMINESIONS: "+ _gridWidth + "X" + gridHeight);
        print("CELL SIZE: "+ CellSize);
        Enviroments randomEnviornment = (Enviroments) Random.Range(0, 4);
        for (int i = 0; i < gridHeight; i++)
        {
            int timesOfSameRandom = Random.Range(20, 80);
            for (int j = 0; j < _gridWidth; j++)
            {
                if (timesOfSameRandom <= 0)
                {
                    randomEnviornment = (Enviroments) Random.Range(0, 4);
                    timesOfSameRandom = Random.Range(20, 80);
                }
                timesOfSameRandom--;
                _cellsArray[i, j] = Instantiate(gridCellTemplate);
                _gridCellsArray[i, j] = _cellsArray[i, j].GetComponent<GridCell>();
                count++;
                if (count == currentLimit)
                {
                    enviornment++;
                    count = 0;
                }
                _gridCellsArray[i, j].SetBackgroundEnviornment(randomEnviornment);
            }
        }
        
        
        for (int i = 0; i < countDeadZone; i++)
        {
            _gridCellsArray[Random.Range(0, gridHeight), Random.Range(0, _gridWidth)].
                SetBackgroundEnviornment(Enviroments.DeadZone);
        }
    }

    private Vector3[,] GetArrayOfCellPositions()
    {
        Vector3[,] cellsPositions = new Vector3[gridHeight, _gridWidth];
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                cellsPositions[i, j] = _cellsArray[i, j].transform.position;
            }
        }

        return cellsPositions;
    }

    public void StartGame()
    {
        playerController.gameObject.SetActive(false);
        InvokeRepeating("NewGenerationUpdate", generationInterval, generationInterval);
    }

    void NewGenerationUpdate(){
        
        ApplyRules();
        RenderCells();
    }

    void RenderCells(){
        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                _gridCellsArray[i, j].DeactivateCell();
            }
        }

        for (int i = 0; i < gridHeight; i++){
            
            for (int j = 0; j < _gridWidth; j++){
                
                if(_gridCellsArray[i, j].isCellActive == 0) continue;
                
                Vector3 cellPosition = new Vector3(
                    j * CellSize + CellSize / 2 ,
                    (CellSize * gridHeight)-(i*CellSize + CellSize/2),
                    0
                );
                _cellsArray[i, j].transform.position = cellPosition;
                _gridCellsArray[i, j].SetActiveCell();
            }
        }
    }

    void RenderGrid()
    {
        for (int i = 0; i < gridHeight; i++){
            
            for (int j = 0; j < _gridWidth; j++){
                Vector3 cellPosition = new Vector3(
                    j * CellSize + CellSize/2 ,
                    (CellSize * gridHeight)-(i*CellSize + CellSize/2),
                    0
                );//cell position in world coordinates;
                _cellsArray[i, j].transform.position = cellPosition;
                //Instantiate(cellTemplate, cellPosition, Quaternion.identity, transform);
            }
        }
    }

     private void ApplyRules(){
        int[,] NextGenGrid = new int[gridHeight,_gridWidth];
        Enviroments[,] nextCellColor = new Enviroments[gridHeight,_gridWidth];
        Enviroments[,] nextEnviornmentColor = new Enviroments[gridHeight,_gridWidth];

        for (int i = 0; i < gridHeight; i++){
            for (int j = 0; j < _gridWidth; j++)
            {
                int livingNeighbours = 0;
                
                GridCell currentCell = _gridCellsArray[i, j];
                Enviroments auxCellEnviornment = currentCell.cellEnviornment;
                List<GridCell> cells = new List<GridCell>();
                
                if (currentCell.backgroundEnvionment == Enviroments.DeadZone)
                {
                    continue;
                }

                if (currentCell.isCellDevelopingSlowly)
                {
                    currentCell.SetIfCellDevelopingSlowly(true);
                    continue;
                }

                cells = CountLivingNeighbours(i, j);
                int livingNeighboursRed = 0;
                int livingNeighboursGrey = 0;
                int livingNeighboursYellow = 0;
                int livingNeighboursBlue = 0;

                foreach (GridCell cell in cells)
                {
                    if (cell.isCellActive == 1)
                    {
                        switch (cell.cellEnviornment)
                        {
                            case Enviroments.TargarienRed:
                                livingNeighboursRed++;
                                break;
                            case Enviroments.StarkGrey:
                                livingNeighboursGrey++;
                                break;
                            case Enviroments.LannisterYellow:
                                livingNeighboursYellow++;
                                break;
                            case Enviroments.FreePeopleBlue:
                                livingNeighboursBlue++;
                                break;
                            case Enviroments.None:
                                break;
                        }
                    }
                }

                if (livingNeighboursRed > livingNeighboursGrey &&
                    livingNeighboursRed > livingNeighboursYellow &&
                    livingNeighboursRed > livingNeighboursBlue)
                {
                    livingNeighbours = livingNeighboursRed;
                    auxCellEnviornment = Enviroments.TargarienRed;
                }
                else if (livingNeighboursGrey > livingNeighboursRed &&
                    livingNeighboursGrey > livingNeighboursYellow &&
                    livingNeighboursGrey > livingNeighboursBlue)
                {
                    livingNeighbours = livingNeighboursGrey;
                    auxCellEnviornment = Enviroments.StarkGrey;
                }
                else if (livingNeighboursYellow > livingNeighboursRed &&
                         livingNeighboursYellow > livingNeighboursGrey &&
                         livingNeighboursYellow > livingNeighboursBlue)
                {
                    livingNeighbours = livingNeighboursYellow;
                    auxCellEnviornment = Enviroments.LannisterYellow;
                }
                else if (livingNeighboursBlue > livingNeighboursRed &&
                         livingNeighboursBlue > livingNeighboursGrey &&
                         livingNeighboursBlue > livingNeighboursYellow)
                {
                    livingNeighbours = livingNeighboursBlue;
                    auxCellEnviornment = Enviroments.FreePeopleBlue;
                }

                if (livingNeighbours == 4)
                {
                    nextEnviornmentColor[i, j] = auxCellEnviornment;
                }
                else
                {
                    nextEnviornmentColor[i, j] = currentCell.backgroundEnvionment;
                }
                
                if(livingNeighbours == 3){
                    NextGenGrid[i, j] = 1;
                    nextCellColor[i, j] = auxCellEnviornment;
                }
                else if(livingNeighbours == 2){
                    NextGenGrid[i,j] = 1;
                    nextCellColor[i, j] = Enviroments.None;
                }
                else if(livingNeighbours == 1)
                {
                    NextGenGrid[i, j] = 0;
                    nextCellColor[i, j] = Enviroments.None;
                }
                else
                {
                    nextCellColor[i, j] = currentCell.cellEnviornment;
                }
            }
        }

        for (int i = 0; i < gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                GridCell cell = _gridCellsArray[i, j];
                if (cell.backgroundEnvionment == Enviroments.DeadZone)
                {
                    continue;
                }
                if (cell.GetIfCellEnviornmentEqualsWithBack())
                {
                    cell.SetIfCellDevelopingSlowly(false);
                    continue;
                }
                cell.SpawnCell(nextCellColor[i,j], 
                    nextEnviornmentColor[i, j], NextGenGrid[i, j]);
            }
        }
     }
    

    List<GridCell> CountLivingNeighbours(int i, int j){
        
        List<GridCell> result = new List<GridCell>();
        
        for (int iNeighbour = i-1; iNeighbour < i+2; iNeighbour++){
            for (int jNeighbour = j-1; jNeighbour < j+2; jNeighbour++){
                if(iNeighbour == i && jNeighbour == j) continue;
                try
                {
                    GridCell cell = _gridCellsArray[iNeighbour, jNeighbour];
                    result.Add(cell);
                }catch{
                }
            }
        }
        return result;
    }
}
