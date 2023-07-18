using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MouseTargeting : MonoBehaviour
{
    public LayerMask targetLayer;
    private Camera mainCamera;
    [SerializeField]
    private List<BuildingOptions> buildingOptionsList;
    private BuildingOptions currentTarget;

    private void Start()
    {
        mainCamera = Camera.main;
        buildingOptionsList = new List<BuildingOptions>(FindObjectsOfType<BuildingOptions>());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer))
            {
                // Verificar se o objeto clicado é um botão
                Button clickedButton = hit.collider.GetComponent<Button>();
                if (clickedButton != null)
                {
                    // Não ocultar o canvas se foi clicado em um botão
                    return;
                }

                BuildingOptions newTarget = hit.collider.GetComponent<BuildingOptions>();

                // Verificar se houve mudança de alvo
                if (newTarget != currentTarget)
                {
                    // Ocultar o canvas do alvo anterior
                    HidePreviousCanvas();

                    // Armazenar o novo alvo
                    currentTarget = newTarget;

                    // Exibir o canvas do novo alvo, se disponível
                    ShowCanvasForTarget();
                }
            }
            else
            {
                // Se não houver alvo, ocultar o canvas
                HidePreviousCanvas();
            }
        }
    }

    private void ShowCanvasForTarget()
    {
        if (currentTarget != null)
        {
            currentTarget.Interact();
        }
    }

    private void HidePreviousCanvas()
    {
        if (currentTarget != null)
        {
            currentTarget.HideCanvas();
            currentTarget = null;
        }
    }
}
