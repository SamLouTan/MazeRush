using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;


public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private GameObject _inGameMenuPrefab;
    [SerializeField] private int _mazeWidth;
    [SerializeField] private int _mazeDepth;

    [SerializeField] private GameObject _player;
    [SerializeField] private AudioClip[] _jumpscare;
    private MazeCell[,] _mazeGrid;
    public int MazeWidth => _mazeWidth;
    public int MazeDepth => _mazeDepth;

    private AudioSource audioSource;
    private Random _random;
    public int Difficulty { get; private set; }
    
    public int MazeSeed { get; private set; } = 0;

    public bool[,] CoinNotOnCell { get; private set; }

    private void Awake()
    {
     
        MazeSeed = (int)DateTime.Now.Ticks;
        
        MazeData mazeData = SaveManager.LoadMaze();
        Difficulty = PlayerPrefs.GetInt("Difficulty") != 0 ? PlayerPrefs.GetInt("Difficulty") : 1;
        _mazeDepth *=Difficulty;
        _mazeWidth *=Difficulty;
        CoinNotOnCell = new bool[_mazeWidth, _mazeDepth];
        if (mazeData != null)
        {
            MazeSeed = mazeData.MazeSeed;
            _mazeWidth = mazeData.MazeWidth;
            _mazeDepth = mazeData.MazeDepth;
            CoinNotOnCell = mazeData.CoinNotOnCell;
            Difficulty = mazeData.Difficulty;
        }
    }

    private void Start()
    {
        _random = new Random(MazeSeed);
        audioSource = this.AddComponent<AudioSource>();
        MazeInit();
        Debug.Log($"Maze Seed: {MazeSeed}");
    }

    private void MazeInit()
    {
        Debug.Log(_mazeDepth);
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                _mazeGrid[x, z].name = $"Maze Cell ({x}, {z})";
                _mazeGrid[x, z].transform.parent = transform;
                Debug.Log(CoinNotOnCell[x, z]);
                _mazeGrid[x, z].Coin.SetActive(!CoinNotOnCell[x, z]);
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);
        _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.GetChild(0).tag = "Finish";
        GameObject player = Instantiate(_player, new Vector3(-0.5f, 0.3f, 0.5f), Quaternion.identity);
        player.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        InGameMenuManager inGameMenuManager = GetComponent<InGameMenuManager>();
        inGameMenuManager._player = player;
        player.GetComponent<Player>().CurrentRoom = 3;
        inGameMenuManager.inGameMenu = Instantiate(_inGameMenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        inGameMenuManager.inGameMenu.SetActive(true);
        

        StartCoroutine(PlayRandomSound());
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
        } while (nextCell != null);
    }

    private void Update()
    {
    }

    IEnumerator PlayRandomSound()
    {
        while (audioSource.isPlaying) yield return null;
        float randomNumber = new Random().Next(10, 60);
        yield return new WaitForSeconds(randomNumber);
        audioSource.clip = _jumpscare[new Random().Next(1, _jumpscare.Length)];
        audioSource.spatialize = true;
        audioSource.volume = 0.3f;
        audioSource.Play();
        StartCoroutine(PlayRandomSound());
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => _random.Next(1, 10)).FirstOrDefault();
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

        if (z - 1 >= 0)
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

    public Vector2 GetCellPosition(GameObject cell)
    {
        Debug.Log(cell.name);
        // find number in cell name with regex
        Regex regex = new Regex(@"\d");

        Match regexResult = regex.Match(cell.name);
        Debug.Log(regexResult.Value);
        return new Vector2(int.Parse(regexResult.Value), int.Parse(regexResult.NextMatch().Value));
    }
}