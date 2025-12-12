using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button overButton;

    private void Start() {
        panel.SetActive(false);
        overButton.onClick.AddListener(ReturnToMenu);
        resumeButton.onClick.AddListener(Resume);

        GameInput.instance.OnPauseAction += Pause;
    }

    private void OnDestroy() {
        GameInput.instance.OnPauseAction -= Pause;
    }

    private void Pause() {
        Time.timeScale = 0;
        panel.SetActive(true);
    }

    private void Resume() {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void ReturnToMenu() {
        Time.timeScale = 1f;
        Loader.Load(Loader.Scene.MainMenuScene);
    }
}
