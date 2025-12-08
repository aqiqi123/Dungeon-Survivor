using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "SOs/Weapon")]
public class WeaponSO : ScriptableObject
{
    [Header("基本信息")]
    [SerializeField] private string weaponName;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject bulletPrefab;

    [Header("基础数值")]
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [SerializeField] private float speed;
    [SerializeField] private float duration;//光环、圣经持续时间
    [SerializeField] private int count;
    [SerializeField] private float area;

    public string WeaponName => weaponName;
    public Sprite Icon => icon;
    public GameObject WeaponPrefab => weaponPrefab;
    public GameObject BulletPrefab => bulletPrefab;
    public float Damage => damage;
    public float Cooldown => cooldown;
    public float Speed => speed;
    public float Duration => duration;
    public int Count => count;
    public float Area => area;
}
