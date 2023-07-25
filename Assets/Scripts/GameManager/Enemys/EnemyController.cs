using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float currentHealth;
    public float currentDamage;
    public float currentArmor;
    public float currentAttackSpeed;
    public float currentAttackRange;
    public float currentMoveSpeed;
    public float attackCooldown;
    public float animatorCooldown;
    public float lastAttackTime = 0.0f;
    public float nextAttackTime;
    public int experienceValue;
    private EnemyMovement enemyMovement;
    public EnemyData enemyData;
    public Transform targetUnit;
    public UnitController target;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public LayerMask unitLayer;
    public bool isRanged;
    [Header("Melee Attack")]
    private bool performMeleeAttack = true;

    [Header("Ranged Attack")]
    private bool performRangedAttack = true;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public GameObject spawnedProjectile;
    ProjectileScript projectileScript;
    private bool isAlive = true;
    public UnityEvent<int> OnDeath = new UnityEvent<int>(); // Evento com argumento int para passar a experiência ao evento.

    private void Start()
    {
        currentHealth = enemyData.maxHealth;
        currentDamage = enemyData.attackDamage;
        currentArmor = enemyData.armor;
        currentAttackSpeed = enemyData.attackSpeed;
        currentAttackRange = enemyData.attackRange;
        currentMoveSpeed = enemyData.moveSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        unitLayer = LayerMask.GetMask("Unit");
        experienceValue = this.enemyData.experience;
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (!isRanged)
        {
            if (target != null && performMeleeAttack && Time.time > nextAttackTime)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= currentAttackRange)
                {
                    StartCoroutine(MelleAttackInterval());
                }
            }
        }
        else if (isRanged)
        {
            if (target != null && performRangedAttack && Time.time > nextAttackTime)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= currentAttackRange)
                {
                    StartCoroutine(RangedAttackInterval());
                }
            }
        }
    }

    private IEnumerator MelleAttackInterval()
    {
        performMeleeAttack = false;
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackCooldown);

        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            performMeleeAttack = true;
        }
    }

    private IEnumerator RangedAttackInterval()
    {
        performRangedAttack = false;
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackCooldown);

        if (target == null)
        {
            animator.SetBool("isAttacking", false);
            performRangedAttack = true;
        }
    }
    public void TakeDamage(float damageAmount)
    {
        
        if (!isAlive)
            return;
        currentHealth -= damageAmount * (enemyData.armor / (enemyData.armor + 100));
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            FindNearestUnit();
        }
    }

    private void Die()
    {
        isAlive = false;
        OnDeath.Invoke(experienceValue); // Invocamos o evento passando o valor de experiência.
        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    private void FindNearestUnit()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        Transform nearestUnit = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject unit in units)
        {
            if (unit != gameObject)
            {
                float distance = Vector3.Distance(transform.position, unit.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestUnit = unit.transform;
                    targetUnit = nearestUnit;
                    target = unit.GetComponent<UnitController>();
                    
                }
            }
        }

        if (nearestUnit != null)
        {
            if(closestDistance > currentAttackRange)
            {
                enemyMovement.MoveToDestination(nearestUnit.position);
            }
            else
            {
                transform.LookAt(targetUnit.position);
            }
            
        }
    }


    private void AttackTargetUnit()
    {
        target.TakeDamage(currentDamage);
        nextAttackTime = Time.time + attackCooldown;
        performMeleeAttack = true;
        animator.SetBool("isAttacking", false);
    }

    private void RangedAttackTargetUnit()
    {

        spawnedProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation);
        ProjectileScript projectileTarget = spawnedProjectile.GetComponent<ProjectileScript>();

        if (projectileTarget != null)
        {
            projectileTarget.SetSource(transform);
            if (target != null)
            {
                projectileTarget.SetTarget(target.transform);
            }
            else
            {
                performRangedAttack = true;
                animator.SetBool("isAttacking", false);
                return;
            }
        }
        nextAttackTime = Time.time + attackCooldown;
        performRangedAttack = true;
        animator.SetBool("isAttacking", false);
    }
}
