using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 这是一个纯数据类，用来把升级信息传给 UI
public class UpgradeOption {
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeIcon;
    public Action onSelect; // 选中这个选项后要执行的逻辑
}

public class UpgradeManager : MonoBehaviour {
    [Header("数据库 (把所有SO拖进去)")]
    public List<WeaponSO> allWeapons;
    public List<PassiveItemSO> allPassives;

    [Header("极限突破图标")]
    public Sprite goldIcon;
    public Sprite healIcon;

    [Header("引用")]
    [SerializeField] private UpgradeUI upgradeUI; // 你之前写的 UI 控制器

    // 监听升级事件
    private void Start() {
        if (LevelManager.Instance != null)
            LevelManager.Instance.OnLevelUp += OnLevelUpHandler;
    }

    private void OnLevelUpHandler(int level) {
        GenerateUpgrades();
    }

    // --- 核心算法：生成选项 ---
    public void GenerateUpgrades() {
        List<UpgradeOption> possibleOptions = new List<UpgradeOption>();

        // 1. 遍历所有武器，看看哪些能出现在升级池里
        foreach (var wp in allWeapons) {
            int currentLv = InventoryManager.Instance.GetWeaponLevel(wp);
            bool hasIt = currentLv > 0;

            // 规则A: 没拥有 且 栏位已满 -> 跳过
            if (!hasIt && !InventoryManager.Instance.CanAddWeapon()) continue;

            // 规则B: 已拥有 且 已满级 -> 跳过
            if (hasIt && currentLv >= wp.LevelData.Count) continue;

            // 生成选项数据
            int nextLv = currentLv + 1;
            // 只有当有下一级数据时才添加 (防止索引越界)
            if (nextLv <= wp.LevelData.Count) {
                WeaponStats nextStats = wp.LevelData[nextLv - 1];

                UpgradeOption option = new UpgradeOption();
                option.upgradeName = wp.WeaponName; // UI标题
                option.upgradeDescription = $"{(hasIt ? $"Level Up to LV {nextLv}" : "New Weapon!")}\n{nextStats.description}";
                option.upgradeIcon = wp.Icon;
                option.onSelect = () => {
                    InventoryManager.Instance.AddOrLevelUpWeapon(wp);
                    EndUpgradeProcess();
                };

                possibleOptions.Add(option);
            }
        }

        // 2. 遍历所有被动道具 (逻辑同上)
        foreach (var item in allPassives) {
            int currentLv = InventoryManager.Instance.GetPassiveLevel(item);
            bool hasIt = currentLv > 0;

            if (!hasIt && !InventoryManager.Instance.CanAddPassive()) continue;
            if (hasIt && currentLv >= item.LevelData.Count) continue;

            int nextLv = currentLv + 1;
            if (nextLv <= item.LevelData.Count) {
                PassiveStats nextStats = item.LevelData[nextLv - 1];

                UpgradeOption option = new UpgradeOption();
                option.upgradeName = item.ItemName;
                option.upgradeDescription = $"{(hasIt ? $"Level up to LV {nextLv}" : "New Item!")}\n{nextStats.description}";
                option.upgradeIcon = item.Icon;
                option.onSelect = () => {
                    InventoryManager.Instance.AddOrLevelUpPassive(item);
                    EndUpgradeProcess();
                };

                possibleOptions.Add(option);
            }
        }

        // 3. 极限突破 (Limit Break)
        // 如果池子里完全没东西选了（全满级了），就给一些保底奖励
        if (possibleOptions.Count == 0) {
            // 选项A: 金币
            UpgradeOption goldOption = new UpgradeOption();
            goldOption.upgradeName = "Gold Bag";
            goldOption.upgradeDescription = "Get 100 Gold";
            goldOption.upgradeIcon = goldIcon;
            goldOption.onSelect = () => {
                PlayerStats.Instance.AddGold(100);
                EndUpgradeProcess();
            };
            possibleOptions.Add(goldOption);

            // 选项B: 回血
            UpgradeOption healOption = new UpgradeOption();
            healOption.upgradeName = "Chicken";
            healOption.upgradeDescription = "Heal 30 HP";
            healOption.upgradeIcon = healIcon;
            healOption.onSelect = () => {
                PlayerStats.Instance.Heal(30);
                EndUpgradeProcess();
            };
            possibleOptions.Add(healOption);
        }

        // 4. 从可能的选项里随机抽 3 个 (洗牌)
        List<UpgradeOption> finalChoices = new List<UpgradeOption>();
        int optionsCount = possibleOptions.Count;

        // 这里的逻辑：如果有4个候选，随机选3个。如果有2个候选，就只显示2个。
        for (int i = 0; i < 3; i++) {
            if (possibleOptions.Count == 0) break;

            int randomIndex = UnityEngine.Random.Range(0, possibleOptions.Count);
            finalChoices.Add(possibleOptions[randomIndex]);

            // 移除已选的，防止重复出现同一个
            possibleOptions.RemoveAt(randomIndex);
        }

        // 5. 打开 UI
        // 记得在这里暂停游戏
        Time.timeScale = 0f;
        upgradeUI.OpenPanel(finalChoices);
    }

    private void EndUpgradeProcess() {
        // 关闭 UI 并恢复时间
        // 如果你有之前讨论的"UI排队"逻辑，这里应该调用 CheckPendingUpgrades
        Time.timeScale = 1f;
        upgradeUI.ClosePanel();
    }
}
