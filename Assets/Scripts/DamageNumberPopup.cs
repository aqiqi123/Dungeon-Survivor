using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class DamageNumberPopup : MonoBehaviour
{
    [SerializeField]private GameObject damageNumberPrefab;

    private HealthSystem healthSystem;

    private void Awake() {
        healthSystem = GetComponent<HealthSystem>();
        
        if(healthSystem != null) {
            healthSystem.OnTakeDamage += PopupDamageNumber;
        }
    }

    private void OnDestroy() {
        if(healthSystem != null) {
            healthSystem.OnTakeDamage -= PopupDamageNumber;
        }
    }

    private void PopupDamageNumber(float damageAmount) {
        if(damageNumberPrefab == null || ObjectPoolManager.Instance == null) return;

        GameObject damageNumberObj = ObjectPoolManager.Instance.Spawn(damageNumberPrefab, transform.position, Quaternion.identity);
        
        if (damageNumberObj != null && damageNumberObj.TryGetComponent<DamageNumber>(out var damageNumber)) {
            damageNumber.SetDamageText(damageAmount);
        }
    }
}
