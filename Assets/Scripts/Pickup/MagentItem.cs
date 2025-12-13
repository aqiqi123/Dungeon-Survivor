using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagentItem : PickupBase, IPickupable
{
    public void OnPickUp() {
        foreach(var pickup in ActivePickups) {
            if(pickup.TryGetComponent<ExpGem>(out ExpGem expGem)) {
                expGem.ForceMagnetize();
            }
        }
    }
}
