using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine)
    {
    }

    private bool alreadyAppliedForce;
    private bool alreadyAppliedDealing;
    public override void Enter()
    {
        alreadyAppliedForce = false;
        alreadyAppliedDealing = false;

        stateMachine.MovementSpeedModifier = 0;
        base.Enter();
        StartAnimation(stateMachine.Enemy.animationData.AttackParameterHash);
        StartAnimation(stateMachine.Enemy.animationData.BaseAttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.animationData.WalkParameterHash);
        StopAnimation(stateMachine.Enemy.animationData.BaseAttackParameterHash);
    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        float nomalizedTime = GetNormalizedTime(stateMachine.Enemy.Animator, "Attack");
        if(nomalizedTime < 1f)
        {
            if(nomalizedTime >= stateMachine.Enemy.Data.ForceTransitionTime)
                TryApplyForce();

            //������ ó��
            if(!alreadyAppliedDealing && nomalizedTime >= stateMachine.Enemy.Data.Dealing_Start_TransitionTime)
            {
                stateMachine.Enemy.Weapon.SetAttack(stateMachine.Enemy.Data.Damage, stateMachine.Enemy.Data.Force);
                stateMachine.Enemy.Weapon.gameObject.SetActive(true);
                alreadyAppliedDealing = true;
            }

            if (alreadyAppliedDealing && nomalizedTime >= stateMachine.Enemy.Data.Dealing_End_TransitionTime)
            {
                stateMachine.Enemy.Weapon.gameObject.SetActive(false);
            }


        }
        else
        {
            if (IsInChaseRange())
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
                return;
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }
        }
    }

    private void TryApplyForce()
    {
        //�̹� �����ߴٸ� void return
        if (alreadyAppliedForce)
            return;

        alreadyAppliedForce = true;


        //���� �ް��ִ� �� ����
        stateMachine.Enemy.ForceReciver.Reset();

        //ForceReciver�� ���� �ް��ִ� ���� ������(AddForce()) 
        stateMachine.Enemy.ForceReciver.AddForce(stateMachine.Enemy.transform.forward * stateMachine.Enemy.Data.Force);
    }
}
    