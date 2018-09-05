using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : PersistableObject
{
    [SerializeField] private PersistableObject objectPrefab;
    [SerializeField] private PersistantStorage storage;

    [SerializeField] private float spawnRadius = 5f;

    [SerializeField] private KeyCode createKey = KeyCode.C;
    [SerializeField] private KeyCode deleteKey = KeyCode.D;
    [SerializeField] private KeyCode newGameKey = KeyCode.N;
    [SerializeField] private KeyCode saveKey = KeyCode.S;
    [SerializeField] private KeyCode loadKey = KeyCode.L;

    private List<PersistableObject> objects;

    private void Awake()
    {
        //Initializing the list of peristable objects
        objects = new List<PersistableObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            //Because the ObjectSpawner derives from PersitableObject we can save the spawner in the storage
            storage.Save(this);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            //Beginning a new game before loading so the scene is empty
            BeginNewGame();
            //Then loading the saved spawner from the storage
            storage.Load(this);
        }
    }

    void CreateObject()
    {
        //Instantiate a persistable object
        PersistableObject tempObject = Instantiate(objectPrefab);
        //And get the transform of this object to manipulate it
        Transform objectTransform = tempObject.transform;
        //Set the position to a random point in a sphere
        objectTransform.localPosition = Random.insideUnitSphere * spawnRadius;
        //Give it a random rotation
        objectTransform.localRotation = Random.rotation;
        //And scale
        objectTransform.localScale = Vector3.one * Random.Range(0.1f, 1f);
        //And add it to our list of persistable objects
        objects.Add(tempObject);
    }

    void BeginNewGame()
    {
        //Looping through all the objects and destroying their gameobjects
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i].gameObject);
        }
        //The list still has references to the destroyed objects so its needed to be cleared as well
        objects.Clear();
    }

    public override void Save(GameDataWriter writer)
    {
        //Write the current count of objects to the savefile
        writer.Write(objects.Count);
        for (int i = 0; i < objects.Count; i++)
        {
            //Call the save method on every object to store positon rotation and scale in the savefile
            objects[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        //Get the ammount of saved objects from the savefile
        int count = reader.ReadInt();
        //For every saved object
        for (int i = 0; i < count; i++)
        {
            //Instantiate a new persistable object
            PersistableObject tempObject = Instantiate(objectPrefab);
            //Load the data from the saved object
            tempObject.Load(reader);
            //And add the retrieved object to the list
            objects.Add(tempObject);
        }
    }
}
