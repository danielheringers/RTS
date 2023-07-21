using UnityEngine;

public class AttackRangeCircle : MonoBehaviour
{
    public Material circleMaterial;
    public float radius;
    public int segments = 64;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = circleMaterial;
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = segments + 1;

        UpdateCircle();
    }

    public void UpdateCircle()
    {
        float angle = 0f;
        float angleStep = 2f * Mathf.PI / segments;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Sin(angle) * radius;
            float z = Mathf.Cos(angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));

            angle += angleStep;
        }
    }
}