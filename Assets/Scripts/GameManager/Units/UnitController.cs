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
    public float stoppingDistance;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    private bool hasLaunchedProjectile = false;
    public int frameToLaunchProjectile = 10;
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
        stoppingDistance = currentAttackRange;
    }

    private void ReceiveExperience(int experienceReceived)
    {
        characterData.GainExperience(experienceReceived);
    }


    private void Update()
    {
        if (!isRanged && isAutoBattle)
        {
            CheckForEnemyInRange();
        }
        else if(isRanged && isAutoBattle)
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
            // Se estiver no modo de combate autom�tico, procurar inimigos e atacar automaticamente.
            PerformAutoBattle();
        }
        currentHealth = characterData.maxHealth;
        currentDamage = characterData.attackDamage;
        currentArmor = characterData.armor;
        currentAttackSpeed = characterData.attackSpeed;
        currentAttackRange = characterData.attackRange;
        currentMoveSpeed = characterData.moveSpeed;

        // Verifica se a animação de ataque está sendo reproduzida
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("archer_04_attack_A"))
        {
            // Calcula o tempo atual da animação em frames
            float currentFrame = animator.GetCurrentAnimatorStateInfo(0).normalizedTime * animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;

            // Verifica se o 10º frame da animação foi alcançado e se o projetil ainda não foi lançado
            if (currentFrame >= frameToLaunchProjectile && !hasLaunchedProjectile)
            {
                hasLaunchedProjectile = true;

            }
        }
        else
        {
            // Reseta o flag quando a animação não estiver sendo reproduzida
            hasLaunchedProjectile = false;
        }
    }
    public void CheckForEnemyInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentAttackRange, enemyLayer);

        bool hasValidTarget = false; // Vari�vel para indicar se a unidade encontrou um alvo v�lido.

        foreach (Collider col in hitColliders)
        {
            EnemyController enemyUnit = col.GetComponent<EnemyController>();

            if (enemyUnit && enemyUnit.IsAlive())
            {
                // Calcula a dist�ncia entre a unidade e o inimigo atual.
                float distanceToEnemy = Vector3.Distance(transform.position, enemyUnit.transform.position);

                // Verifica se o inimigo atual est� dentro da dist�ncia de ataque.
                if (distanceToEnemy <= currentAttackRange)
                {
                    // Define o estado de ataque como true.
                    isAttacking = true;

                    // Ataca o inimigo somente quando a unidade estiver pr�xima o suficiente.
                    if (distanceToEnemy <= stoppingDistance)
                    {
                        AttackEnemy(enemyUnit);
                    }

                    hasValidTarget = true; // Indica que a unidade encontrou um alvo v�lido para atacar.
                    break; // Sai do loop, pois j� encontramos um alvo v�lido.
                }
            }
        }

        // Se a unidade n�o encontrou nenhum alvo v�lido dentro da dist�ncia de ataque, desativa o estado de ataque.
        if (!hasValidTarget)
        {
            isAttacking = false;
        }
    }

    public void CheckForEnemyInRangeisRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentAttackRange, enemyLayer);

        bool hasValidTarget = false; // Vari�vel para indicar se a unidade encontrou um alvo v�lido.

        foreach (Collider col in hitColliders)
        {
            EnemyController enemyUnit = col.GetComponent<EnemyController>();

            if (enemyUnit && enemyUnit.IsAlive())
            {
                // Calcula a dist�ncia entre a unidade e o inimigo atual.
                float distanceToEnemy = Vector3.Distance(transform.position, enemyUnit.transform.position);

                // Verifica se o inimigo atual est� dentro da dist�ncia de ataque.
                if (distanceToEnemy <= currentAttackRange)
                {
                    // Define o estado de ataque como true.
                    isAttacking = true;

                    // Ataca o inimigo somente quando a unidade estiver pr�xima o suficiente.
                    if (distanceToEnemy <= currentAttackRange)
                    {
                        AttackEnemy(enemyUnit);
                    }

                    hasValidTarget = true; // Indica que a unidade encontrou um alvo v�lido para atacar.
                    break; // Sai do loop, pois j� encontramos um alvo v�lido.
                }
            }
        }

        // Se a unidade n�o encontrou nenhum alvo v�lido dentro da dist�ncia de ataque, desativa o estado de ataque.
        if (!hasValidTarget)
        {
            isAttacking = false;
        }
    }

    public void AttackEnemy(EnemyController enemy)
    {

        // Verifica se o tempo desde o �ltimo ataque � maior do que o tempo de intervalo entre ataques.
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            // Realiza o ataque e aplica dano ao inimigo
            if (isRanged)
            {
                // Se o personagem for ranged, atira um proj�til
                ShootProjectile(enemy);
            }
            else
            {
                enemy.TakeDamage(characterData.attackDamage);
            }

            // Verifica se o inimigo foi derrotado
            if (!enemy.IsAlive())
            {
                // Define o estado de ataque como false e limpa o alvo atual.
                isAttacking = false;
                target = null;

                // Remova a refer�ncia do alvo no proj�til.
                if (projectilePrefab != null)
                {
                    ProjectileScript projectileScript = projectilePrefab.GetComponent<ProjectileScript>();
                    if (projectileScript != null)
                    {
                        projectileScript.SetTarget(null);
                    }
                }
            }

            // Atualiza o tempo do �ltimo ataque.
            lastAttackTime = Time.time;
        }
    }
    private void ShootProjectile(EnemyController target)
    {
        // Verifica se o prefab do proj�til est� configurado
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab not set on " + gameObject.name);
            return;
        }

        // Instancia o proj�til no ponto de inst�ncia do proj�til
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        // Obt�m um componente do proj�til (voc� precisa definir seu script para movimentar o proj�til e causar dano)
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();

        // Define o alvo para o proj�til (isso permite que o proj�til saiba quem ele deve perseguir)
        projectileScript.SetTarget(target.transform);

        // Define a fonte do proj�til (a unidade que est� atirando)
        projectileScript.SetSource(transform);

        if (projectileScript.target == null)
        {
            Destroy(projectile);
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
                // Verifica se a unidade j� est� atacando um inimigo.
                if (isAttacking)
                {
                    // Se a unidade j� est� atacando, continua a atacar o inimigo atual.
                    if (target == enemyUnit)
                    {
                        AttackEnemy(enemyUnit);
                    }
                }
                else
                {
                    // Calcula a dist�ncia entre a unidade e o inimigo atual.
                    float distanceToEnemy = Vector3.Distance(transform.position, enemyUnit.transform.position);
                    // Verifica se o inimigo atual est� mais pr�ximo do que o anteriormente selecionado.
                    if (distanceToEnemy < nearestDistance)
                    {
                        nearestDistance = distanceToEnemy;
                        nearestEnemy = enemyUnit;
                    }
                }
            }
        }

        // Se foi encontrado um inimigo pr�ximo, define o destino para a movimenta��o da unidade.
        if (nearestEnemy != null)
        {
            // Verifica se a unidade j� est� atacando um inimigo.
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