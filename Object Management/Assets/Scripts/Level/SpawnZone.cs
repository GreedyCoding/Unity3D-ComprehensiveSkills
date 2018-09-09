using UnityEngine;

//Spawnzone class use for overriding
public abstract class SpawnZone : MonoBehaviour
{
    //With a vector3 as the Spawnpoint
    public abstract Vector3 SpawnPoint { get; }
}
