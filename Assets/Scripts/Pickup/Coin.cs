using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : PickupBase, IPickupable 
{
    [SerializeField] private int coinAmount;
    public void OnPickUp() {
        
    }
}
