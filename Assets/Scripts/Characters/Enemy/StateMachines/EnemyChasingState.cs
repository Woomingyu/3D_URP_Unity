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
        //���� �����̹Ƿ� �̵� ó�� ����
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
        

        float playerDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude; //���� Ǫ�¿��� = ���ſ� => sqrMagnitude �״�� ��� �� ���ϱ� �Դ�

        return playerDistanceSqr <= stateMachine.Enemy.Data.AttackRange * stateMachine.Enemy.Data.AttackRange;
    }
}
