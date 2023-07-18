using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingOptions : MonoBehaviour
{
    public GameObject optionsPanel;

    private bool isPanelOpen;
    public GameObject buildingObject;
    private MeshRenderer meshRenderer;
    public bool IsConstructed;

    private bool isBuilding = false; // Flag para controlar se a estrutura está sendo construída



    private void Start()
    {
        
        isPanelOpen = false;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            buildingObject = this.gameObject;

        }

        if (isBuilding && !IsConstructed)
        {
            if(!IsConstructed)
            { 
                GameManager.instance.BuildStructure(buildingObject); // Chama a função BuildStructure do GameManager, passando o objeto da estrutura
                // Reseta as variáveis
                isBuilding = false;
                
            }
            Debug.Log("Construção já feita");

        }
    }

    private void OnMouseDown()
    {
        if(isPanelOpen == false)
        {
            OpenOptionsPanel();
        }
    }



    private void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);
        isPanelOpen = true;
        Debug.Log("Abriu Pela função");
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
}
