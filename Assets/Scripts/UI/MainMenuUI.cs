using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    [SerializeField]private CharacterSelectUI characterSelectUI;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            characterSelectUI.OpenPanel();
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
