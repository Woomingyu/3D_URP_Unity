using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
        //정지 상태이므로 이동 처리 방지
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Enemy.animationData.GroundParameterHash);
        StartAnimation(stateMachine.Enemy.animationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.animationData.GroundParameterHash);
        StopAnimation(stateMachine.Enemy.animationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
            return;
        }
    }
}
