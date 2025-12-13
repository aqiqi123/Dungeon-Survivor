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

        // 清理对象池缓存
        if (ObjectPoolManager.Instance != null) {
            ObjectPoolManager.Instance.ClearAllPools();
        }

        // 清理所有 DOTween 动画
        DG.Tweening.DOTween.KillAll();

        Loader.Load(Loader.Scene.MainMenuScene);
    }
}
