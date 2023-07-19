using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public UnitData unitData;
    public int currentHealth;
    public int currentDamage;
    public int currentArmor;
    public float currentAttackSpeed;
    public float currentAttackRange;
    public float currentMoveSpeed;
    public bool isRanged;
    public bool isSelected;

    public float attackCooldown;
    public float lastAttackTime = 0.0f;
    public NavMeshAgent navMeshAgent;
    public Camera mainCamera;
    public UnitSelection unitSelection;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public Animator animator;
    public bool isAttacking;
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

        attackCooldown = 1f / unitData.attackSpeed;
    }

    private void Update()
    {
        CheckForEnemyInRange();
    }

    public bool CheckForEnemyInRange()
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentAttackRange, enemyLayer);

        foreach (Collider col in hitColliders)
        {
            EnemyController enemyUnit = col.GetComponent<EnemyController>();

            
            if (enemyUnit != null)
            {
                return isAttacking = true;
                Debug.Log("Atacando");
            }
        }

        return isAttacking = false;
    }

    public void AttackEnemy(EnemyController enemy)
    {
        // Realiza o ataque e aplica dano ao inimigo.
        enemy.TakeDamage(unitData.attackDamage);

        // Atualiza o tempo do último ataque.
        lastAttackTime = Time.time;
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
}
