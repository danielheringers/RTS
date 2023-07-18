using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab;
    public UnitData unitData;
    public ItemData defaultItem; // O item padrão que a unidade começa com.

    public void SpawnUnit(Vector3 spawnPosition)
    {
        GameObject newUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
        UnitController unitController = newUnit.GetComponent<UnitController>();

        unitController.unitData = Instantiate(unitData); // Cria uma cópia única do Scriptable Object.
        unitController.unitData.unitID = UnitIDGenerator.GenerateUniqueID(); // Atribui o ID único à unidade.

        // Adicione o item padrão ao inventário da unidade.
        unitController.unitData.inventoryItems[0] = defaultItem;
    }
}