using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputOrigin : MonoBehaviour
{
    public PlayerInput InputActions { get; private set; }
    public PlayerInput.PlayerActions playerActions { get; private set; }


    private void Awake()
    {
        InputActions = new PlayerInput();

        playerActions = InputActions.Player;
    }


    //활성화 상태에 따른 액션 on/off
    private void OnEnable()
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }
}
