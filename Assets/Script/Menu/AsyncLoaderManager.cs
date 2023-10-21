using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class AsyncLoaderManager : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _progressText;
    private float progress;


    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            progress = Mathf.Clamp01(asyncLoad.progress / .9f);
            _slider.value = progress * 100;
            _progressText.text = $"{progress * 100}%";
            yield return null;
        }
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);


        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading");
            progress = Mathf.Clamp01(asyncLoad.progress / .9f);
            _slider.value = progress;
            _progressText.text = $"{progress * 100}%";
            Debug.Log(progress);
            yield return null;
        }
    }

    private void Update()
    {
        _slider.value = progress;
        _progressText.text = $"{progress * 100}%";
    }
}