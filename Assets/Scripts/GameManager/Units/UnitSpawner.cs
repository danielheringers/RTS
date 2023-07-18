using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public GameObject unitPrefab;
    public UnitData unitData;
    public ItemData defaultItem; // O item padr�o que a unidade come�a com.

    public void SpawnUnit(Vector3 spawnPosition)
    {
        GameObject newUnit = Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
        UnitController unitController = newUnit.GetComponent<UnitController>();

        unitController.unitData = Instantiate(unitData); // Cria uma c�pia �nica do Scriptable Object.
        unitController.unitData.unitID = UnitIDGenerator.GenerateUniqueID(); // Atribui o ID �nico � unidade.

        // Adicione o item padr�o ao invent�rio da unidade.
        unitController.unitData.inventoryItems[0] = defaultItem;
    }
}