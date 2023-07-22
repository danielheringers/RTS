using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public LayerMask unitLayer;
    public LayerMask enemyLayer;
    public LayerMask groundLayer;
    public Camera mainCamera;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                { 
                    UnitSelection.Instance.ShiftClickSelect(hit.collider.GetComponent<UnitController>());
                }
                else
                {
                    UnitSelection.Instance.ClickSelect(hit.collider.GetComponent<UnitController>());
                }

            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    UnitSelection.Instance.DeselectAll();
                }
                
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 destination = hit.point;
                foreach (UnitController unit in UnitSelection.Instance.selectedUnits)
                {
                    UnitMovement unitMovement = unit.GetComponent<UnitMovement>();

                    if (unitMovement != null)
                    {
                        unit.isAutoBattle = false;
                        unitMovement.MoveToDestination(destination);
                    }
                }
                
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            // Ativar ou desativar o modo de combate automático para todas as unidades selecionadas.
            ToggleAutoBattleForSelectedUnits();
        }

    }
    private void ToggleAutoBattleForSelectedUnits()
    {
        // Percorrer todas as unidades selecionadas e ativar/desativar o modo de combate automático.
        foreach (UnitController unit in UnitSelection.Instance.selectedUnits)
        {
            unit.ActivateAutoBattle(!unit.isAutoBattle);

        }
    }
}
