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

            //데미지 처리
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
        //이미 적용했다면 void return
        if (alreadyAppliedForce)
            return;

        alreadyAppliedForce = true;


        //내가 받고있던 힘 리셋
        stateMachine.Enemy.ForceReciver.Reset();

        //ForceReciver에 내가 받고있는 힘을 더해줌(AddForce()) 
        stateMachine.Enemy.ForceReciver.AddForce(stateMachine.Enemy.transform.forward * stateMachine.Enemy.Data.Force);
    }
}
    