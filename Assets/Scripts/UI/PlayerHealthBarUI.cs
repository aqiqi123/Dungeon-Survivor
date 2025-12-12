using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Canvas canvas;

    private HealthSystem targetHealth;

    private void Start() {
        if(PlayerStats.Instance != null) {
            targetHealth=PlayerStats.Instance.GetComponent<HealthSystem>();

            if(targetHealth != null) {
                targetHealth.OnHealthChanged += UpdateHealthUI;

                UpdateHealthUI(targetHealth.currentHealth, targetHealth.maxHealth);
            }
        }
    }

    private void OnDestroy() {
        if (targetHealth!=null) {
            targetHealth.OnHealthChanged -= UpdateHealthUI;
        }
    }

    private void UpdateHealthUI(float current, float max) {
        float fillAmount = current / max;

        fillImage.fillAmount=fillAmount;

        if (fillAmount >=0.99999f) {
            canvas.enabled = false;
        } else {
            canvas.enabled = true;
        }
    }

    private void LateUpdate() {
        transform.rotation = Quaternion.identity;
    }
}
