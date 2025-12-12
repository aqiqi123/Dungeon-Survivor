using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput instance {  get; private set; }

    public event Action OnPauseAction;

    private PlayerInputActions playerInputActions;

    private void Awake() {
        instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.UI.Enable();

        playerInputActions.UI.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnPauseAction?.Invoke();
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector=playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector=inputVector.normalized;

        return inputVector;
    }
}
