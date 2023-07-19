using UnityEngine;
using System.Collections.Generic;

public class UnitSelection : MonoBehaviour
{
    public LayerMask unitLayer;

    public List<UnitController> selectedUnits = new List<UnitController>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                UnitController selectedUnit = hit.collider.GetComponent<UnitController>();

                if (selectedUnit != null && !selectedUnits.Contains(selectedUnit))
                {
                    AddToSelection(selectedUnit);
                }
            }
            else
            {
                ClearSelection();
            }
        }
    }

    public void ClearSelection()
    {
        foreach (UnitController unit in selectedUnits)
        {
            unit.SetSelected(false);
        }
        selectedUnits.Clear();
    }

    public void AddToSelection(UnitController unit)
    {
        unit.SetSelected(true);
        selectedUnits.Add(unit);
    }

    public void RemoveFromSelection(UnitController unit)
    {
        unit.SetSelected(false);
        selectedUnits.Remove(unit);
    }

    public List<UnitController> GetSelectedUnits()
    {
        return selectedUnits;
    }

    private void OnDrawGizmos()
    {
        foreach (UnitController unit in selectedUnits)
        {
            if (unit != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(unit.transform.position, 1.5f);
            }
        }
    }
}
