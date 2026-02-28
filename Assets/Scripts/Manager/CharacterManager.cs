using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CharacterManager
{
    //用于跨场景传递当前选择的角色
    public static CharacterSO SelectedCharacter;

    public const string GOLD_SAVE_KEY = "TotalGold";

    public static int GetTotalGold() {
        return SaveManager.Load().totalGold;
    }

    public static void AddTotalGold(int amount) {
        SaveData data=SaveManager.Load();
        data.totalGold += amount;
        SaveManager.Save(data);
    }
}
