using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayingTimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playingTimeText;
    [SerializeField] private bool useDetailedFormat = false; // 是否显示小时

    private void Start() {
        if (playingTimeText == null) {
            Debug.LogError("GamePlayingTimeUI: playingTimeText 未分配！");
            return;
        }

        // 订阅时间更新事件（性能更好）
        if (GameTimeManager.Instance != null) {
            GameTimeManager.Instance.OnTimeUpdated += UpdateTimeDisplay;
        }
    }

    private void Update() {
        // 备用方案：如果事件系统失效，每帧更新
        if (GameTimeManager.Instance != null) {
            UpdateTimeDisplay(GameTimeManager.Instance.ElapsedTime);
        }
    }

    private void UpdateTimeDisplay(float elapsedTime) {
        if (playingTimeText != null && GameTimeManager.Instance != null) {
            playingTimeText.text = useDetailedFormat 
                ? GameTimeManager.Instance.GetDetailedFormattedTime()
                : GameTimeManager.Instance.GetFormattedTime();
        }
    }

    private void OnDestroy() {
        // 取消订阅
        if (GameTimeManager.Instance != null) {
            GameTimeManager.Instance.OnTimeUpdated -= UpdateTimeDisplay;
        }
    }
}
