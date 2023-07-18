using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeepBuildingOptions : MonoBehaviour, IInteractable
{
    public GameObject optionsPanel;
    public GameObject castlePostConstructionOptionsPanel;

    private bool isPanelOpen;
    public GameObject buildingObject;
    public bool IsConstructed;

    private bool isBuilding = false; // Flag para controlar se a estrutura est� sendo constru�da



    private void Start()
    {

        isPanelOpen = false;

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            buildingObject = this.gameObject;

        }

        if (isBuilding && !IsConstructed)
        {
            if (!IsConstructed)
            {
                GameManager.instance.BuildStructure(buildingObject); // Chama a fun��o BuildStructure do GameManager, passando o objeto da estrutura
                // Reseta as vari�veis
                isBuilding = false;
                IsConstructed = true;

            }
        }
    }

    private void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);
        isPanelOpen = true;
    }

    public void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);
        isPanelOpen = false;
    }

    public void BuildStructure()
    {
        if (!isBuilding)
        {
            // Inicia a constru��o da estrutura
            isBuilding = true;
        }
    }

    public void OpenPostConstructionOptionsPanel()
    {
        optionsPanel.SetActive(false); // Fechar o painel de op��es normal
        castlePostConstructionOptionsPanel.SetActive(true); // Abrir o painel de op��es p�s-constru��o do castelo
        isPanelOpen = true;
    }
    public void ClosePostConstructionOptionsPanel()
    {
        optionsPanel.SetActive(false); // Fechar o painel de op��es normal
        castlePostConstructionOptionsPanel.SetActive(false); // Abrir o painel de op��es p�s-constru��o do castelo
        isPanelOpen = false;
    }

    public void Interact()
    {
        if (!IsConstructed && !isPanelOpen)
        {
            OpenOptionsPanel();
        }
        else if (!IsConstructed && isPanelOpen)
        {
            CloseOptionsPanel();
        }
        else if (IsConstructed && !isPanelOpen)
        {
            OpenPostConstructionOptionsPanel();
        }
        else
        {
            ClosePostConstructionOptionsPanel();
        }
    }

    public void HideCanvas()
    {
        CloseOptionsPanel();
        ClosePostConstructionOptionsPanel();
    }

}
