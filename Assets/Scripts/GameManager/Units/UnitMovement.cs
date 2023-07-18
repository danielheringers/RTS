using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bool isMoving = navMeshAgent.velocity.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);
    }

    // Define o destino para a movimentação da unidade.
    public void MoveToDestination(Vector3 destination)
    {
        navMeshAgent.destination = destination;
    }
}
