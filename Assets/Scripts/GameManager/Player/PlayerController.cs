using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public LayerMask unitLayer;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    private UnitController unitController;
    public Camera mainCamera;

    private List<UnitController> selectedUnits = new List<UnitController>();

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                UnitController selectedUnit = hit.collider.GetComponent<UnitController>();

                if (selectedUnit != null)
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        
                        AddToSelection(selectedUnit);
                    }
                    else
                    {
                        
                        ClearSelection();
                        AddToSelection(selectedUnit);
                    }
                }
            }
            else
            {
               
                ClearSelection();
            }
        }


        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, enemyLayer))
            {
                Vector3 destination = hit.point;

                foreach (UnitController unit in selectedUnits)
                {
                    if (unit.isAttacking)
                    {
                        
                        EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                        if (enemy != null && Vector3.Distance(unit.transform.position, enemy.transform.position) <= unit.currentAttackRange)
                        {
                            unit.AttackEnemy(enemy);
                        }
                        else
                        {
                            // Calcula a direção do inimigo para a unidade
                            Vector3 directionToEnemy = (enemy.transform.position - unit.transform.position).normalized;

                            // Calcula a posição de destino para que a unidade fique a uma distância de attackDistance do inimigo
                            Vector3 stopPosition = enemy.transform.position - directionToEnemy * unit.currentAttackRange;

                            // Mantém a altura da unidade na posição de destino
                            stopPosition.y = unit.transform.position.y;

                            unit.GetComponent<UnitMovement>().MoveToDestination(stopPosition);
                            unit.isAttacking = false;
                        }
                    }
                }
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 destination = hit.point;
                foreach (UnitController unit in selectedUnits)
                {
                    if (unit)
                    { 
                        unit.isAttacking = false;
                        unit.GetComponent<UnitMovement>().MoveToDestination(destination);
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

    private void AddToSelection(UnitController unit)
    {
        unit.SetSelected(true);
        selectedUnits.Add(unit);
    }

    private void ClearSelection()
    {
        foreach (UnitController unit in selectedUnits)
        {
            unit.SetSelected(false);
        }
        selectedUnits.Clear();
    }

    private void ToggleAutoBattleForSelectedUnits()
    {
        // Percorrer todas as unidades selecionadas e ativar/desativar o modo de combate automático.
        foreach (UnitController unit in selectedUnits)
        {
            unit.ActivateAutoBattle(!unit.isAutoBattle);
        }
    }
}
