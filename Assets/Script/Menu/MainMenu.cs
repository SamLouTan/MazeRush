using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _subMainMenu;
    [SerializeField]
    private GameObject _subSettingsMenu;
    [SerializeField] 
    private GameObject _loadingScreen;
    [SerializeField] 
    private GameObject _levelSelector;
    
    [SerializeField] private AsyncLoaderManager _asyncLoaderManager;
    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _subMainMenu.SetActive(false);
        _subSettingsMenu.SetActive(false);
        _loadingScreen.SetActive(false);
        _levelSelector.SetActive(false);

    }

    public void StartGame()
    {
        _subMainMenu.SetActive(false);
        _levelSelector.SetActive(false);
        _loadingScreen.SetActive(true);
        _asyncLoaderManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        _subMainMenu.SetActive(false);
        _subSettingsMenu.SetActive(false);
        _levelSelector.SetActive(false);
        _mainMenu.SetActive(true);
    }
    public void PlayGame()
    {
        //display sub menu ui 
       _subMainMenu.SetActive(true);
       _levelSelector.SetActive(false);
       _mainMenu.SetActive(false);
    }

    public void Settings()
    {
        // display controls settings from input manager
        Time.timeScale = 0;
        _subSettingsMenu.SetActive(true);
        _subMainMenu.SetActive(false);
        _mainMenu.SetActive(false);
        _levelSelector.SetActive(false);
        
    }

    public void LevelSelector()
    {
        //Display Level Selector
        _subMainMenu.SetActive(false);
        _mainMenu.SetActive(false);
        _levelSelector.SetActive(true);
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit(1);
    }
    
    
}
    