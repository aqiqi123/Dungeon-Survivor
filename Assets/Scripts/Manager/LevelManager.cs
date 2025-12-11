using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance {  get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentExperience = 0;
    [SerializeField] private int experienceToNextLevel = 100;
    [SerializeField] private float experienceGrowthMultiplier = 1.2f;

    //参数：当前经验、升级所需总经验、当前等级
    public event Action<int,int,int> OnExperienceChanged;
    public event Action<int> OnLevelUp;

    private void Awake() {
        if (Instance == null)Instance = this;
        else Destroy(gameObject);
    }

    private void Start() {
        UpdateUI();
    }

    public void AddExperience(int amount) {
        currentExperience += amount;

        while (currentExperience >= experienceToNextLevel) {
            currentExperience -= experienceToNextLevel;
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp() {
        currentLevel++;

        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * experienceGrowthMultiplier);

        OnLevelUp?.Invoke(currentLevel);
    }

    private void UpdateUI() {
        OnExperienceChanged?.Invoke(currentExperience, experienceToNextLevel, currentLevel);
    }

    public void GetCurrentLevelStatus(out int level, out int currentExp, out int maxExp) {
        level = currentLevel;
        currentExp = currentExperience;
        maxExp = experienceToNextLevel;
    }
}
