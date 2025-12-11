using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start() {
        if(PlayerStats.Instance != null) {
            PlayerStats.Instance.OnGoldChanged += UpdateUI;
        }

        UpdateUI();
    }

    private void OnDestroy() {
        if (PlayerStats.Instance != null) {
            PlayerStats.Instance.OnGoldChanged -= UpdateUI;
        }
    }

    private void UpdateUI() {
        if (PlayerStats.Instance == null || goldText == null) return;

        // ¸üÐÂÊý×Ö
        goldText.text = PlayerStats.Instance.CurrentGold.ToString();
    }
}
