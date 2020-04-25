using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public GameObject unitInThisCell;

    public SpriteRenderer spriteRendererOfBackground;
    
    public Enviroments backgroundEnvionment;
    //CELL
    public Enviroments cellEnviornment;
    public int isCellActive;
    public bool isCellDevelopingSlowly;

    private SpriteRenderer _spriteRendererOfCell;
    private void Start()
    {
        _spriteRendererOfCell = unitInThisCell.transform.GetChild(0).GetComponent<SpriteRenderer>();
        ChooseBackgroundColorEnviornemnt();
        ChooseCellColorEnviornemnt();
    }

    public void SpawnCell(Enviroments cellColorEnviroments, 
        Enviroments cellBackgroundColorEnviornment, 
        int alive)
    {
        cellEnviornment = cellColorEnviroments;
        backgroundEnvionment = cellBackgroundColorEnviornment;
        isCellActive = alive;
        ChooseCellColorEnviornemnt();
        ChooseBackgroundColorEnviornemnt();
    }

    public void SetCellEnviornment(Enviroments enviroments)
    {
        isCellActive = 1;
        cellEnviornment = enviroments;
       // ChooseEnviornemnt();
        ChooseCellColorEnviornemnt(); 
        SetActiveCell();
    }
    
    public void SetBackgroundEnviornment(Enviroments enviroments)
    {
        backgroundEnvionment = enviroments;
        ChooseBackgroundColorEnviornemnt();
    }

    public void DeactivateCell()
    {
        //cellEnviornment = Enviroments.None;
        //isCellActive = 0;
        unitInThisCell.SetActive(false);
    }

    public void SetActiveCell()
    {
        unitInThisCell.SetActive(true);
    }

    private void ChooseBackgroundColorEnviornemnt()
    {
        switch (backgroundEnvionment)
        {
            case Enviroments.TargarienRed:
                spriteRendererOfBackground.color = Color.red;
                break;
            case Enviroments.StarkGrey:
                spriteRendererOfBackground.color = Color.grey;
                break;
            case Enviroments.LannisterYellow:
                spriteRendererOfBackground.color = Color.yellow;
                break;
            case Enviroments.FreePeopleBlue:
                spriteRendererOfBackground.color = Color.blue;
                break;
            case Enviroments.DeadZone:
                spriteRendererOfBackground.color = Color.black;
                break;
        }
    }
    
    private void ChooseCellColorEnviornemnt()
    {
        switch (cellEnviornment)
        {
            case Enviroments.TargarienRed:
                _spriteRendererOfCell.color = Color.red;
                break;
            case Enviroments.StarkGrey:
                _spriteRendererOfCell.color = Color.grey;
                break;
            case Enviroments.LannisterYellow:
                _spriteRendererOfCell.color = Color.yellow;
                break;
            case Enviroments.FreePeopleBlue:
                _spriteRendererOfCell.color = Color.blue;
                break;
            case Enviroments.DeadZone:
                _spriteRendererOfCell.color = Color.black;
                break;
        }
    }

    public bool GetIfCellEnviornmentEqualsWithBack()
    {
        if (cellEnviornment == backgroundEnvionment)
            return true;
        return false;
    }

    public void SetIfCellDevelopingSlowly(bool isSlowly)
    {
        isCellDevelopingSlowly = isSlowly;
    }
}
