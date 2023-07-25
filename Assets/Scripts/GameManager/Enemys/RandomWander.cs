using UnityEngine;
using UnityEngine.AI;

public class RandomWander : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderInterval = 5f;

    private Transform spawnPoint;
    private NavMeshAgent navMeshAgent;
    private float timer;

    private void Start()
    {
        spawnPoint = transform.parent; // O spawn point deve ser o pai do inimigo.
        navMeshAgent = GetComponent<NavMeshAgent>();
        timer = wanderInterval;
    }

    private void Update()
    {
        // Se o inimigo n�o tiver um alvo, movimente-se aleatoriamente.
        if (navMeshAgent.enabled && !navMeshAgent.hasPath)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
                randomDirection += spawnPoint.position;
                NavMeshHit navHit;

                // Encontra a posi��o v�lida mais pr�xima para onde o inimigo deve se mover.
                NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, NavMesh.AllAreas);

                // Define o destino para o movimento aleat�rio.
                navMeshAgent.SetDestination(navHit.position);

                timer = wanderInterval;
            }
        }
    }
}
