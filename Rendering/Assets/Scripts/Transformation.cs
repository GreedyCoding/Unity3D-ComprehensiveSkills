using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Abstract class we dont use we just inherit from this
public abstract class Transformation : MonoBehaviour
{
    public abstract Vector3 Apply(Vector3 point);
}
