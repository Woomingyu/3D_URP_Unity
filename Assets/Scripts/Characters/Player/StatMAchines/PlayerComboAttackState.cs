using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAttackState : PlayerAttackState
{
    private bool alreadyAppliedForce; // 포스 적용 체크
    private bool alreadyApplyCombo; // 콤보 적용 체크


    AttackInfoData attackInfoData;

    public PlayerComboAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //애니메이션 해쉬값 처리
        StartAnimation(stateMachine.Player.animationData.ComboAttackParameterHash);

        //변수 초기화
        alreadyApplyCombo = false;
        alreadyAppliedForce = false;

        //콤보인덱스에 대한 정보
        int comboIndex = stateMachine.ComboIndex;
        //콤보어택에 대한 정보
        attackInfoData = stateMachine.Player.Data.AttackData.GetAttackInfo(comboIndex);
        //해쉬값 처리 없이 바로 애니 적용
        stateMachine.Player.Animator.SetInteger("Combo", comboIndex);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.animationData.ComboAttackParameterHash);

        //콤보 적용X == 공격중에 콤보를 사용하지 않았음
        if (!alreadyApplyCombo)
            stateMachine.ComboIndex = 0; //콤보 인덱스 초기화(다음에 사용해야 하므로)
    }

    private void TryComboAttack()
    {
        //콤보어택 진행중 & 마지막 공격 & 공격이 끝남(멈춤) 이라면 void return 
        if (alreadyApplyCombo)
            return;
        if (attackInfoData.ComboStateIndex == -1)
            return;
        if (!stateMachine.IsAttacking)
            return;

        //모든 조건을 넘어왔다면 true
        alreadyApplyCombo = true;
    }


    //밀어내는 힘
    private void TryApplyForce()
    {
        //이미 적용했다면 void return
        if (alreadyAppliedForce)
            return;

        alreadyAppliedForce = true;


        //내가 받고있던 힘 리셋
        stateMachine.Player.ForceReciver.Reset();

        //ForceReciver에 내가 받고있는 힘을 더해줌(AddForce()) 
        stateMachine.Player.ForceReciver.AddForce(stateMachine.Player.transform.forward * attackInfoData.Force);
    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        //노멀라이즈 타임 가져오기
        float nomalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");
        if (nomalizedTime < 1f) // 애니메이션 처리중
        {
            //애니메이션 진행 시간이 힘을 적용해야 하는 시간보다 커졌다
            if (nomalizedTime >= attackInfoData.ForceTransitionTime)
                TryApplyForce();

            if (nomalizedTime >= attackInfoData.ComboTransitionTime)
                TryComboAttack();
        }
        else // 애니메이션 처리 완료
        {
            if (alreadyApplyCombo)
            {
                //콤보 인덱스 증가(다음 인덱스 변수에 적용)
                stateMachine.ComboIndex = attackInfoData.ComboStateIndex;
                stateMachine.ChangeState(stateMachine.ComboAttackState);
            }
            else // 애니메이션 완전 종료
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }
}
    

