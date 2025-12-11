using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponStats {
    [TextArea] public string description; // 这一级的描述，如 "增加 1 个投射物"
    public float damage;
    public float cooldown;
    public float speed;
    public float duration;
    public int count;
    public float area;
    public int pierce;
    public float projectileInterval;
    public float attackInterval;
}

[CreateAssetMenu(fileName = "WeaponSO", menuName = "SOs/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("基本信息")]
    [SerializeField] private string weaponName;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject bulletPrefab;

    [Header("等级数据")]
    [SerializeField]List<WeaponStats> levelData;

    public string WeaponName => weaponName;
    public Sprite Icon => icon;
    public GameObject WeaponPrefab => weaponPrefab;
    public GameObject BulletPrefab => bulletPrefab;
    public List<WeaponStats> LevelData => levelData;
    public int MaxLevel => levelData.Count;
}
