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


    //Ȱ��ȭ ���¿� ���� �׼� on/off
    private void OnEnable()
    {
        InputActions.Enable();
    }

    private void OnDisable()
    {
        InputActions.Disable();
    }
}
