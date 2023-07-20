using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    private int currentHealth;
    public int experienceValue = 50;

    private void Start()
    {
        currentHealth = enemyData.maxHealth;
        Debug.Log(currentHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log(damageAmount);
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
