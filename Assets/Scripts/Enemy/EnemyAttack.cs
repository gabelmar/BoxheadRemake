using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    public enum AttackType 
    {
        Melee,
        Ranged
    }
    [SerializeField] protected float attackRange = 1.75f;
    [SerializeField] protected int attackDamage = 30;
    [SerializeField] protected Transform damageArea;
    [SerializeField]
    protected AttackType type;

    public AttackType Type => type;
    public abstract void Attack(Vector3 targetDirection);

    public float AttackRange => attackRange;

    public bool IsRanged => type == AttackType.Ranged;
}
