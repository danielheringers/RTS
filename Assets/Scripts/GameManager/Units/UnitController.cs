using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public UnitData unitData;
    private int currentHealth;
    public int currentDamage;
    public int currentArmor;
    public float currentAttackSpeed;
    public float currentAttackRange;
    public float currentMoveSpeed;
    public bool isRanged;
    private bool isSelected;

    private float attackCooldown;
    private float lastAttackTime = 0.0f;
    public NavMeshAgent navMeshAgent;
    public Camera mainCamera;
    public UnitSelection unitSelection;
    public LayerMask groundLayer;
    private Animator animator;

    private bool isAttacking;
    private EnemyController targetEnemy;
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
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                
                if (unitSelection.GetSelectedUnits().Count > 0)
                {

                    UnitController selectedUnit = unitSelection.GetSelectedUnits()[0];

                   
                    EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                    if (enemy != null)
                    {
                       
                        selectedUnit.GetComponent<UnitMovement>().MoveToDestination(hit.point);
                        animator.SetBool("isAttacking", true);
                        return; 
                    }
                }

                
                navMeshAgent.destination = hit.point;
                animator.SetBool("isAttacking", false);
            }
        }
    }

    private void AttackEnemy(EnemyController enemy)
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
