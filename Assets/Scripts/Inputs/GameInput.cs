using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    private PlayerInputActions playerInputActions;
    public event EventHandler OnPlayerMoveLeft;
    public event EventHandler OnPlayerMoveRight;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are multiple instances of GameInput");
        }
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
    }
    private void Start()
    {
        playerInputActions.Player.MoveLeft.performed += MoveLeft_performed;
        playerInputActions.Player.MoveRight.performed += MoveRight_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
    }
    private void GameManager_OnStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            playerInputActions.Disable();
        }
        else
        {
            playerInputActions.Enable();
        }
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameManager.Instance.ToogleGamePause();
    }

    private void OnDestroy()
    {
        playerInputActions.Player.MoveLeft.performed -= MoveLeft_performed;
        playerInputActions.Player.MoveRight.performed -= MoveRight_performed;

        playerInputActions.Dispose();
    }

    private void MoveRight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerMoveRight?.Invoke(this, EventArgs.Empty);
    }

    private void MoveLeft_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPlayerMoveLeft?.Invoke(this, EventArgs.Empty);
    }
}
