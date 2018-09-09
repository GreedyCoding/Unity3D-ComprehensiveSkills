using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnZone : SpawnZone
{
    [SerializeField] bool surfaceOnly;
    
    public override Vector3 SpawnPoint
    {
        get
        {
            Vector3 point;
            //Cube is centered at the origin with a half a unit to the left and half a unit to the right
            //so we get a random range between -0.5f and 0.5f to get a random x,y,z coordinate
            point.x = Random.Range(-0.5f, 0.5f);
            point.y = Random.Range(-0.5f, 0.5f);
            point.z = Random.Range(-0.5f, 0.5f);
            if (surfaceOnly)
            {
                //if we want to draw on the surface only we select a random axis of the point
                int axis = Random.Range(0, 3);
                //If the points axis value is lower then 0 shift it to the left if its bigger shift it to the right
                point[axis] = point[axis] < 0f ? -0.5f : 0.5f;
            }
            //Use the point vector to set the transform to the random positon
            return transform.TransformPoint(point);
        }
    }

    void OnDrawGizmos()
    {
        //Set the color of the gizmos to cyan
        Gizmos.color = Color.cyan;
        //Set the matrix of the gizmo to the transform from the inspector
        Gizmos.matrix = transform.localToWorldMatrix;
        //And draw the cube
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
