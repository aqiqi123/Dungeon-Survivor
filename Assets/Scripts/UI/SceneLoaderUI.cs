using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderUI : MonoBehaviour
{
    public static event Action OnLoadComplete;

    private void Start()
    {
        StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(
            Loader.TargetScene.ToString(), LoadSceneMode.Single);

        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            yield return null;
        }

        SceneManager.sceneLoaded += OnTargetSceneLoaded;
        op.allowSceneActivation = true;
    }

    private void OnTargetSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnTargetSceneLoaded;

        OnLoadComplete?.Invoke();
        OnLoadComplete = null;

        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.FadeOutAndDestroy();
        }
    }
}
