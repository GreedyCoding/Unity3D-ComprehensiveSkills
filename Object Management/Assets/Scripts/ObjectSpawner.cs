using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : PersistableObject
{
    const int buildVersion = 1;

    [SerializeField] private ShapeFactory shapeFactory;
    [SerializeField] private PersistantStorage storage;

    [SerializeField] private float spawnRadius = 5f;

    [SerializeField] private KeyCode createKey = KeyCode.C;
    [SerializeField] private KeyCode destroyKey = KeyCode.D;
    [SerializeField] private KeyCode newGameKey = KeyCode.N;
    [SerializeField] private KeyCode saveKey = KeyCode.S;
    [SerializeField] private KeyCode loadKey = KeyCode.L;

    private float creationProgress;
    private float destrucionProgress;

    private List<Shape> shapes;

    public float CreationSpeed { get; set; }
    public float DestructionSpeed { get; set; }

    private void Awake()
    {
        Application.targetFrameRate = 300;
        //Initializing the list of peristable objects
        shapes = new List<Shape>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateShape();
        }
        else if (Input.GetKeyDown(destroyKey))
        {
            DestroyShape();
        }
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
        }
        else if (Input.GetKeyDown(saveKey))
        {
            //Because the ObjectSpawner derives from PersitableObject we can save the spawner in the storage
            storage.Save(this, buildVersion);
        }
        else if (Input.GetKeyDown(loadKey))
        {
            //Beginning a new game before loading so the scene is empty
            BeginNewGame();
            //Then loading the saved spawner from the storage
            storage.Load(this);
        }

        creationProgress += Time.deltaTime * CreationSpeed;
        while (creationProgress >= 1f)
        {
            creationProgress -= 1f;
            CreateShape();
        }

        destrucionProgress += Time.deltaTime * DestructionSpeed;
        while (destrucionProgress >= 1f)
        {
            destrucionProgress -= 1f;
            DestroyShape();
        }
    }


    void BeginNewGame()
    {
        //Looping through all the objects and destroying their gameobjects
        for (int i = 0; i < shapes.Count; i++)
        {
            Destroy(shapes[i].gameObject);
        }
        //The list still has references to the destroyed objects so its needed to be cleared as well
        shapes.Clear();
    }

    void CreateShape()
    {
        //Instantiate a persistable object
        Shape tempShape = shapeFactory.GetRandomShape();
        //And get the transform of this object to manipulate it
        Transform objectTransform = tempShape.transform;
        //Set the position to a random point in a sphere
        objectTransform.localPosition = Random.insideUnitSphere * spawnRadius;
        //Give it a random rotation
        objectTransform.localRotation = Random.rotation;
        //And scale
        objectTransform.localScale = Vector3.one * Random.Range(0.1f, 1f);
        //Set the color of the Shape
        tempShape.SetColor(Random.ColorHSV(
            hueMin: 0f, hueMax: 1f,
            saturationMin: 0.8f, saturationMax: 1f,
            valueMin: 0.5f, valueMax: 1f,
            alphaMin: 1f, alphaMax: 1f
        ));
        //And add it to our list of persistable objects
        shapes.Add(tempShape);
    }

    void DestroyShape()
    {
        if (shapes.Count > 0)
        {
            //Get a random index
            int index = Random.Range(0, shapes.Count);
            //Destroy the gameobject
            Destroy(shapes[index].gameObject);
            //Getting the last index of the array
            int lastIndex = shapes.Count - 1;
            //To shift the last shape to the spot we just removed from
            shapes[index] = shapes[lastIndex];
            //And then we remove the last shape
            shapes.RemoveAt(lastIndex);
        }
    }

    public override void Save(GameDataWriter writer)
    {
        //Write the current count of objects to the savefile
        writer.Write(shapes.Count);
        for (int i = 0; i < shapes.Count; i++)
        {
            //Write the shapeid of the current shape to the file 
            writer.Write(shapes[i].ShapeId);
            //Write the matrial id
            writer.Write(shapes[i].MaterialId);
            //and call the save method to store positon rotation and scale of the current shape in the savefile
            shapes[i].Save(writer);
        }
    }

    public override void Load(GameDataReader reader)
    {
        //Read the first int and flip it to do the version check
        int saveVersion = reader.VersionControl;
        //If the version is bigger then the saveVersion 
        if (saveVersion > buildVersion)
        {
            Debug.LogError("Unsupported future save version" + saveVersion + ". Currently running" + buildVersion);
            return;
        }
        int count;
        //If the first read value is smaller or equal to 0
        if (saveVersion <= 0)
        {
            //We set the count to the first value because it was the count
            count = -saveVersion;
        }
        else
        {
            //Else we read the count now because we read a version number
            count = reader.ReadInt();
        }

        //For every saved object
        for (int i = 0; i < count; i++)
        {
            //If the saveVersion is bigger then 0 read the shapeId from the memory otherwise just set it to 0
            int shapeId = saveVersion > 0 ? reader.ReadInt() : 0;
            //If the saveVersion is bigger then 0 read the materialId from the memory otherwise just set it to 0
            int materialId = saveVersion > 0 ? reader.ReadInt() : 0;
            //Instantiate a temporary persistable object
            Shape tempShape = shapeFactory.GetShape(shapeId, materialId);
            //Use the load function on the Shape object to retrieve the saved information
            tempShape.Load(reader);
            //And add the loaded object to the list
            shapes.Add(tempShape);
        }
    }
}
