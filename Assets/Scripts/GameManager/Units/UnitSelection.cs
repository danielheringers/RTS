using UnityEngine;
using System.Collections.Generic;

public class UnitSelection : MonoBehaviour
{
    public LayerMask unitLayer;

    private List<UnitController> selectedUnits = new List<UnitController>();

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

                    selectedUnits.Add(selectedUnit);
                    
                }
            }
            else
            {

                ClearSelection();
            }
        }
    }


    private void ClearSelection()
    {
        selectedUnits.Clear();
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
