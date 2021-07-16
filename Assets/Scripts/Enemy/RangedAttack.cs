using UnityEngine;

public class RangedAttack : EnemyAttack
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float projectileScale = 1f;

    public override void Attack(Vector3 targetDirection)
    {
        DevilFireball fireBall = Instantiate(projectilePrefab, damageArea.position, Quaternion.identity).GetComponent<DevilFireball>();
        fireBall.Speed = projectileSpeed;
        fireBall.Damage = attackDamage;
        fireBall.transform.localScale = fireBall.transform.localScale * projectileScale;
        fireBall.Direction = targetDirection;
    }
}
