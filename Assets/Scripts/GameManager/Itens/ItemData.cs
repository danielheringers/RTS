using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data")]
public class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;
    public Sprite itemIcon;
    public int health;
    public int attackDamage;
    public int armor;
    public float attackSpeed;
    public float attackRange;
    public float moveSpeed;

    public float passivedef;
    public float passiveAtk;
    // Adicione mais atributos do item aqui, se necessário.
}
