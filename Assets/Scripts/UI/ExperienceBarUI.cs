using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Start() {
        if (LevelManager.Instance == null) return;

        LevelManager.Instance.OnExperienceChanged += UpdateExperienceBar;

        int startLevel, startExp, startMaxExp;
        LevelManager.Instance.GetCurrentLevelStatus(out startLevel, out startExp, out startMaxExp);
        UpdateExperienceBar(startExp, startMaxExp, startLevel);
    }

    private void OnDestroy() {
        if (LevelManager.Instance == null) return;

        LevelManager.Instance.OnExperienceChanged -= UpdateExperienceBar;
    }

    private void UpdateExperienceBar(int currentExp, int maxExp, int level) {
        float fillAmount = (float)currentExp / maxExp;

        if (fillImage != null) {
            fillImage.fillAmount = fillAmount;
        }

        if (levelText != null) {
            levelText.text = $"LV {level}";
        }
    }
}
