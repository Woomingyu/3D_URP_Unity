using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//추상클래스(자체 객체화 불가)
public abstract class StateMachine
{
    protected IState currentState;

    public void ChangeState(IState newState)
    {
        //?. : null이 아닌 경우에만 호출
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
