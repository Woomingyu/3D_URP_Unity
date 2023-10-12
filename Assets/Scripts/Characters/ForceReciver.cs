using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReciver : MonoBehaviour
{
    //��Ʈ�ѷ��� �߷�or�ٸ� �� ����

    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag = 0.3f; // ����


    //���� ����,��
    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;

    //�̵� �� ���ٽ� �߰����� �� + �������� ����ϴ� ��
    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        //���� �پ��ִ��� üũ
        if(verticalVelocity < 0f && controller.isGrounded)
        {
            //���� ���
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            //���� �ƴ϶�� ���� �������� �߷� ����
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        //SmoothDamp : ������ ��ǥ������ ����(current -> target : impact -> Vector3.zero , ���� velocity, ����ð� )
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);
    }

    //Velocity ����
    public void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }

    //���� �̿� �޴� ��
    public void AddForce(Vector3 force)
    {
        impact += force;
    }

    //����
    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
