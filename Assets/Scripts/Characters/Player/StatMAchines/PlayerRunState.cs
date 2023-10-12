using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
   public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Player.animationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.animationData.RunParameterHash);
    }
}
