using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    private int currentHealth;
    public int experienceValue = 50;
    private bool isAlive = true;
    private void Start()
    {
        currentHealth = enemyData.maxHealth;
        Debug.Log(currentHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isAlive)
            return;
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
        isAlive = false;
        Destroy(gameObject);
    }
    public bool IsAlive()
    {
        return isAlive;
    }

    // Aqui você pode adicionar outros comportamentos para o inimigo, como movimentação, ataque, etc.
}
