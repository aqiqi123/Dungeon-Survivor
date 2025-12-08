using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance {  get; private set; }

    [SerializeField] private CharacterSO characterData;

    public float CurrentMaxHealth {  get; private set; }
    public float CurrentMoveSpeed {  get; private set; }

    private HealthSystem healthSystem;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        healthSystem = GetComponent<HealthSystem>();

        InitializeStats();
    }

    private void InitializeStats() {
        if(characterData == null) {
            Debug.LogError("PlayerStats:Íü¼ÇÍÏ×§CharacterSOÁË£¡");
            return;
        }

        CurrentMaxHealth = characterData.MaxHealth;
        CurrentMoveSpeed = characterData.MoveSpeed;

        healthSystem.Initialize(CurrentMaxHealth);
    }
}
