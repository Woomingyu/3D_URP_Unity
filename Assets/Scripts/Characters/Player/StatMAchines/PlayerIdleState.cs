using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        //���� �����̹Ƿ� �̵� ó�� ����
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Player.animationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Enter();
        StopAnimation(stateMachine.Player.animationData.IdleParameterHash);
    }

    public override void Update()
    {
        base .Update();
    }
}
