using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleonSpawner : MonoBehaviour
{
    //Adjustable through inspector
    public float timeBetweenSpawns;
    public float spawnDistance;
    //Array for the Prefabs in the inspector
    public Nucleon[] nucleonPrefabs;

    //Keeping track of last spawn
    private float timeSinceLastSpawn;

    private void FixedUpdate()
    {
        //Add fixedDeltaTime to time since last spawn
        timeSinceLastSpawn += Time.fixedDeltaTime;
        //If the time since last spawn is bigger then the time between spawns
        if (timeSinceLastSpawn >= timeBetweenSpawns)
        {
            //Spawn a nucleon
            SpawnNucleon();
            //and reset the time since last spawn back to zero
            timeSinceLastSpawn = 0f;
        }
    }

    void SpawnNucleon()
    {
        //Getting a random nucleon from our prefab array
        Nucleon prefab = nucleonPrefabs[Random.Range(0, nucleonPrefabs.Length)];
        //Instantiting a Nucleon with the selected prefab
        Nucleon instance = Instantiate<Nucleon>(prefab);
        //and setting the local position of the nucleon to a random spherepoint times spawndistance
        instance.transform.localPosition = Random.onUnitSphere * spawnDistance;
    }
}
