using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAttackState : PlayerAttackState
{
    private bool alreadyAppliedForce; // ���� ���� üũ
    private bool alreadyApplyCombo; // �޺� ���� üũ


    AttackInfoData attackInfoData;

    public PlayerComboAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //�ִϸ��̼� �ؽ��� ó��
        StartAnimation(stateMachine.Player.animationData.ComboAttackParameterHash);

        //���� �ʱ�ȭ
        alreadyApplyCombo = false;
        alreadyAppliedForce = false;

        //�޺��ε����� ���� ����
        int comboIndex = stateMachine.ComboIndex;
        //�޺����ÿ� ���� ����
        attackInfoData = stateMachine.Player.Data.AttackData.GetAttackInfo(comboIndex);
        //�ؽ��� ó�� ���� �ٷ� �ִ� ����
        stateMachine.Player.Animator.SetInteger("Combo", comboIndex);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.animationData.ComboAttackParameterHash);

        //�޺� ����X == �����߿� �޺��� ������� �ʾ���
        if (!alreadyApplyCombo)
            stateMachine.ComboIndex = 0; //�޺� �ε��� �ʱ�ȭ(������ ����ؾ� �ϹǷ�)
    }

    private void TryComboAttack()
    {
        //�޺����� ������ & ������ ���� & ������ ����(����) �̶�� void return 
        if (alreadyApplyCombo)
            return;
        if (attackInfoData.ComboStateIndex == -1)
            return;
        if (!stateMachine.IsAttacking)
            return;

        //��� ������ �Ѿ�Դٸ� true
        alreadyApplyCombo = true;
    }


    //�о�� ��
    private void TryApplyForce()
    {
        //�̹� �����ߴٸ� void return
        if (alreadyAppliedForce)
            return;

        alreadyAppliedForce = true;


        //���� �ް��ִ� �� ����
        stateMachine.Player.ForceReciver.Reset();

        //ForceReciver�� ���� �ް��ִ� ���� ������(AddForce()) 
        stateMachine.Player.ForceReciver.AddForce(stateMachine.Player.transform.forward * attackInfoData.Force);
    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        //��ֶ����� Ÿ�� ��������
        float nomalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");
        if (nomalizedTime < 1f) // �ִϸ��̼� ó����
        {
            //�ִϸ��̼� ���� �ð��� ���� �����ؾ� �ϴ� �ð����� Ŀ����
            if (nomalizedTime >= attackInfoData.ForceTransitionTime)
                TryApplyForce();

            if (nomalizedTime >= attackInfoData.ComboTransitionTime)
                TryComboAttack();
        }
        else // �ִϸ��̼� ó�� �Ϸ�
        {
            if (alreadyApplyCombo)
            {
                //�޺� �ε��� ����(���� �ε��� ������ ����)
                stateMachine.ComboIndex = attackInfoData.ComboStateIndex;
                stateMachine.ChangeState(stateMachine.ComboAttackState);
            }
            else // �ִϸ��̼� ���� ����
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }
}
    

