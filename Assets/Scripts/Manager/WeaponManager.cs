using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("武器挂载点")]
    [SerializeField] private Transform weaponHolder;

    // 运行时持有的所有武器逻辑脚本
    private List<WeaponBase> activeWeapons = new List<WeaponBase>();

    // 添加新武器
    public void AddWeapon(WeaponSO data) {
        if (data.WeaponPrefab == null) return;

        // 实例化武器逻辑物体
        GameObject weaponObj = Instantiate(data.WeaponPrefab, weaponHolder);

        // 获取逻辑脚本
        WeaponBase weaponScript = weaponObj.GetComponent<WeaponBase>();
        if (weaponScript != null) {
            weaponScript.Initialize(data); // 初始化为 Lv1
            activeWeapons.Add(weaponScript);
        }
    }

    // 升级已有武器
    public void LevelUpWeapon(WeaponSO data, int newLevel) {
        // 遍历找到对应的武器脚本
        foreach (var weapon in activeWeapons) {
            if (weapon.WeaponData == data) { // 比较引用
                weapon.LevelUp(newLevel);
                return;
            }
        }
    }
}
