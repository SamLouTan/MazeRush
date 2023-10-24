using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NbEnemy : MonoBehaviour
{
    private GameObject[] enemies;
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject homeIcon;
    [SerializeField] private GameObject nextLevelIcon;
    [SerializeField] private TextMeshProUGUI timerText;
    private PlayerData player;

    private float _timer;
    private int currentRoom;
    private int _difficulty;
    // Start is called before the first frame update
    void Start()
    {
        
        player = SaveManager.LoadPlayer();
        if (player != null)
        {
            _timer = player.Timer;
            _difficulty= player.Difficulty;
        }
        else
        {
            _timer = 0;
            _difficulty = PlayerPrefs.GetInt("Difficulty") != 0 ? PlayerPrefs.GetInt("Difficulty") : 1;
        }
        currentRoom =2;
        homeIcon.GetComponent<Button>().interactable = true;
        homeIcon.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene(0); });
        nextLevelIcon.GetComponent<Button>().interactable = true;
        nextLevelIcon.GetComponent<Button>().onClick.AddListener(() =>
        {
            InputManager inputManager =FindObjectOfType<InputManager>();
            if (inputManager != null) inputManager.OnLevelEnding(false);
            SceneManager.LoadScene(CONSTANTS.SCENE_TO_LOAD[_difficulty - 1, 2]);
        });
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (player != null)
        {
            if (player.CurrentRoom == 2)
            {
                EndingLevelUI();

            }
        }
        if (enemies.Length == 0)
        {
            EndingLevelUI();
            
        }
        else
        {
            enemyCountText.text = "Enem" + (enemies.Length == 1 ? "y" : "ies") + " remainning : " + enemies.Length;
        }

        _timer += Time.deltaTime;
        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int hours = Mathf.FloorToInt(_timer / 3600F);
        int minutes = Mathf.FloorToInt(_timer / 60F);
        int seconds = Mathf.FloorToInt(_timer - minutes * 60);
        string niceTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        timerText.text = niceTime;
    }

    private void EndingLevelUI()
    {
        Time.timeScale = 0;
        enemyCountText.gameObject.SetActive(false);
        endScreen.SetActive(true);
        endScreen.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = timerText.text;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InputManager inputManager =FindObjectOfType<InputManager>();
        if (inputManager != null) inputManager.OnLevelEnding(true);
    }
}