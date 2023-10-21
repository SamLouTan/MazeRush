using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class InGameMenuManager : MonoBehaviour
{
    public GameObject inGameMenu;
    public GameObject _player;
    private InputManager _inputManager;
    private GameObject pauseMenu;
    private GameObject resumeButton;
    private GameObject settings;
    private GameObject controls;
    private GameObject mainMenu;
    private bool _isMenuOpen;
    private bool _isInSettings;

    private void Start()
    {
        _isMenuOpen = false;
        inGameMenu.SetActive(true);
        _inputManager = _player.GetComponent<InputManager>();
        pauseMenu = inGameMenu.transform.GetChild(0).GetChild(0).gameObject;
        resumeButton = pauseMenu.transform.GetChild(1).gameObject;
        resumeButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            pauseMenu.SetActive(false);
            _inputManager.DisplayMenu = false;
        });
        settings = pauseMenu.transform.GetChild(2).gameObject;
        settings.GetComponent<Button>().onClick.AddListener(() =>
        {
            pauseMenu.gameObject.SetActive(false);
            _isInSettings= true;
            inGameMenu.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            controls = inGameMenu.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;


            controls.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(() =>
            {
                controls.transform.parent.gameObject.SetActive(false);
                pauseMenu.gameObject.SetActive(true);
                _isInSettings = false;
            });
        });
        mainMenu = pauseMenu.transform.GetChild(3).gameObject;
        mainMenu.GetComponent<Button>().onClick.AddListener(() =>
        {
            Player player = _player.GetComponent<Player>();
            SaveManager.SavePlayer(player);
          if(GetComponent<MazeGenerator>() != null)  SaveManager.SaveMaze(GetComponent<MazeGenerator>());
            SceneManager.LoadScene(0);
        });
    }


    // Update is called once per frame
    void Update()
    {
        if (_inputManager.DisplayMenu)
        {
            Time.timeScale = 0;
            inGameMenu.SetActive(true);
            if(_isInSettings==false) pauseMenu.SetActive(true);
        }
        else
        {
            inGameMenu.gameObject.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }
}