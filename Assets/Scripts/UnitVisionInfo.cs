using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitVisionInfo
{
    public List<Vector3> borderPoints;
    public Vector3 unitPosition;
    public float viewDistance;
    public UnitVisionInfo(List<Vector3> borderPoints, Vector3 unitPosition, float viewDistance)
    {
        this.borderPoints = borderPoints;
        this.unitPosition = unitPosition;
        this.viewDistance = viewDistance;
    }
}