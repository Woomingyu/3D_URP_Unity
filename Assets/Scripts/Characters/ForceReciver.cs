using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReciver : MonoBehaviour
{
    //컨트롤러에 중력or다른 힘 적용

    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag = 0.3f; // 저항


    //각종 방향,힘
    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;

    //이동 값 람다식 추가적인 힘 + 수직으로 상승하는 힘
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        //땅에 붙어있는지 체크
        if(verticalVelocity < 0f && controller.isGrounded)
        {
            //물리 사용
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            //땅이 아니라면 점점 빨라지는 중력 적용
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        //SmoothDamp : 서서히 목표값으로 진행(current -> target : impact -> Vector3.zero , 현재 velocity, 진행시간 )
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    //Velocity 리셋
    public void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }

    //점프 이외 받는 힘
    public void AddForce(Vector3 force)
    {
        impact += force;
    }

    //점프
    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
