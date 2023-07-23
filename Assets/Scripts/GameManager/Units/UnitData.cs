using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitData", menuName = "Unit Data")]
public class UnitData : ScriptableObject
{
    public int unitID; 
    public string unitName;
    public int maxHealth;
    public int attackDamage;
    public int armor;
    public float attackSpeed;
    public float attackRange;
    public float moveSpeed;
    public bool isRanged;


    public ItemData[] inventoryItems = new ItemData[6]; 
    public RuntimeAnimatorController animatorController;

    
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
        currentExperience -= experienceToNextLevel;

        experienceToNextLevel += 100;

    }
}
