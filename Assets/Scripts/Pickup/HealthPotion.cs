using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : PickupBase, IPickupable 
{
    [SerializeField] private float healAmount;

    public void OnPickUp() {
        if (PlayerStats.Instance != null) {
            PlayerStats.Instance.Heal(healAmount);
        }
    }
}
