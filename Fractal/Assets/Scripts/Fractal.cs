using System.Collections;
using UnityEngine;

public class Fractal : MonoBehaviour 
{
    //Public array of meshes we fill in the Unity inspector
    public Mesh[] meshes;
    //Material Reference field for the inspector
    public Material material;

    //Public references to adjust the Fractal in the inspector
    public int maxDepth = 5;
    public float childScale = 0.5f;
    public float spawnProbabilty = 0.7f;
    public float maxRotationSpeed;
    public float maxTwist;

    //Int to store the current depth of the object
    private int depth = 0;
    //Float to store the rotationspeed of the object
    private float rotationSpeed;

    //Two Dimensional Array for the materials
    private Material[,] materials;
    //Array to store all the directions the childs will get as vectors
    private static Vector3[] childDirections =
    {
        Vector3.up,         
        Vector3.right,      
        Vector3.left,       
        Vector3.forward,    
        Vector3.back        
    };
    //Array to store all the orientations the childs will get as Euler angles
    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };

    private void Start()
    {
        //Setting a random rotationspeed between minusMaxRot and maxRotSpeed
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        //Setting a random rotation between minusMaxTwist and maxTwist
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
        //If there are no matrials initialized yet
        if (materials == null)
        {
            //initialize the materials so unity does not create a new material for every cube that gets created
            InitializeMaterials();
        }
        //Choose and addd a random mesh selected from the mesh array we filled in the inspector
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        //Set the material to the corrisponding material we initialized before
        gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0,2)];

        //If this object has not reached the maxDepth 
        if (depth < maxDepth)
        {
            //Create another children
            StartCoroutine(CreateChildren());
        }
    }

    private void Update()
    {
        //Rotating this obejct based on rotationspeed around the y
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private IEnumerator CreateChildren()
    {
        //Loop through all the possible directions
        for (int i = 0; i < childDirections.Length; i++)
        {
            //Check if random value(0-1) is smaller then the spawnprobabilty
            if (Random.value < spawnProbabilty)
            {
                //if it was wait for a random ammout of seconds before
                yield return new WaitForSeconds(Random.Range(0.1f, 0.7f));
                //initializing a new Fractal child passing this as parent and i as index we use for direction and orientation
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);

            }
        }
    }

    private void Initialize(Fractal parent, int childIndex)
    {
        //Passing these values from the parent to the child
        meshes = parent.meshes;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        childScale = parent.childScale;
        spawnProbabilty = parent.spawnProbabilty;
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;
        //Adding 1 to then parents depth to get the current depth this object is sitting on
        depth = parent.depth + 1;
        //Passing over the parents transform
        transform.parent = parent.transform;
        //Setting the localscale of the child to be half of the partent
        transform.localScale = Vector3.one * childScale;
        //Setting position to the value corrisponding to the passed childindex and offsetting the object by 0.5, 0.5 and the size of the child
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        //Setting rotation to the value corresponding to the passed childindex
        transform.localRotation = childOrientations[childIndex];
    }

    private void InitializeMaterials()
    {
        //Creating a new Matrial array with as many spots as we have maxDepth with a second dimension to store another color
        materials = new Material[maxDepth + 1, 2];
        //Loop throught the first dimension of the array
        for (int i = 0; i <= maxDepth; i++)
        {
            //Since we are assigning a seperate color for the maxDepth we define the transition as maxDepth -1
            float transition = i / (maxDepth - 1f);
            //Squaring the transition value so we get a nicer transition
            transition *= transition;
            //Setting the material in the first dimension to lerp from white to yellow
            materials[i, 0] = new Material(material);
            materials[i, 0].color = Color.Lerp(Color.white, Color.yellow, transition);
            //Setting the material in the second dimension to lerp from white to cyan
            materials[i, 1] = new Material(material);
            materials[i, 1].color = Color.Lerp(Color.white, Color.cyan, transition);
        }
        //setting maxDepth color value in the first dimension to magenta
        materials[maxDepth, 0].color = Color.magenta;
        //setting maxDepth color vlaue in the second dimenstion to red
        materials[maxDepth, 1].color = Color.red;
    }

}
