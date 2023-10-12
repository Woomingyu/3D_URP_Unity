using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    public EnemyChasingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    public override void Enter()
    {
        //정지 상태이므로 이동 처리 방지
        stateMachine.MovementSpeedModifier = 1f;
        base.Enter();
        StartAnimation(stateMachine.Enemy.animationData.GroundParameterHash);
        StartAnimation(stateMachine.Enemy.animationData.RunParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.animationData.GroundParameterHash);
        StopAnimation(stateMachine.Enemy.animationData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (!IsInChaseRange())
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
        else if(IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }
    }
    protected bool IsInAttackRange()
    {
        
        if(stateMachine.Target.IsDead)
        {
            return false;
        }
        

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude; //제곱 푸는연산 = 무거움 => sqrMagnitude 그대로 사용 후 곱하기 함더

        return playerDistanceSqr <= stateMachine.Enemy.Data.AttackRange * stateMachine.Enemy.Data.AttackRange;
    }
}
