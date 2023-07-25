using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int enemyID;
    public string enemyName;
    public float maxHealth;
    public float attackDamage;
    public float armor;
    public float attackSpeed;
    public float attackRange;
    public float moveSpeed;
    public bool isRanged;
    public int experience;
}
