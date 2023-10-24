using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;


public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int CurrentRoom { get; set; } // Current level is the level that the player is currently playing

    public int
        CurrentGold
    {
        get;
        private set;
    } // Current gold is the gold that the player currently has in the game actually not implemented yet

    public Transform PlayerTransform { get; private set; }

    public int RemainingBullets { get; private set; } // 0 = no bullets, 1 = 1 bullet, 2 = 2 bullets, etc.
    public int Checkpoint { get; private set; } // 0 = no checkpoint, 1 = checkpoint 1, 2 = checkpoint 2, etc.
    public Vector3 CheckpointPosition { get; private set; } // store checkpoint position as a float array
    public int Difficulty { get; private set; } // 1= easy, 2 = normal, 3 = hard, etc.
    public int[,] UnlockedLevels { get; private set; } // 0 = locked, 1 = unlocked

    [SerializeField] private PlayerController playerController;

    [SerializeField] private TextMeshProUGUI goldText;
    private PlayerData _playerData { get; set; }

    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject endScreen;
    private Button _mainMenuButton;
    private Button _nextLevelButton; // button to go to next level
    public float Timer { get; private set; }

    private void Awake()
    {
        Difficulty = PlayerPrefs.GetInt("Difficulty");
    }

    private void Start()
    {
        _mainMenuButton = endScreen.transform.GetChild(2).GetComponent<Button>();
        _mainMenuButton.interactable = true;
        _mainMenuButton.onClick.AddListener(() => { SceneManager.LoadScene(0); });

        if (CurrentRoom != 3)
        {
            _nextLevelButton = endScreen.transform.GetChild(3).GetComponent<Button>();
            _nextLevelButton.interactable = true;
            _nextLevelButton.onClick.AddListener(() =>
            {
                InputManager inputManager = GetComponent<InputManager>();
                if (inputManager != null) inputManager.OnLevelEnding(false);
                // save player data
                SceneManager.LoadScene(CONSTANTS.SCENE_TO_LOAD[Difficulty - 1, 1]);
            });
        }


        playerController = GetComponent<PlayerController>();
        PlayerTransform = playerController.transform;
        _playerData = SaveManager.LoadPlayer();
        if (_playerData != null)
        {
            if (_playerData.CurrentRoom == 3)
            {
                CurrentRoom = _playerData.CurrentRoom;
                CurrentGold = _playerData.CurrentGold;
                if (goldText != null) goldText.text = CurrentGold.ToString();
                RemainingBullets = _playerData.RemainingBullets;
                Checkpoint = _playerData.Checkpoint;
                CheckpointPosition = new Vector3(_playerData.CheckpointPosition[0], _playerData.CheckpointPosition[1],
                    _playerData.CheckpointPosition[2]);
                Difficulty = _playerData.Difficulty;
                UnlockedLevels = _playerData.UnlockedLevels;
                PlayerTransform.position =
                    new Vector3(_playerData.Position[0], _playerData.Position[1], _playerData.Position[2]);
                PlayerTransform.rotation =
                    Quaternion.Euler(new Vector3(_playerData.Rotation[0], _playerData.Rotation[1],
                        _playerData.Rotation[2]));
                Timer = _playerData.Timer;
            }
        }
        else
        {
            CurrentGold = 0;
            RemainingBullets = 0;
            Checkpoint = 0;
            CheckpointPosition = new Vector3(0, 0, 0);
            Difficulty = PlayerPrefs.GetInt("Difficulty") != 0 ? PlayerPrefs.GetInt("Difficulty") : 1;
            UnlockedLevels = new int[3, 3];
            PlayerTransform.position = PlayerTransform.position;
            PlayerTransform.rotation = PlayerTransform.rotation;
            PlayerTransform.localScale = PlayerTransform.localScale;
            Timer = 0.0f;
        }

        endScreen.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        Timer += Time.deltaTime;
        UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int hours = Mathf.FloorToInt(Timer / 3600);
        int minutes = Mathf.FloorToInt(Timer / 60);
        int seconds = Mathf.FloorToInt(Timer % 60);

        timerText.text = String.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            CurrentGold++;
            /* getting cell position and to know on position */
            GameObject cell = other.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            MazeGenerator parent = cell.GetComponentInParent<MazeGenerator>();
            Vector2 cellPosition = parent.GetCellPosition(cell);
            parent.CoinNotOnCell[(int)cellPosition.x, (int)cellPosition.y] = true;
            Destroy(other.gameObject);
            goldText.text = CurrentGold.ToString();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            InputManager inputManager = GetComponent<InputManager>();
            if (inputManager != null) inputManager.OnLevelEnding(true);
            // stop timer,  show end screen with timer and gold 
            Time.timeScale = 0;
            timerText.gameObject.SetActive(false);
            goldText.transform.parent.gameObject.SetActive(false);
            endScreen.SetActive(true);
            TextMeshProUGUI endTimerText = endScreen.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            endTimerText.text = timerText.text;
            if (CurrentRoom == 3)
            {
                Debug.Log("Current Room: 3");
                TextMeshProUGUI endGoldText =
                    endScreen.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
                endGoldText.text = goldText.text;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 1;
            SaveManager.SavePlayer(this);
            if (CurrentRoom == 3)
            {
                SaveManager.SaveMaze(FindObjectOfType<MazeGenerator>());
            }
        }
    }
}