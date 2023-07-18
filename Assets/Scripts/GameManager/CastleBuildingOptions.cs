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

    private bool isBuilding = false; // Flag para controlar se a estrutura está sendo construída



    private void Start()
    {

        isPanelOpen = false;
        // Obter todos os botões dentro do painel de opções
        Button[] buttons = optionsPanel.GetComponentsInChildren<Button>();

        // Iterar por cada botão e verificar o nome
        foreach (Button button in buttons)
        {
            if (button.name == "Construir")
            {
                // Atribuir a função desejada ao botão 1
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
                GameManager.instance.BuildStructure(buildingObject); // Chama a função BuildStructure do GameManager, passando o objeto da estrutura
                // Reseta as variáveis
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
            // Inicia a construção da estrutura
            isBuilding = true;
        }
    }

    public void OpenPostConstructionOptionsPanel()
    {
        optionsPanel.SetActive(false); // Fechar o painel de opções normal
        castlePostConstructionOptionsPanel.SetActive(true); // Abrir o painel de opções pós-construção do castelo
        isPanelOpen = true;
    }
    public void ClosePostConstructionOptionsPanel()
    {
        optionsPanel.SetActive(false); // Fechar o painel de opções normal
        castlePostConstructionOptionsPanel.SetActive(false); // Abrir o painel de opções pós-construção do castelo
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
        // Obtém o componente Button do botão do Canvas
        Button button = optionsPanel.GetComponentInChildren<Button>();

        // Configura o botão para chamar a função Interact quando clicado
        button.onClick.AddListener(Interact);
    }
}
