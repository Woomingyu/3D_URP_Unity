using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.JumpForce = stateMachine.Player.Data.AirData.JumpForce;
        stateMachine.Player.ForceReciver.Jump(stateMachine.JumpForce);

        base.Enter();

        StartAnimation(stateMachine.Player.animationData.JumpParameterHash);
    }

    public override void Exit() 
    {
        base.Exit();

        StopAnimation(stateMachine.Player.animationData.JumpParameterHash);
    }

    //���� ������Ʈ(�߶�)
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //������ ������ ��(����)
        if(stateMachine.Player.Controller.velocity.y <= 0)
        {
            //�������� ���·� ����
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}
