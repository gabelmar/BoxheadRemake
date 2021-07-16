using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement/Targeting")]
    [SerializeField] private Transform playerPos;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 1.5f;
    [SerializeField] private float targetUpdateInterval = 2f;

    [SerializeField]
    private LayerMask layersThatBlockVision;

    [Header("Components")]
    [SerializeField]
    private EnemyAttack attack;
    [SerializeField]
    private NavMeshAgent navAgent;
    [SerializeField]
    private HealthSystem health;
    [SerializeField]
    private Collider col;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Enemy enemy;
    

    private Vector3 targetDirectionNormalized;
    private Vector3 targetDirection;
    private float distanceToPlayer;
    private Quaternion lookRotation;

    private Player player;

    private ZombieManager zombieManager;

    private bool isRotatingToPlayer = false;

    private float hitTimeout = 0.85f;
    private bool isHit = false;
    private bool isDead = false;

    private bool isRanged;

    void OnEnable()
    {
        health.OnDamageTaken += DamageTaken;
        health.OnDeath += Die;
    }
    private void OnDisable()
    {
        health.OnDamageTaken -= DamageTaken;
        health.OnDeath -= Die;
    }

    private void Update()
    {
        if (isRotatingToPlayer) 
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            if (Quaternion.Angle(transform.rotation, lookRotation) < 1.5f)
                isRotatingToPlayer = false;
        }

        if (isHit) 
        {
            hitTimeout -= Time.deltaTime;
            if (hitTimeout <= 0f)
            {
                isHit = false;
                hitTimeout = 0.85f;
                navAgent.speed = movementSpeed;
            }
            else if (hitTimeout <= 0.9f) 
            {
                navAgent.speed = 0f;
            }
        }
    }

    private IEnumerator UpdateTarget()
    {
        while (true) 
        {
            yield return new WaitForSeconds(targetUpdateInterval);

            if (isDead)
                yield break;

            UpdateTargetDirection();
            distanceToPlayer = Vector3.Distance(transform.position, playerPos.position);
            if (distanceToPlayer >= attack.AttackRange || (isRanged && !HasVisionToPlayer()))
            {
                //out of attack range Update target /destination
                if(navAgent.isStopped)
                    navAgent.isStopped = false;

                navAgent.SetDestination(playerPos.position);
                lookRotation = Quaternion.LookRotation(targetDirectionNormalized);
                isRotatingToPlayer = true;

                animator.SetBool(AnimatorKeys.zombieAttack, false);
                animator.SetBool(AnimatorKeys.zombieIsWalking, true);
            }
            else 
            {
                // close enough trigger an attack
                navAgent.isStopped = true;
                lookRotation = Quaternion.LookRotation(targetDirectionNormalized);
                isRotatingToPlayer = true;

                animator.SetBool(AnimatorKeys.zombieAttack, true) ;
                animator.SetBool(AnimatorKeys.zombieIsWalking, false);
            }
        }
    }

    public void Initialize(Transform player, ZombieManager zombieManager) 
    {
        this.zombieManager = zombieManager;
        playerPos = player;
        navAgent.SetDestination(playerPos.position);
        navAgent.speed = movementSpeed;
        transform.LookAt(playerPos.position);
        isRanged = attack.IsRanged;
        StartCoroutine(UpdateTarget());
        this.player = playerPos.GetComponent<Player>();
    }
    public void DamageTaken(int damage)
    {
        // Zombie got hit
        navAgent.velocity = - transform.forward * movementSpeed;
        animator.SetBool(AnimatorKeys.zombieAttack, false);
        animator.SetTrigger(AnimatorKeys.zombieHit);
        isHit = true;
    }

    private void Die(int hp) 
    {
        if (isDead)
            return;
        isDead = true;
        navAgent.enabled = false;
        col.enabled = false;
        animator.SetBool(AnimatorKeys.zombieIsWalking, false);
        animator.SetBool(AnimatorKeys.zombieAttack, false);
        animator.SetTrigger(AnimatorKeys.zombieDie);
        animator.SetBool(AnimatorKeys.zombieIsDead, true);
        player.KilledEnemy(enemy);
        zombieManager.RemoveZombie(gameObject);
        Destroy(gameObject, 3f); 
    }
    private void UpdateTargetDirection() 
    {
        targetDirection = playerPos.position - transform.position;
        targetDirection.y = 0.0f;
        targetDirectionNormalized = targetDirection.normalized;
    }
    private bool HasVisionToPlayer() 
    {
        if (Physics.Raycast(transform.position, targetDirection, out RaycastHit hit, distanceToPlayer, layersThatBlockVision))
            return hit.transform.CompareTag("Player");
        else return false;
    }
    public void AttackFromAnimation()
    {
        if (isDead || isHit)
            return;

        attack.Attack(targetDirectionNormalized);
    }

    private IEnumerator Scale(float value, float time, Action finishAction) 
    {
        Vector3 originalScale = transform.localScale;
        Vector3 finalScale = originalScale * value;
        float currentTime = 0.0f;

        while (currentTime <= time) 
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, finalScale, currentTime / time);
            yield return null;
        }

        finishAction?.Invoke();
    }

    private IEnumerator FallOver(float time, Action finishAction) 
    {
        Vector3 positionDifference = new Vector3(0.0f, -0.79f, -1.329f);
        Vector3 finalPosition = transform.position + transform.TransformDirection(positionDifference);
        Vector3 originalPosition = transform.position;
        
        Quaternion originalRotation = transform.rotation;
        Quaternion finalRotation = Quaternion.Euler(-90.0f, originalRotation.eulerAngles.y, 0.0f);
        float currentTime = 0.0f;
        float percentage = 0.0f;

        while (currentTime <= time) 
        {
            currentTime += Time.deltaTime;
            percentage = currentTime / time;
            transform.position = Vector3.Lerp(originalPosition, finalPosition, percentage);
            transform.rotation = Quaternion.Lerp(originalRotation, finalRotation, percentage);
            yield return null;
        }
        animator.enabled = false;
        yield return new WaitForSeconds(0.5f);
        finishAction?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireSphere(attack.damageArea.position, attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + targetDirection);
    }
}
