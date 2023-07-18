using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private float constructionTimer = 0f;
    private float constructionTime = 15f;
    private bool isCompleted;
    private Queue<GameObject> constructionQueue;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        constructionQueue = new Queue<GameObject>();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Mapa1");
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void BuildStructure(GameObject selectedObject)
    {
        

        // Verificar se j� h� uma constru��o em andamento
        if (constructionQueue.Count > 0 || isCompleted)
        {
            Debug.Log("J� h� uma constru��o em andamento. Aguarde a finaliza��o.");
            return;
        }

        // Adicionar a estrutura � fila de constru��o
        constructionQueue.Enqueue(selectedObject);

        // Iniciar a constru��o
        StartCoroutine(ConstructionTimer());

        Debug.Log("Construindo estrutura para o objeto: " + selectedObject.name);
    }

    private IEnumerator ConstructionTimer()
    {
        while (constructionQueue.Count > 0)
        {
            GameObject selectedObject = constructionQueue.Peek();

            constructionTimer = constructionTime;

            // Obter a lista de objetos filhos do objeto selecionado
            List<GameObject> childObjects = GetChildObjects(selectedObject);

            // Ativar o primeiro objeto filho
            ActivateChildObject(childObjects, 0);

            while (constructionTimer > 0f)
            {
                yield return new WaitForSeconds(1f);
                constructionTimer--;

                // Verificar se � o momento de ativar o pr�ximo objeto filho
                if (constructionTimer == Mathf.FloorToInt(constructionTime / 2f))
                {
                    ActivateChildObject(childObjects, 1);
                }
                else if (constructionTimer == 1)
                {
                    // No �ltimo segundo, ativar o �ltimo objeto filho e desativar o pen�ltimo
                    ActivateChildObject(childObjects, 2);
                    DeactivateChildObject(childObjects, 1);
                }
            }

            // Desativar o primeiro objeto filho
            DeactivateChildObject(childObjects, 0);
            isCompleted = true;
            // Remover a estrutura da fila de constru��o
            constructionQueue.Dequeue();

            Debug.Log("Estrutura constru�da para o objeto: " + selectedObject.name);
        }
        isCompleted = false;
    }


    private List<GameObject> GetChildObjects(GameObject parentObject)
    {
        List<GameObject> childObjects = new List<GameObject>();

        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            Transform child = parentObject.transform.GetChild(i);
            childObjects.Add(child.gameObject);
        }

        return childObjects;
    }

    private void ActivateChildObject(List<GameObject> childObjects, int index)
    {
        if (index >= 0 && index < childObjects.Count)
        {
            childObjects[index].SetActive(true);
        }
    }

    private void DeactivateChildObject(List<GameObject> childObjects, int index)
    {
        if (index >= 0 && index < childObjects.Count)
        {
            childObjects[index].SetActive(false);
        }
    }


}
