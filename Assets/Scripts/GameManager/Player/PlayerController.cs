using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public LayerMask unitLayer;
    public LayerMask enemyLayer;
    public LayerMask groundLayer;
    public Camera mainCamera;
    EnemyController enemyUnit;


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

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
            {
                foreach (UnitController unit in UnitSelection.Instance.selectedUnits)
                {
                    UnitMovement unitMovement = unit.GetComponent<UnitMovement>();
                    enemyUnit = hit.collider.GetComponent<EnemyController>();
                    if (unitMovement != null)
                    {
                        Debug.Log("Enemy");
                        unit.targetController = enemyUnit;
                        unit.target = hit.collider.GameObject();
                        unitMovement.MoveToEnemy(enemyUnit);
                    }
                }
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
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

    }

}
