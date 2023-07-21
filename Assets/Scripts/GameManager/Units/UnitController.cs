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
    public CharacterData characterData;
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
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    private EnemyController target;
    private int experienceToReceive;
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
        attackCooldown = 1f / characterData.attackSpeed;
        animatorCooldown = 1f * characterData.attackSpeed;

        // Busca por todos os inimigos presentes na cena e adiciona o ouvinte ao evento OnDeath.
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemies)
        {
            enemy.OnDeath.AddListener(ReceiveExperience);
        }
    }

    private void ReceiveExperience(int experienceReceived)
    {
        characterData.GainExperience(experienceReceived);
    }


    private void Update()
    {
        if (!isRanged)
        {
            CheckForEnemyInRange();
        }
        else
        {
            CheckForEnemyInRangeisRange();
        }
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
        currentHealth = characterData.maxHealth;
        currentDamage = characterData.attackDamage;
        currentArmor = characterData.armor;
        currentAttackSpeed = characterData.attackSpeed;
        currentAttackRange = characterData.attackRange;
        currentMoveSpeed = characterData.moveSpeed;
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

    public void CheckForEnemyInRangeisRange()
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
                    if (distanceToEnemy <= currentAttackRange)
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
        if (isRanged)
        {
            // Verifica se o tempo desde o último ataque é maior do que o tempo de intervalo entre ataques.
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                // Realiza o ataque e aplica dano ao inimigo
                if (isRanged)
                {
                    // Se o personagem for ranged, atira um projétil
                    ShootProjectile(enemy);
                    enemy.TakeDamage(characterData.attackDamage);
                }

                // Verifica se o inimigo foi derrotado
                if (!enemy.IsAlive())
                {
                    // Define o estado de ataque como false e limpa o alvo atual.
                    isAttacking = false;
                    target = null;

                    // Remova a referência do alvo no projétil.
                    if (projectilePrefab != null)
                    {
                        ProjectileScript projectileScript = projectilePrefab.GetComponent<ProjectileScript>();
                        if (projectileScript != null)
                        {
                            projectileScript.SetTarget(null);
                        }
                    }
                }

                // Atualiza o tempo do último ataque.
                lastAttackTime = Time.time;
            }
        }
        else
        {
            // Verifica se o tempo desde o último ataque é maior do que o tempo de intervalo entre ataques.
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                // Realiza o ataque e aplica dano ao inimigo
                enemy.TakeDamage(characterData.attackDamage);

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
    }
    private void ShootProjectile(EnemyController target)
    {
        // Verifica se o prefab do projétil está configurado
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab not set on " + gameObject.name);
            return;
        }

        // Instancia o projétil no ponto de instância do projétil
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Obtém um componente do projétil (você precisa definir seu script para movimentar o projétil e causar dano)
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();

        // Define o alvo para o projétil (isso permite que o projétil saiba quem ele deve perseguir)
        projectileScript.SetTarget(target.transform);

        // Define a fonte do projétil (a unidade que está atirando)
        projectileScript.SetSource(transform);

        // Adicione outras configurações específicas do projétil aqui, se necessário.
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
}
