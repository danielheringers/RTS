using System.Collections;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public float maxHeight = 3f;
    public float heightDuration = 1f;
    public float destroyDelay = 1f;

    private Transform source;
    private Vector3 initialPosition;
    private bool isMoving = false;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetSource(Transform source)
    {
        this.source = source;
    }

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (target != null && !isMoving)
        {
            StartCoroutine(MoveToTarget());
        }

    }

    private IEnumerator MoveToTarget()
    {
        isMoving = true;

        float time = 0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = target.position;

        while (time < 1f)
        {
            if (target == null)
            {
                Destroy(gameObject);
                yield break;
            }

            time += Time.deltaTime * speed;


            float height = Mathf.Sin(Mathf.PI * time) * maxHeight;


            Vector3 newPosition = Vector3.Lerp(initialPosition, targetPosition, time);
            newPosition.y += height;

            transform.position = newPosition;


            transform.LookAt(targetPosition);

            yield return null;
        }


        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}
