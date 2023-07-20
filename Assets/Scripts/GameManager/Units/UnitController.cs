using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{ 
    private UnitMovement unitMovement;
    public int currentHealth;
    public int currentDamage;
    public int currentArmor;
    public float currentAttackSpeed;
    public float currentAttackRange;
    public float currentMoveSpeed;
    public bool isRanged;
    public bool isSelected;
    public bool isAutoBattle = false;
    public UnitStats unitStats = new UnitStats();
    public float attackCooldown;
    public float animatorCooldown;
    public float lastAttackTime = 0.0f;
    public NavMeshAgent navMeshAgent;
    public Camera mainCamera;
    public UnitSelection unitSelection;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public Animator animator;
    public bool isAttacking = false;
    public float stoppingDistance = 1f;

    private EnemyController target;
    private int experienceToReceive;
    private void Start()
    {
        InitializeStats();
        currentHealth = unitStats.maxHealth;
        currentDamage = unitStats.attackDamage;
        currentArmor = unitStats.armor;
        currentAttackSpeed = unitStats.attackSpeed;
        currentAttackRange = unitStats.attackRange;
        currentMoveSpeed = unitStats.moveSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyLayer = LayerMask.GetMask("Enemy");
        attackCooldown = 1f / unitStats.attackSpeed;
        animatorCooldown = 1f * unitStats.attackSpeed;

        // Busca por todos os inimigos presentes na cena e adiciona o ouvinte ao evento OnDeath.
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.OnDeath.AddListener(ReceiveExperience);
        }
    }

    private void ReceiveExperience(int experienceReceived)
    {
        unitStats.GainExperience(experienceReceived);
    }


    private void Update()
    {
        CheckForEnemyInRange();
        if (isAttacking == true)
        {
            animator.SetBool("isAttacking", true);
            animator.SetFloat("animatorSpeed", animatorCooldown);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
        if (isAutoBattle)
        {
            // Se estiver no modo de combate automático, procurar inimigos e atacar automaticamente.
            PerformAutoBattle();
        }
        currentHealth = unitStats.maxHealth;
        currentDamage = unitStats.attackDamage;
        currentArmor = unitStats.armor;
        currentAttackSpeed = unitStats.attackSpeed;
        currentAttackRange = unitStats.attackRange;
        currentMoveSpeed = unitStats.moveSpeed;
    }
    public void CheckForEnemyInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentAttackRange, enemyLayer);

        bool hasValidTarget = false; // Variável para indicar se a unidade encontrou um alvo válido.

        foreach (Collider col in hitColliders)
        {
            EnemyController enemyUnit = col.GetComponent<EnemyController>();

            if (enemyUnit && enemyUnit.IsAlive())
            {
                // Calcula a distância entre a unidade e o inimigo atual.
                float distanceToEnemy = Vector3.Distance(transform.position, enemyUnit.transform.position);

                // Verifica se o inimigo atual está dentro da distância de ataque.
                if (distanceToEnemy <= currentAttackRange)
                {
                    // Define o estado de ataque como true.
                    isAttacking = true;

                    // Ataca o inimigo somente quando a unidade estiver próxima o suficiente.
                    if (distanceToEnemy <= stoppingDistance)
                    {
                        AttackEnemy(enemyUnit);
                    }

                    hasValidTarget = true; // Indica que a unidade encontrou um alvo válido para atacar.
                    break; // Sai do loop, pois já encontramos um alvo válido.
                }
            }
        }

        // Se a unidade não encontrou nenhum alvo válido dentro da distância de ataque, desativa o estado de ataque.
        if (!hasValidTarget)
        {
            isAttacking = false;
        }
    }


    public void AttackEnemy(EnemyController enemy)
    {
        // Verifica se o tempo desde o último ataque é maior do que o tempo de intervalo entre ataques.
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Realiza o ataque e aplica dano ao inimigo
            enemy.TakeDamage(unitStats.attackDamage);

            // Verifica se o inimigo foi derrotado
            if (!enemy.IsAlive())
            {
                // Define o estado de ataque como false e limpa o alvo atual.
                isAttacking = false;
                target = null;
            }

            // Atualiza o tempo do último ataque.
            lastAttackTime = Time.time;
        }
    }



    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Adicione aqui a lógica para quando a unidade morrer.
        Destroy(gameObject);
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    // Define se a unidade está selecionada.
    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }

    public void ActivateAutoBattle(bool activate)
    {
        isAutoBattle = activate;

        // Se o Auto Battle foi ativado, interrompa qualquer movimento atual para que a unidade possa atacar imediatamente.
        if (isAutoBattle)
        {
            navMeshAgent.ResetPath();
        }
    }
    private void PerformAutoBattle()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f, enemyLayer);

        EnemyController nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider col in hitColliders)
        {
            EnemyController enemyUnit = col.GetComponent<EnemyController>();

            if (enemyUnit != null && enemyUnit.IsAlive())
            {
                // Verifica se a unidade já está atacando um inimigo.
                if (isAttacking)
                {
                    // Se a unidade já está atacando, continua a atacar o inimigo atual.
                    if (target == enemyUnit)
                    {
                        AttackEnemy(enemyUnit);
                    }
                }
                else
                {
                    // Calcula a distância entre a unidade e o inimigo atual.
                    float distanceToEnemy = Vector3.Distance(transform.position, enemyUnit.transform.position);
                    // Verifica se o inimigo atual está mais próximo do que o anteriormente selecionado.
                    if (distanceToEnemy < nearestDistance)
                    {
                        nearestDistance = distanceToEnemy;
                        nearestEnemy = enemyUnit;
                    }
                }
            }
        }

        // Se foi encontrado um inimigo próximo, define o destino para a movimentação da unidade.
        if (nearestEnemy != null)
        {
            // Verifica se a unidade já está atacando um inimigo.
            if (!isAttacking || target == null || !target.IsAlive())
            {
                target = nearestEnemy;
                Vector3 directionToEnemy = (target.transform.position - transform.position).normalized;
                Vector3 destination = target.transform.position - directionToEnemy * stoppingDistance;
                navMeshAgent.SetDestination(destination);

            }
        }
    }

    private void InitializeStats()
    {
        unitStats = new UnitStats();
    }
}
