using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagentItem : PickupBase, IPickupable
{
    public void OnPickUp() {
        foreach (var pickup in ActivePickups)
        {
            if (pickup != null && pickup != this && pickup is MagentItem)
            {
                pickup.ForceMagnetize();
            }
        }
    }
}
