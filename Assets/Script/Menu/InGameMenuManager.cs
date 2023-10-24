using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InGameMenuManager : MonoBehaviour
{
    public GameObject inGameMenu;
    public GameObject player;
    private InputManager _inputManager;
    private GameObject _pauseMenu;
    private GameObject _resumeButton;
    private GameObject _settings;
    private GameObject _controls;
    private GameObject _mainMenu;
    private bool _isInSettings;
    private Slider _slider;


    private void Start()
    {
        inGameMenu.SetActive(true);
        _inputManager = player.GetComponent<InputManager>();
        _pauseMenu = inGameMenu.transform.GetChild(0).GetChild(0).gameObject;
        _resumeButton = _pauseMenu.transform.GetChild(1).gameObject;
        _resumeButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            _pauseMenu.SetActive(false);
            _inputManager.DisplayMenu = false;
            
        });
        _settings = _pauseMenu.transform.GetChild(2).gameObject;
        _settings.GetComponent<Button>().onClick.AddListener(() =>
        {
            _pauseMenu.gameObject.SetActive(false);
            _isInSettings = true;
            inGameMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            _controls = inGameMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
            _slider = _controls.transform.GetChild(8).GetComponent<Slider>();
            _slider.value = PlayerPrefs.GetFloat("MouseSensitivity");
            _slider.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                System.MathF.Round(_slider.value, 2).ToString("F2");

            _slider.onValueChanged
                .AddListener(OnSensitivityChange); // add listener to slider to change mouse sensitivity

            _controls.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() =>// back button to pause menu
            {
                _controls.transform.parent.gameObject.SetActive(false);
                _pauseMenu.gameObject.SetActive(true);
                _isInSettings = false;
                PlayerPrefs.Save();
            });
        });
        _mainMenu = _pauseMenu.transform.GetChild(3).gameObject;
        _mainMenu.GetComponent<Button>().onClick.AddListener(() =>// main menu button
        {
          
            SaveManager.SavePlayer(player.GetComponent<Player>());
            if (GetComponent<MazeGenerator>() != null) SaveManager.SaveMaze(GetComponent<MazeGenerator>());
            SceneManager.LoadScene(0);
        });
    }

    private void OnSensitivityChange(float value)
    {
        _slider.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            System.MathF.Round(value, 2).ToString("F2");
        player.GetComponent<PlayerController>().ChangeMouseSensitivity(value);
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputManager.DisplayMenu)
        {
            Time.timeScale = 0; // stop time
            inGameMenu.SetActive(true);
            if (_isInSettings == false) _pauseMenu.SetActive(true); // activate pause menu if not in settings menu
        }
        else
        {
            inGameMenu.gameObject.gameObject.SetActive(false); // deactivate menu
            Time.timeScale = 1; // resume time
        }
    }
}