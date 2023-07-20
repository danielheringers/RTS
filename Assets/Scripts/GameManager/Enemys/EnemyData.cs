using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int enemyID;
    public string enemyName;
    public int maxHealth;
    public int attackDamage;
    public int armor;
    public float attackSpeed;
    public float attackRange;
    public float moveSpeed;
    public bool isRanged;
    public int experience;
}
