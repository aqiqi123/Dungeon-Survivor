using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("[SceneTransition]");
                instance = go.AddComponent<SceneTransition>();
                instance.CreateOverlay();
            }
            return instance;
        }
    }
    private static SceneTransition instance;

    [Header("过渡动画")]
    [SerializeField] private CanvasGroup fadeOverlay;    
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    [Header("选项")]
    [SerializeField] private bool blockClicksWhileFaded = true;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartTransition(Action onFadedIn)
    {
        StartCoroutine(FadeInRoutine(onFadedIn));
    }

    public void FadeOutAndDestroy()
    {
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeInRoutine(Action onFadedIn)
    {
        fadeOverlay.alpha = 0f;
        fadeOverlay.DOFade(1f, fadeInDuration);         
        yield return new WaitForSeconds(fadeInDuration); 

        onFadedIn?.Invoke(); 
    }

    private IEnumerator FadeOutRoutine()
    {
        fadeOverlay.DOFade(0f, fadeOutDuration); 
        yield return new WaitForSeconds(fadeOutDuration);

        Destroy(gameObject);                            
        instance = null;                               
    }

    private void CreateOverlay()
    {
        var canvasGO = new GameObject("FadeOverlay",
            typeof(Canvas), typeof(CanvasGroup), typeof(GraphicRaycaster));

        canvasGO.transform.SetParent(transform);

        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; 

        var image = canvasGO.AddComponent<Image>();
        image.color = Color.black;
        image.raycastTarget = blockClicksWhileFaded;

        fadeOverlay = canvasGO.GetComponent<CanvasGroup>();
        fadeOverlay.alpha = 0f;
    }
}
