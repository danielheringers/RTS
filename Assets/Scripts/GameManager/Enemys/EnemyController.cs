using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    private int currentHealth;
    public int experienceValue;
    private bool isAlive = true;
    public UnityEvent<int> OnDeath = new UnityEvent<int>(); // Evento com argumento int para passar a experiência ao evento.

    private void Start()
    {
        currentHealth = this.enemyData.maxHealth;
        experienceValue = this.enemyData.experience;
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isAlive)
            return;
        currentHealth -= damageAmount - enemyData.armor / (enemyData.armor + 100);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        OnDeath.Invoke(experienceValue); // Invocamos o evento passando o valor de experiência.
        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return isAlive;
    }
}
