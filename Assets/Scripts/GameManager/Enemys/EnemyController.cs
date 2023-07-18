using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    private int currentHealth;
    private int currentDamage;
    private int currentArmor;
    private float currentAttackSpeed;
    private float currentAttackRange;
    private float currentMoveSpeed;
    public int experienceValue = 50;

    private void Start()
    {
        currentHealth = enemyData.maxHealth;
        currentDamage = enemyData.attackDamage;
        currentArmor = enemyData.armor;
        currentAttackSpeed = enemyData.attackSpeed;
        currentAttackRange = enemyData.attackRange;
        currentMoveSpeed = enemyData.moveSpeed;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }

        UnitController unitController = GetComponent<UnitController>();
        if (unitController != null)
        {
            // Chama o método GainExperience do UnitData da unidade.
            unitController.unitData.GainExperience(experienceValue);
        }

    }

    private void Die()
    {
        // Adicione aqui a lógica para quando o inimigo morrer.
        Destroy(gameObject);
    }

    // Aqui você pode adicionar outros comportamentos para o inimigo, como movimentação, ataque, etc.
}
