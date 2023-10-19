using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private GameObject _inGameMenuPrefab;
    [SerializeField] private int _mazeWidth;
    [SerializeField] private int _mazeDepth;

    [SerializeField] private GameObject _player;
    private MazeCell[,] _mazeGrid;
    public int MazeWidth => _mazeWidth;
    public int MazeDepth => _mazeDepth;
    private void Start()
    {
        MazeInit();
    }

    private void MazeInit()
    { _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                _mazeGrid[x, z].name = $"Maze Cell ({x}, {z})";
                _mazeGrid[x, z].transform.parent = transform;
            }
        }
        GenerateMaze(null, _mazeGrid[0, 0]); 
        _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.GetChild(0).tag = "Finish";
        GameObject player = Instantiate(_player, new Vector3(-0.5f, 0.3f, 0.5f), Quaternion.identity);
        player.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        InGameMenuManager inGameMenuManager = GetComponent<InGameMenuManager>();
        inGameMenuManager._player = player;
        inGameMenuManager.inGameMenu =  Instantiate(_inGameMenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        inGameMenuManager.inGameMenu.SetActive(true);
        

    }
    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWall(previousCell, currentCell);
        MazeCell nextCell;
        System.Threading.Thread.Sleep(1);
        do
        { 
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        }while (nextCell != null);
    }



    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        
        Vector3 cellPosition = currentCell.transform.position;
        int x = (int)cellPosition.x;
        int z = (int)cellPosition.z;
        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            if (cellToRight.IsVisited == false)
            {
                yield return cellToRight;
            }
        }

        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];
            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }
        if(z-1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];
            if (cellToBack.IsVisited == false)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWall(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null)
        {
            return;
        }

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
        }
    }

    private float CalculateProgress()
    {
        int visitedCells = _mazeGrid.Cast<MazeCell>().Count(cell => cell.IsVisited);
        int totalCells = _mazeWidth * _mazeDepth;
        return (float)visitedCells / totalCells;
    }
}