using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
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
    public CharacterData characterData;
    public NavMeshAgent navMeshAgent;
    public Camera mainCamera;
    public UnitSelection unitSelection;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public Animator animator;
    public bool isAttacking = false;
    public float stoppingDistance;
    public bool isRanged;
    public bool isSelected;
    public bool isAutoBattle = false;
    public GameObject target;
    public EnemyController targetController;
    [Header("Melee Attack")]
    private bool performMeleeAttack = true;
    private float nextAttackTime;

    [Header("Ranged Attack")]
    private bool performRangedAttack = true;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public GameObject spawnedProjectile;
    ProjectileScript projectileScript;


    private void Start()
    {
        currentHealth = characterData.maxHealth;
        currentDamage = characterData.attackDamage;
        currentArmor = characterData.armor;
        currentAttackSpeed = characterData.attackSpeed;
        currentAttackRange = characterData.attackRange;
        currentMoveSpeed = characterData.moveSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyLayer = LayerMask.GetMask("Enemy");
        // Busca por todos os inimigos presentes na cena e adiciona o ouvinte ao evento OnDeath.
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.OnDeath.AddListener(ReceiveExperience);
        }
        stoppingDistance = currentAttackRange;

        //Novo Metodos de Seleção
        UnitSelection.Instance.unitList.Add(this);
    }

    private void OnDestroy()
    {
        UnitSelection.Instance.unitList.Remove(this);
    }

    private void Update()
    {
        attackCooldown = 1f / characterData.attackSpeed;

        if (!isRanged)
        {
            if (target != null && performMeleeAttack && Time.time > nextAttackTime)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= currentAttackRange)
                {
                    StartCoroutine(MelleAttackInterval());
                    this.transform.LookAt(target.transform.position);
                }
            }
        }
        else if(isRanged)
        {
            if (target != null && performRangedAttack && Time.time > nextAttackTime)
            {
                if (Vector3.Distance(transform.position, target.transform.position) <= currentAttackRange)
                {
                    StartCoroutine(RangedAttackInterval());
                    this.transform.LookAt(target.transform.position);
                }
            }
        }


        

    }

    private void ReceiveExperience(int experienceReceived)
    {
        characterData.GainExperience(experienceReceived);
    }



    private IEnumerator MelleAttackInterval()
    {
        performMeleeAttack = false;
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackCooldown);

        if(target == null)
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
    private void MeleeAttackEnemy()
    {
        targetController.TakeDamage(currentDamage);
        
        nextAttackTime = Time.time + attackCooldown;
        performMeleeAttack = true;
        animator.SetBool("isAttacking", false);
    }

    private void RangedAttackEnemy()
    {

        spawnedProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation);
        ProjectileScript projectileTarget = spawnedProjectile.GetComponent<ProjectileScript>();

        if(projectileTarget != null)
        {
            projectileTarget.SetSource(transform);
            if(target != null)
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

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount * (currentArmor / (currentArmor + 100));
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Adicione aqui a l�gica para quando a unidade morrer.
        Destroy(gameObject);
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    // Define se a unidade est� selecionada.
    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }

}