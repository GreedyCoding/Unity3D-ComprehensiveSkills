using UnityEngine;

public class GameLevel : MonoBehaviour
{
    //Settable spawnzone for every level
    [SerializeField] SpawnZone spawnZone;

	void Start ()
    {
        //Setting the spawnzone the objectspawner uses to the spawnzone we have on this level
        ObjectSpawner.Instance.SpawnZoneOfLevel = spawnZone;
	}
}
