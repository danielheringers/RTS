using System.Collections.Generic;
using UnityEngine;

public class LineFormation : FormationBase
{
    [SerializeField] private int _numUnits = 5;
    [SerializeField] private float _spacing = 2f;

    public override IEnumerable<Vector3> EvaluatePoints()
    {
        List<Vector3> formationPoints = new List<Vector3>();

        // Calculate the middle position of the formation
        Vector3 middleOffset = Vector3.zero;
        if (_numUnits > 1)
        {
            middleOffset = new Vector3((_numUnits - 1) * 0.5f * _spacing, 0f, 0f);
        }

        for (int i = 0; i < _numUnits; i++)
        {
            Vector3 pos = new Vector3(i * _spacing, 0f, 0f);

            pos -= middleOffset;

            pos += GetNoise(pos);

            pos *= Spread;

            formationPoints.Add(pos);
        }

        return formationPoints;
    }
}
