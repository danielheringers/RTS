using Unity.VisualScripting;

[System.Serializable]
public class UnitStats
{
    public int maxHealth = 100;
    public int attackDamage = 100;
    public int armor = 23;
    public float attackSpeed = 0.65f;
    public float attackRange = 1.5f;
    public float moveSpeed = 10f;
    // Outros atributos que você deseja adicionar...

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
        attackDamage += 100;
        armor += 3;
        attackSpeed += attackSpeed * 0.025f;

        currentExperience -= experienceToNextLevel;

        experienceToNextLevel += 100;
    }
}
