using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] SpawnZone spawnZone;

	void Start () {
        ObjectSpawner.Instance.SpawnZoneOfLevel = spawnZone;
	}
}
