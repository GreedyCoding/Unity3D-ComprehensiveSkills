using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    //Using scaleOffset of 1 because the graph should range from -1 to 1
    private float scaleOffset = 1f;
    //Because the cubes we use as points are 1f wide we offset them by 0.5f so they are on the right spots
    private float pointOffset = 0.5f;

    static private float pi = Mathf.PI;

    //Sets a dropdown in the inspector for the public Enumerator of the function types
    [Header("Select desired function to be shown")]
    public GraphFunctionEnumerator functionDropdownSelection;
    
    //Values for adjusting the graph function
    [Header("Resoltution of the graph")]
    [Range(10,100)] public int resolution = 50;
    [Header("Used for frequenzy of the main sinus")]
    [Range(0,5)] public int sinFrequenzyMultiplier = 1;
    [Header("Used for Multisinus and 2D sinus only")]
    [Range(0,5)] public int multisinModulationFactor = 2;
    [Range(0,5)] public int multisinMorphingFactor = 1;

    //Static values that get updated in Update() to be used in the static functions
    public static int s_sinFrequenzyMultiplier = 1;
    public static int s_multisinModulationFactor = 2;
    public static int s_multisinMorphingFactor = 1;

    //Refernce to the pointprefab that gets instantiated
    [Header("Prefab used for the points")]
    public Transform pointPrefab;

    //Transform array for all the points
    Transform[] points;

    //Array of graphfunctions instatiated with all available functions passed in
    GraphFunctionDelegate[] graphFunctions = {

        SineFunction,
        Sine2DFunction,
        Sine2DAlternateFunction,
        MultiSineFunction,
        MultiSine2DFunction


    };

    private void Awake() {

        //The size of each step on the scale is 2(number of scales)/resolution
        float step = 2f / resolution;
        //Calculation the scale out of the loop cause it is only required once
        Vector3 scale = Vector3.one * step;
        //Initializing an position vector and setting y and z value to 0 initially
        Vector3 position;    position.y = 0f;    position.z = 0f;

        //Creating a new Transform array for our points
        points = new Transform[resolution * resolution];

        //Looping through the layers(Z-Coordinates) first and then looping through all
        //X-Coordinates and instantiating points there. Y-Coordinates get manipluated in update
        for (int index = 0, zCoordinate = 0; zCoordinate < resolution; zCoordinate++) {

            //We can set the z position here because we only change z values inside this loop
            position.z = (zCoordinate + pointOffset) * step - scaleOffset;

            for (int xCoordinate = 0; xCoordinate < resolution; xCoordinate++, index++) {

                //Instantiating a point from the pointPrefab
                Transform point = Instantiate(pointPrefab);

                //Setting x value along the scale
                //Adding pointoffset to index to set the point on the right spot on the scale
                //Multiplying by step to set it at the right position
                //And substracting scaleOffset (in this case 1 so we have a scale from -1 to 1)
                position.x = (xCoordinate + pointOffset) * step - scaleOffset;
                //Set position of the point to the calcualted position
                point.localPosition = position;
                //Set scale of the point to the calcualted scale
                point.localScale = scale;
                //Setting the Graph object as parent and 
                //Adding false as second argument so unity will not attemt to keep the object at its original world position, rotation, and scale
                point.SetParent(transform, false);

                //Adding the point to the according spot in the array
                points[index] = point;

            }

        }

    }

    private void Update() {

        float time = Time.time;

        //Setting a delegate for the currently in the dropdown selected function
        GraphFunctionDelegate graphFunction = graphFunctions[(int)functionDropdownSelection] ;

        //Looping through all the points
        for (int index = 0; index < points.Length; index++) {

            //Getting the current point and storing it in a transform variable
            Transform point = points[index];
            //Storing the points localposition in a position vector
            Vector3 position = point.localPosition;
            //Manipulating the y postion based on the x positon and the current time
            position.y = graphFunction(position.x, position.z, time);
            //Setting the local postion to the manipulated position vector
            point.localPosition = position;

        }

        //Setting the static equal to the according slider values in the inspector beacuse the functions have to be static to be
        //used by the delegate type. Only solution for now.
        s_sinFrequenzyMultiplier = sinFrequenzyMultiplier;
        s_multisinModulationFactor = multisinModulationFactor;
        s_multisinMorphingFactor = multisinMorphingFactor;

    }

    static float SineFunction(float _x, float _z, float _time) {

        //Returns the sinus of x plus time 
        return Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_x + _time));

    }

    static float MultiSineFunction(float _x, float _z, float _time) {

        //Calculates the sinus of x plus time
        float sinOfX = Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_x + _time));
        //Adds another sinus calculation to the the sinOfX
        float doubleSinOfX = sinOfX + Mathf.Sin(s_multisinModulationFactor * pi * (_x + _time * s_multisinMorphingFactor)) / 2f;
        //Devide the value by 1.5 because the amplitude is 1.5 now
        doubleSinOfX *= 2f / 3f;
        //Returns the adjusted value
        return doubleSinOfX;

    }

    static float MultiSine2DFunction(float _x, float _z, float _time) {

        float result = 4f * Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_x + _z + _time * 0.5f));
        result += Mathf.Sin(pi * (_x * s_multisinModulationFactor + _time));
        result += Mathf.Sin(2f * pi * (_z * s_multisinMorphingFactor + 2f * _time)) * 0.5f;
        result *= 1f / 5.5f;
        return result;

    }

    static float Sine2DFunction(float _x, float _z, float _time) {

        return Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_x * s_multisinModulationFactor + _z * s_multisinMorphingFactor + _time));

    }

    static float Sine2DAlternateFunction(float _x, float _z, float _time) {

        float result = Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_x * s_multisinModulationFactor + _time));
        result += Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_z * s_multisinMorphingFactor + _time));
        result *= 0.5f;
        return result;

    }

}
