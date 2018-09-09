using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawnZone : SpawnZone
{
    //Toggle on to only spawn shapes on the surface
    [SerializeField] bool surfaceOnly;

    //Overriding the Spawnpoint function from the Spawnzone
    public override Vector3 SpawnPoint
    {
        get
        {
            //Applying the entire transformation of the inspector transform of the spawnpoint by using transform.transformpoint
            //If surfaceonly use onUnitSphere else use insideUnitSphere to get the points
            return transform.TransformPoint(surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);
        }
    }

    void OnDrawGizmos()
    {
        //Set the color of the gizmos to cyan
        Gizmos.color = Color.cyan;
        //Set the matrix of the gizmo to the transform from the inspector
        Gizmos.matrix = transform.localToWorldMatrix;
        //And draw the sphere
        Gizmos.DrawWireSphere(Vector3.zero, 1f);
    }
}
