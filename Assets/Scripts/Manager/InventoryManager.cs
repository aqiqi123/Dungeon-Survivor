using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance { get; private set; }

    [Header("限制配置")]
    public int maxWeaponSlots = 6;
    public int maxPassiveSlots = 6;

    [Header("UI 引用 (左上角图标)")]
    public List<Image> weaponSlotImages;  // 拖入 6 个 Image
    public List<Image> passiveSlotImages; // 拖入 6 个 Image
    public Sprite emptySlotIcon;          // 空槽位的默认图 (可以是透明图)

    // --- 运行时数据 ---
    // 记录由于物品SO和当前等级 (1代表Lv1)
    public Dictionary<WeaponSO, int> weaponLevels = new Dictionary<WeaponSO, int>();
    public Dictionary<PassiveItemSO, int> passiveLevels = new Dictionary<PassiveItemSO, int>();

    // 引用玩家身上的 WeaponManager
    private WeaponManager weaponManager;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // 初始化 UI：把所有图标设为默认
        ClearSlotUI();
    }

    private void Start() {
        if (PlayerStats.Instance != null) {
            weaponManager = PlayerStats.Instance.GetComponent<WeaponManager>();

            InitializeStartingWeapon();
        }
    }

    private void InitializeStartingWeapon() {
        // 1. 获取角色数据
        CharacterSO charData = PlayerStats.Instance.CharacterData;

        // 2. 检查是否有初始武器
        if (charData != null && charData.StartingWeapon != null) {
            AddOrLevelUpWeapon(charData.StartingWeapon);
        }
    }

    // --- 查询方法 (供 UpgradeManager 使用) ---

    // 获取某个武器的等级，没有则返回 0
    public int GetWeaponLevel(WeaponSO data) => weaponLevels.ContainsKey(data) ? weaponLevels[data] : 0;
    public int GetPassiveLevel(PassiveItemSO data) => passiveLevels.ContainsKey(data) ? passiveLevels[data] : 0;

    // 能不能拿新武器
    public bool CanAddWeapon() => weaponLevels.Count < maxWeaponSlots;
    public bool CanAddPassive() => passiveLevels.Count < maxPassiveSlots;

    // --- 核心操作 ---

    public void AddOrLevelUpWeapon(WeaponSO data) {
        // 1. 如果是新武器
        if (!weaponLevels.ContainsKey(data)) {
            weaponLevels.Add(data, 1);

            // 物理生成
            if (weaponManager != null) weaponManager.AddWeapon(data);

            // 更新 UI 图标
            UpdateSlotIcon(weaponSlotImages, weaponLevels.Count - 1, data.Icon);
        }
        // 2. 如果是升级
        else {
            int nextLv = weaponLevels[data] + 1;
            weaponLevels[data] = nextLv;

            // 通知逻辑升级
            if (weaponManager != null) weaponManager.LevelUpWeapon(data, nextLv);
        }
    }

    public void AddOrLevelUpPassive(PassiveItemSO data) {
        // 1. 如果是新道具
        if (!passiveLevels.ContainsKey(data)) {
            passiveLevels.Add(data, 1);

            // 应用 Lv1 属性
            PlayerStats.Instance.ApplyPassiveStats(data.LevelData[0]);

            // 更新 UI 图标
            UpdateSlotIcon(passiveSlotImages, passiveLevels.Count - 1, data.Icon);
        }
        // 2. 如果是升级
        else {
            int nextLv = passiveLevels[data] + 1;
            passiveLevels[data] = nextLv;

            // 应用升级属性
            // 假设你的 SO 数据填的是"当前等级提供的额外加成" (增量逻辑)
            PlayerStats.Instance.ApplyPassiveStats(data.LevelData[nextLv - 1]);
        }
    }

    // --- UI 辅助 ---
    private void UpdateSlotIcon(List<Image> slots, int index, Sprite icon) {
        if (index < slots.Count) {
            slots[index].sprite = icon;
            slots[index].enabled = true; // 显示图片
        }
    }

    private void ClearSlotUI() {
        foreach (var img in weaponSlotImages) { img.sprite = emptySlotIcon; img.enabled = false; }
        foreach (var img in passiveSlotImages) { img.sprite = emptySlotIcon; img.enabled = false; }
    }
}
