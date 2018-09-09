using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTransformation : Transformation
{
    [SerializeField] private Vector3 scale;

    public override Vector3 Apply(Vector3 point)
    {
        point.x *= scale.x;
        point.y *= scale.y;
        point.z *= scale.z;
        return point;
    }
}
