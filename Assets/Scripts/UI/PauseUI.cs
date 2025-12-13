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
        int runGold = PlayerStats.Instance.CurrentGold;

        CharacterManager.AddTotalGold(runGold);

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
