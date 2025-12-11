using UnityEngine;

public class ExpGem : PickupBase, IPickupable {
    [SerializeField] private int expAmount;

    public void OnPickUp() {
        if (LevelManager.Instance == null) return;

        LevelManager.Instance.AddExperience(expAmount);
    }
}
