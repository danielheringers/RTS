using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public UnitData unitData;
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
    private void Start()
    {
        currentHealth = unitData.maxHealth;
        currentDamage = unitData.attackDamage;
        currentArmor = unitData.armor;
        currentAttackSpeed = unitData.attackSpeed;
        currentAttackRange = unitData.attackRange;
        currentMoveSpeed = unitData.moveSpeed;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyLayer = LayerMask.GetMask("Enemy");
        attackCooldown = 1f / unitData.attackSpeed;
        animatorCooldown = 1f * unitData.attackSpeed;
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
    }

    public void CheckForEnemyInRange()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentAttackRange, enemyLayer);

        foreach (Collider col in hitColliders)
        {
            EnemyController enemyUnit = col.GetComponent<EnemyController>();


            if (enemyUnit)
            {
                isAttacking = true;
                AttackEnemy(enemyUnit);
            }
            if (!enemyUnit)
            {
                isAttacking = false;
            }
        }
    }

    public void AttackEnemy(EnemyController enemy)
    {
        // Verifica se o tempo desde o último ataque é maior do que o tempo de intervalo entre ataques.
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Realiza o ataque e aplica dano ao inimigo
            enemy.TakeDamage(unitData.attackDamage);
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
        // ...

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f, enemyLayer);

        foreach (Collider col in hitColliders)
        {
            EnemyController enemyUnit = col.GetComponent<EnemyController>();

            if (enemyUnit != null)
            {
                // Adicione mensagens de debug para verificar a posição dos inimigos e o destino de movimento da unidade.
                Debug.Log("Enemy Detected: " + enemyUnit.name + " at " + enemyUnit.transform.position);

                // Realiza o ataque se estiver dentro da distância de ataque.
                if (Vector3.Distance(transform.position, enemyUnit.transform.position) <= currentAttackRange)
                {
                    AttackEnemy(enemyUnit);
                }
                else
                {
                    // Define o destino para a movimentação da unidade.
                    navMeshAgent.destination = enemyUnit.transform.position;
                }
            }
        }

        // ...
    }

}
