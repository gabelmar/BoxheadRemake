using System;
using UnityEngine;

public class MeleeAttack : EnemyAttack
{
    [SerializeField] protected float attackRadius = 1.5f;
    public override void Attack(Vector3 targetDirection)
    {
        Collider[] hitObjects = Physics.OverlapSphere(damageArea.position, attackRadius);
        for (int i = 0; i < hitObjects.Length; i++)
        {
            if (!hitObjects[i].CompareTag("Player"))
                continue;
            else
            {
                hitObjects[i].transform.GetComponent<HealthSystem>().TakeDamage(attackDamage);
                return;
            }
        }
    }
}
