using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public float rotateSpeedMovement = 0.05f;
    private float rotateVelocity;
    float motionSmoothTime = 0.1f;
    private UnitController targetEnemy;
    private float stoppingDistance;
    private HighlightManager highlightManager;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stoppingDistance = GetComponent<EnemyController>().currentAttackRange;
        highlightManager = GetComponent<HighlightManager>();
        targetEnemy = GetComponent<EnemyController>().target;
    }

    private void Update()
    {
        Animation();

        float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
        if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("animatorSpeed", speed, motionSmoothTime, Time.deltaTime);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        if (targetEnemy != null)
        {
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) > stoppingDistance)
            {
                navMeshAgent.SetDestination(targetEnemy.transform.position);
            }
        }

    }

    public void Animation()
    {
        float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
        animator.SetBool("isMoving", navMeshAgent.velocity.sqrMagnitude > 0.1f);
        animator.SetFloat("animatorSpeed", speed, motionSmoothTime, Time.deltaTime);
    }

    // Define o destino para a movimentação da unidade.
    public void MoveToDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
        navMeshAgent.stoppingDistance = 0;
        if (targetEnemy != null)
        {
            highlightManager.DeselectHighLight();
            targetEnemy = null;
        }
    }

    // Define o alvo para a movimentação da unidade (unidade alvo ou ponto de patrulha).
    public void SetTarget(Transform target)
    {
        targetEnemy = target.GetComponent<UnitController>();
        navMeshAgent.stoppingDistance = stoppingDistance;
        highlightManager.SelectedHighlight();
    }

    public void Rotation(Vector3 lookAtPosition)
    {
        Vector3 lookAtDirection = lookAtPosition - transform.position;
        lookAtDirection.y = 0f; // Para evitar inclinação na rotação
        if (lookAtDirection != Vector3.zero)
        {
            transform.forward = lookAtDirection.normalized;
        }
    }
}
