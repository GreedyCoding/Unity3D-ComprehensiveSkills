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
            point.x = Random.Range(-0.5f, 0.5f);
            point.y = Random.Range(-0.5f, 0.5f);
            point.z = Random.Range(-0.5f, 0.5f);
            if (surfaceOnly)
            {
                int axis = Random.Range(0, 3);
                point[axis] = point[axis] < 0f ? -0.5f : 0.5f;
            }
            return transform.TransformPoint(point);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
