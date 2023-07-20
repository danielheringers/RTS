using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab; // Prefab da árvore
    public int numberOfTrees = 10; // Número de árvores a serem geradas
    public float spawnRadius = 10f; // Raio da área de spawn das árvores

    private void Start()
    {
        // Chama a função para gerar as árvores
        SpawnTrees();
    }

    private void SpawnTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            // Gera uma posição aleatória dentro do raio especificado
            Vector3 randomPosition = GetRandomPosition();

            // Instancia a árvore no local gerado
            GameObject tree = Instantiate(treePrefab, randomPosition, Quaternion.identity);

            // Define a rotação aleatória da árvore em torno do eixo Y
            tree.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }
    }

    private Vector3 GetRandomPosition()
    {
        // Gera coordenadas aleatórias dentro do raio definido
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);

        // Retorna a posição aleatória
        return new Vector3(randomX, 0f, randomZ) + transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha uma esfera gizmo para representar a área de spawn no editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
