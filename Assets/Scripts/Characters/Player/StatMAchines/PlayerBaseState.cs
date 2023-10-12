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

    //필요한 부분

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

    //버튼이 눌려지는동안
    protected virtual void OnAttackPerformed(InputAction.CallbackContext obj)
    {
        stateMachine.IsAttacking = true;
    }

    //버튼 뗌
    protected virtual void OnAttackCanceled(InputAction.CallbackContext obj)
    {
        stateMachine.IsAttacking = false;
    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
    }

    //실제 이동 처리
    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);

        Move(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        //카메라가 바라보는 방향으로 이동
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        //땅바닥 안보게
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }

    //플레이어 객체의 모든 위치 이동(이동/점프)
    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Player.Controller.Move(
            ((movementDirection * movementSpeed) 
            + stateMachine.Player.ForceReciver.Movement) 
            * Time.deltaTime

            );
    }

    //디렉션 값을 안받고 처리가능 ForceReciver.Movement만 사용
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


    //애니메이션의 플레이 타임 저장
    protected float GetNormalizedTime(Animator animator, string tag)
    {
        //현재 애니메이션 정보 가져옴
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        //다음 애니메이션 정보 가져옴
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        //애니메이션이 라인을 타고있는지 && 다음 태그가 attack(tag)인지 체크
        if(animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            //다음 애니메이션 리턴
            return nextInfo.normalizedTime;
        }
        //라인을 안타고 있다 && 내 태그가 attack이다
        else if(!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            //내 애니메이션 리턴
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

}
