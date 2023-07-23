using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    Camera mainCamera;
    public float rotateSpeedMovement = 0.05f;
    private float rotateVelocity;
    float motionSmoothTime = 0.1f;
    public LayerMask groundLayer;
    private EnemyController targetEnemy;
    private float stoppingDistance;
    private UnitController unitController;
    private HighlightManager highlightManager;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        unitController = GetComponent<UnitController>();
        stoppingDistance = unitController.currentAttackRange;
        highlightManager = GetComponent<HighlightManager>();
    }

    private void Update()
    {
        Animation();

    }
    public void Animation()
    {
        float speed = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
        if(navMeshAgent.velocity.sqrMagnitude > 0.1f )
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("animatorSpeed", speed, motionSmoothTime, Time.deltaTime);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        if(targetEnemy != null)
        {
            if(Vector3.Distance(transform.position, targetEnemy.transform.position) > stoppingDistance)
            {
                navMeshAgent.SetDestination(targetEnemy.transform.position);
            }
        }
    }
    // Define o destino para a movimentação da unidade.
    public void MoveToDestination(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
        navMeshAgent.stoppingDistance = 0;
        Rotation(destination);

        if(targetEnemy != null)
        {
            highlightManager.DeselectHighLight();
            targetEnemy = null;
        }
    }
    public void MoveToEnemy(EnemyController target)
    {
        
        targetEnemy = target;
        navMeshAgent.SetDestination(targetEnemy.transform.position);
        navMeshAgent.stoppingDistance = stoppingDistance;
        Rotation(targetEnemy.transform.position);
        highlightManager.SelectedHighlight();
    }
    public void Rotation(Vector3 lookAtPosition)
    {
        Quaternion rotationToLookAt = Quaternion.LookRotation(lookAtPosition - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
        transform.eulerAngles = new Vector3(0, rotationY, 0);
    }
}
