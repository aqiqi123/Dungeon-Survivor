using System;
using UnityEngine;

public class GameTimeManager : MonoBehaviour {
    public static GameTimeManager Instance { get; private set; }

    private float elapsedTime = 0f;
    private bool isRunning = false;

    public float ElapsedTime => elapsedTime;

    // 事件：当时间更新时触发（可选，用于UI更新）
    public event Action<float> OnTimeUpdated;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        StartTimer();
    }

    private void Update() {
        if (isRunning) {
            elapsedTime += Time.deltaTime;
            OnTimeUpdated?.Invoke(elapsedTime);
        }
    }

    public void StartTimer() {
        isRunning = true;
    }

    public void StopTimer() {
        isRunning = false;
    }

    public void ResetTimer() {
        elapsedTime = 0f;
    }

    public string GetFormattedTime() {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public string GetDetailedFormattedTime() {
        int hours = Mathf.FloorToInt(elapsedTime / 3600f);
        int minutes = Mathf.FloorToInt((elapsedTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        if (hours > 0) {
            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        } else {
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }
}
