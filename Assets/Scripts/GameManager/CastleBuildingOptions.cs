using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CastleBuildingOptions : MonoBehaviour, IInteractable
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
        // Obter todos os bot�es dentro do painel de op��es
        Button[] buttons = optionsPanel.GetComponentsInChildren<Button>();

        // Iterar por cada bot�o e verificar o nome
        foreach (Button button in buttons)
        {
            if (button.name == "Construir")
            {
                // Atribuir a fun��o desejada ao bot�o 1
                button.onClick.AddListener(BuildStructure);
            }
        }
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

    }

    public void SetInteractOnClick()
    {
        // Obt�m o componente Button do bot�o do Canvas
        Button button = optionsPanel.GetComponentInChildren<Button>();

        // Configura o bot�o para chamar a fun��o Interact quando clicado
        button.onClick.AddListener(Interact);
    }
}
