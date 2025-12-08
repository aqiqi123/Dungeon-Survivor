using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance {  get; private set; }

    [SerializeField] private CharacterSO characterData;

    public float CurrentMoveSpeed {  get; private set; }

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        CurrentMoveSpeed =characterData.MoveSpeed;
    }
}
