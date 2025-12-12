using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TotalCoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalCoinText;

    private void Start() {
        UpdateCoinText();
    }

    private void UpdateCoinText() {
        if (totalCoinText!=null) {
            int currentTotalGold = CharacterManager.GetTotalGold();

            totalCoinText.text=currentTotalGold.ToString();
        }
    }
}
