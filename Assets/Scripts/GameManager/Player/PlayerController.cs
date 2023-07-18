using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public UnitSelection unitSelection;
    public LayerMask groundLayer;
    public Camera mainCamera; // Referência à câmera principal do jogo.

    private void Update()
    {
        // Verifica se o jogador pressionou o botão direito do mouse.
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Verifica se o clique atingiu o chão.
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 destination = hit.point;

                // Move todas as unidades selecionadas para o destino.
                foreach (UnitController unit in unitSelection.GetSelectedUnits())
                {
                    unit.GetComponent<UnitMovement>().MoveToDestination(destination);
                }
            }
        }
    }
}
