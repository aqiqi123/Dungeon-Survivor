using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Button overButton;

    private void Start() {
        panel.SetActive(false);
        overButton.onClick.AddListener(ReturnToMenu);

        if (PlayerStats.Instance != null) {
            PlayerStats.Instance.GetComponent<HealthSystem>().OnDeath += OnPlayerDeath;
        }
    }

    private void OnPlayerDeath() {
        panel.SetActive(true);

        int runGold = PlayerStats.Instance.CurrentGold;

        CharacterManager.AddTotalGold(runGold);

        Time.timeScale = 0f;
    }

    private void ReturnToMenu() {
        Time.timeScale = 1f;
        Loader.Load(Loader.Scene.MainMenuScene);
    }
}
