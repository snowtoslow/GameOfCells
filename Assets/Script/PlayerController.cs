using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Enviroments userEnviornment;

    private float _sizeOfStep = 0;
    private BoxCollider2D _boxCollider2D;
    private bool _isInputUserActive;
    private int _currentPositionI;
    private int _currentPositionJ;
    private Vector3[,] _cellsPosition;

    private bool _userChangesEnviornment;

    private void Start()
    {
        _userChangesEnviornment = false;
        _isInputUserActive = false;
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (_isInputUserActive)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _boxCollider2D.enabled = true;
            }
            else if (Input.GetKeyUp(KeyCode.Z))
            {
                _boxCollider2D.enabled = false;
            }
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _currentPositionI--;
                RefreshPositionOfPlayer();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                _currentPositionI++;
                RefreshPositionOfPlayer();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _currentPositionJ++;
                RefreshPositionOfPlayer();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                _currentPositionJ--;
                RefreshPositionOfPlayer();
            }
        }
    }

    public void SetUserEnviornment(int numberOfEnviornment)
    {
        _sizeOfStep = Instatiator.CellSize / 2;
        Debug.Log(_sizeOfStep);
        userEnviornment = (Enviroments)numberOfEnviornment;
    }

    public void SetIfUserCanInput(bool isUserCanInput, Vector3[,] cellsPositions)
    {
        _isInputUserActive = isUserCanInput;
        _cellsPosition = cellsPositions;
        
        _currentPositionI = _cellsPosition.GetLength(0) / 2;
        _currentPositionJ = _cellsPosition.GetLength(1) / 2;
        RefreshPositionOfPlayer();
    }

    public void SetIfUserChangesEnvionrment(bool isChanging)
    {
        _userChangesEnviornment = isChanging;
    }

    private void RefreshPositionOfPlayer()
    {
        transform.position = _cellsPosition[_currentPositionI, _currentPositionJ];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cell"))
        {
            if (_userChangesEnviornment)
            {
                other.GetComponent<GridCell>().SetCellEnviornment(userEnviornment);
            }
            else
            {
                other.GetComponent<GridCell>().SetBackgroundEnviornment(userEnviornment);
            }
        }
    }
}