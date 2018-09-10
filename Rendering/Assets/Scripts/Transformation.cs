using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class to inherit all transformations from this
public abstract class Transformation : MonoBehaviour
{
    //Matrix to store the transformation
    public abstract Matrix4x4 Matrix { get; }

    public Vector3 Apply(Vector3 point)
    {
        //Grabs the Matrix and performs the multiplication
        return Matrix.MultiplyPoint(point);
    }
}
