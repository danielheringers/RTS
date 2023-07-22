using UnityEngine;
using System.Collections.Generic;

public class UnitSelection : MonoBehaviour
{
    public LayerMask unitLayer;
    public FormationBase defaultFormation;

    public List<UnitController> unitList = new List<UnitController>();
    public List<UnitController> selectedUnits = new List<UnitController>();

    private static UnitSelection instance;
    public static UnitSelection Instance { get { return instance; } }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void ClickSelect(UnitController unitToAdd)
    {
        DeselectAll();
        selectedUnits.Add(unitToAdd);
        unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
        unitToAdd.GetComponent<UnitMovement>().enabled = true;
    }

    public void ShiftClickSelect(UnitController unitToAdd) 
    {
        if(!selectedUnits.Contains(unitToAdd))
        {
            selectedUnits.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
        else
        {
            unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
            unitToAdd.GetComponent<UnitMovement>().enabled = false;
            selectedUnits.Remove(unitToAdd);
        }
    }
    public void DragSelect(UnitController unitToAdd)
    {
        if (!selectedUnits.Contains(unitToAdd))
        {
            selectedUnits.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in selectedUnits)
        {
            unit.GetComponent<UnitMovement>().enabled = false;
            unit.transform.GetChild(0).gameObject.SetActive(false);
        }
        selectedUnits.Clear();

    }
    public void Deselect(UnitController unitToDeselect)
    {

    }

    public void SetUnitsInFormation()
    {
        // Certifique-se de que há unidades selecionadas e uma formação padrão definida.
        if (selectedUnits.Count == 0 || defaultFormation == null)
        {
            return;
        }
        Debug.Log("SetUnitsInFormation() is called!");
        // Obtenha os pontos de posição da formação padrão.
        List<Vector3> formationPoints = new List<Vector3>(defaultFormation.EvaluatePoints());

        // Verifique se o número de unidades selecionadas é menor ou igual ao número de pontos na formação.
        int numUnits = Mathf.Min(selectedUnits.Count, formationPoints.Count);

        // Posicione as unidades selecionadas na formação.
        for (int i = 0; i < numUnits; i++)
        {
            selectedUnits[i].GetComponent<UnitMovement>().MoveToDestination(formationPoints[i]);
        }
    }

}
