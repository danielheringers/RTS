using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "ScriptableObjects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int attackDamage;
    public int armor;
    public float attackSpeed;
    public float attackRange;
    public float moveSpeed;
    public bool isRanged;
    // Adicione outros atributos específicos do personagem aqui.

    public int currentLevel = 1;
    public int currentExperience = 0;
    public int experienceToNextLevel = 100;

    public void GainExperience(int amount)
    {
        currentExperience += amount;

        if (currentExperience >= experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        maxHealth += 84;
        attackDamage += 3;
        armor += 3;
        attackSpeed += attackSpeed * 0.025f;

        currentExperience -= experienceToNextLevel;

        experienceToNextLevel += 100;
    }
}
