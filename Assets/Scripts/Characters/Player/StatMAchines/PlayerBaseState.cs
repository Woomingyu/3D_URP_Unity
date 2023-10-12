using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        ;
    }

    public virtual void Update()
    {
        Move();
    }

    //�ʿ��� �κ�

    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInputOrigin input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled += OnmovementCanceled;
        input.playerActions.Run.started += OnRunStarted;

        stateMachine.Player.Input.playerActions.Jump.started += OnJumpStarted;

        stateMachine.Player.Input.playerActions.Attack.performed += OnAttackPerformed;
        stateMachine.Player.Input.playerActions.Attack.canceled += OnAttackCanceled;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerInputOrigin input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnmovementCanceled;
        input.playerActions.Run.started -= OnRunStarted;

        stateMachine.Player.Input.playerActions.Jump.started -= OnJumpStarted;

        stateMachine.Player.Input.playerActions.Attack.performed -= OnAttackPerformed;
        stateMachine.Player.Input.playerActions.Attack.canceled -= OnAttackCanceled;
    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnmovementCanceled(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        
    }

    //��ư�� �������µ���
    protected virtual void OnAttackPerformed(InputAction.CallbackContext obj)
    {
        stateMachine.IsAttacking = true;
    }

    //��ư ��
    protected virtual void OnAttackCanceled(InputAction.CallbackContext obj)
    {
        stateMachine.IsAttacking = false;
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }

    //���� �̵� ó��
    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);

        Move(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        //ī�޶� �ٶ󺸴� �������� �̵�
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        //���ٴ� �Ⱥ���
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    //�÷��̾� ��ü�� ��� ��ġ �̵�(�̵�/����)
    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Player.Controller.Move(
            ((movementDirection * movementSpeed) 
            + stateMachine.Player.ForceReciver.Movement) 
            * Time.deltaTime

            );
    }

    //�𷺼� ���� �ȹް� ó������ ForceReciver.Movement�� ���
    protected void ForceMove()
    {
        stateMachine.Player.Controller.Move(stateMachine.Player.ForceReciver.Movement * Time.deltaTime);
    }

    private void Rotate(Vector3 movementDirection)
    {
        if(movementDirection != Vector3.zero)
        {
            Transform playerTransform = stateMachine.Player.transform;
            Quaternion targetRation = Quaternion.LookRotation(movementDirection);
            stateMachine.Player.transform.rotation = Quaternion.Slerp(stateMachine.Player.transform.rotation, targetRation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return movementSpeed;
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }


    //�ִϸ��̼��� �÷��� Ÿ�� ����
    protected float GetNormalizedTime(Animator animator, string tag)
    {
        //���� �ִϸ��̼� ���� ������
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        //���� �ִϸ��̼� ���� ������
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        //�ִϸ��̼��� ������ Ÿ���ִ��� && ���� �±װ� attack(tag)���� üũ
        if(animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            //���� �ִϸ��̼� ����
            return nextInfo.normalizedTime;
        }
        //������ ��Ÿ�� �ִ� && �� �±װ� attack�̴�
        else if(!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            //�� �ִϸ��̼� ����
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

}
