using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //충돌체를 가지고 공격
    [SerializeField]
    private Collider myCollider;

    private int damage;
    private float knockback;

    private List<Collider> alreadyColliderWith = new List<Collider>();

    private void OnEnable()
    {
        //켜질때마다 클리어
        alreadyColliderWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        //나 자신, 이미 포함됨 == 리턴
        if (other == myCollider)
            return;
        if (alreadyColliderWith.Contains(other))
            return;

        alreadyColliderWith.Add(other);
        if(other.TryGetComponent(out Health health))
        {
            health.TakeDamage(damage);
        }
        if(other.TryGetComponent(out ForceReciver forceReciver))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReciver.AddForce(direction * knockback);
        }
    }


    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
