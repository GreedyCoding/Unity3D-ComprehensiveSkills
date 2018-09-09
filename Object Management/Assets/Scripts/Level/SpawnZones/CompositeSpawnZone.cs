using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeSpawnZone : SpawnZone
{
    //Setup an array field for the spawnZones we want to use with this spawner
    [SerializeField] SpawnZone[] spawnZones;

    public override Vector3 SpawnPoint
    {
        get
        {
            //Get a random spawnZones index
            int index = Random.Range(0, spawnZones.Length);
            //and return the spawnPoint of the selected spawnZone
            return spawnZones[index].SpawnPoint;
        }
    }
}
