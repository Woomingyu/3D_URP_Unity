using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }


    [field: Header("Animations")]
    [field: SerializeField]
    public PlayerAnimationData animationData { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerInputOrigin Input { get; private set; }
    public CharacterController Controller { get; private set; }

    public ForceReciver ForceReciver { get; private set; }

    [field: SerializeField] public Weapon Weapon { get; private set; }

    public Health health { get; private set; }

    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        animationData.Initialize();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Input = GetComponent<PlayerInputOrigin>();
        Controller = GetComponent<CharacterController>();
        ForceReciver = GetComponent<ForceReciver>();
        health = GetComponent<Health>();

        stateMachine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        //커서 사라지게 하기
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);

        health.OnDie += OnDie;
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    void OnDie()
    {
        Animator.SetTrigger("Die");
        enabled = false;
    }
}
