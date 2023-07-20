using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab; // Prefab da �rvore
    public int numberOfTrees = 10; // N�mero de �rvores a serem geradas
    public float spawnRadius = 10f; // Raio da �rea de spawn das �rvores

    private void Start()
    {
        // Chama a fun��o para gerar as �rvores
        SpawnTrees();
    }

    private void SpawnTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            // Gera uma posi��o aleat�ria dentro do raio especificado
            Vector3 randomPosition = GetRandomPosition();

            // Instancia a �rvore no local gerado
            GameObject tree = Instantiate(treePrefab, randomPosition, Quaternion.identity);

            // Define a rota��o aleat�ria da �rvore em torno do eixo Y
            tree.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }
    }

    private Vector3 GetRandomPosition()
    {
        // Gera coordenadas aleat�rias dentro do raio definido
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);

        // Retorna a posi��o aleat�ria
        return new Vector3(randomX, 0f, randomZ) + transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        // Desenha uma esfera gizmo para representar a �rea de spawn no editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
