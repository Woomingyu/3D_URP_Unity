using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.animationData.GroundParameterHash);
    }

    public override void Exit() 
    { 
        base.Exit();
        StopAnimation(stateMachine.Player.animationData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();

        //�ٸ� ���¿����� ���� �����ϰ� �ϰ�ʹٸ� �߰�
        //�޸��⳪ ��Ÿ ���¿��� �����ϰ�ʹٸ� �ش� State���� OnAttack�� �������̵�
        if(stateMachine.IsAttacking)
        {
            OnAttack();
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //���°� ���� �ƴϰ�, �ٵ��� y�ӵ� �̵��ӵ��� �߷� * �����ӵ� ������Ʈ ���� ������
        if(!stateMachine.Player.Controller.isGrounded
            && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }


    //���� ���¿��� ����� ���� => ���߿��� ������ �̻���
    protected override void OnmovementCanceled(InputAction.CallbackContext context)
    {
        if(stateMachine.MovementInput == Vector2.zero)
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.IdleState);
        
        base.OnmovementCanceled(context);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected virtual void OnMove()
    {
        stateMachine.ChangeState(stateMachine.WalkState);
    }

    //���߿����� �����ϰ�ʹٸ� AirState������ ó��
    protected virtual void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.ComboAttackState);
    }
}
