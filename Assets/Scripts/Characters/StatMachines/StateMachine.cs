using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�߻�Ŭ����(��ü ��üȭ �Ұ�)
public abstract class StateMachine
{
    protected IState currentState;

    public void ChangeState(IState newState)
    {
        //?. : null�� �ƴ� ��쿡�� ȣ��
        currentState?.Exit();
        currentState = newState;

        currentState?.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }
    public void Update()
    {
        currentState?.Update();
    }
    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}
