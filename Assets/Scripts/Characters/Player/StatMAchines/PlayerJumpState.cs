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

    //물리 업데이트(추락)
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        //땅으로 떨어질 때(공중)
        if(stateMachine.Player.Controller.velocity.y <= 0)
        {
            //떨어지는 상태로 변경
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}
