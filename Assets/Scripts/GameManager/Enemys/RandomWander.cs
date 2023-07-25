using UnityEngine;
using UnityEngine.AI;

public class RandomWander : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderInterval = 5f;
    private EnemyController enemyController;
    private Transform spawnPoint;
    private NavMeshAgent navMeshAgent;
    private float timer;

    private void Start()
    {
        spawnPoint = transform.parent; // O spawn point deve ser o pai do inimigo.
        navMeshAgent = GetComponent<NavMeshAgent>();
        timer = wanderInterval;
        enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        // Se o inimigo não tiver um alvo, movimente-se aleatoriamente.
        if (navMeshAgent.enabled && !navMeshAgent.hasPath)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f && enemyController.inBattle == false)
            {
                Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += spawnPoint.position;
                NavMeshHit navHit;

                // Encontra a posição válida mais próxima para onde o inimigo deve se mover.
                NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, NavMesh.AllAreas);

                // Define o destino para o movimento aleatório.
                navMeshAgent.SetDestination(navHit.position);

                timer = wanderInterval;
            }
        }
    }
}
