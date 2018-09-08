using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [SerializeField] bool surfaceOnly;

    public Vector3 SpawnPoint
    {
        get
        {
            //Setting it random to the transformpoint so we can increase the radius by scaling the transform up
            //if surfaceonly we use onUnitsphere so it takes only outside values 
            return transform.TransformPoint(surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);
        }
    }
}
