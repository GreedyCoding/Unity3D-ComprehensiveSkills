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
    private static int s_sinFrequenzyMultiplier = 1;
    private static int s_multisinModulationFactor = 2;
    private static int s_multisinMorphingFactor = 1;

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
        MultiSine2DFunction,
        Ripple,
        Cylinder,
        Sphere,
        Torus

    };

    private void Awake() {

        //The size of each step on the scale is 2(number of scales)/resolution
        float step = 2f / resolution;
        //Calculation the scale out of the loop cause it is only required once
        Vector3 scale = Vector3.one * step;

        //Creating a new Transform array for our points
        points = new Transform[resolution * resolution];
        
        //Loop through the point array
        for (int index = 0; index < points.Length; index++) {
            //And instantiate a point on every spot
            Transform point = Instantiate(pointPrefab);
            //Set points scale to the calculated scale
            point.localScale = scale;
            //Set the Graph as parent to the points
            //false makes the object keep its local orientation rather than its global orientation.
            point.SetParent(transform, false);
            //store the created point at the current array spot
            points[index] = point;
        }

    }

    private void Update() {

        float time = Time.time;

        //Setting a delegate for the currently in the dropdown selected function
        GraphFunctionDelegate graphFunction = graphFunctions[(int)functionDropdownSelection] ;

        float step = 2f / resolution;
        //Looping through all the layers
        for (int index = 0, z = 0; z < resolution; z++) {
            //And calulating v value to fit on scale
            float v = (z + pointOffset) * step - scaleOffset;
            //Looping through all the rows 
            for (int x = 0; x < resolution; x++, index++) {
                //Calculating u value to fit scale
                float u = (x + pointOffset) * step - scaleOffset;
                //Set localposition of the point to the vector that will be returned from the selected function
                points[index].localPosition = graphFunction(u, v, time);

            }
        }

        //Setting the static equal to the according slider values in the inspector beacuse the functions have to be static to be
        //used by the delegate type. Only solution for now.
        s_sinFrequenzyMultiplier = sinFrequenzyMultiplier;
        s_multisinModulationFactor = multisinModulationFactor;
        s_multisinMorphingFactor = multisinMorphingFactor;

    }

    static Vector3 SineFunction(float _x, float _z, float _time) {

        Vector3 result;
        result.x = _x;
        //Returns the sinus of x plus time 
        result.y = Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_x + _time));
        result.z = _z;
        return result;

    }

    static Vector3 MultiSineFunction(float _x, float _z, float _time) {

        Vector3 result;
        result.x = _x;
        //Calculates the sinus of x plus time
        float sinOfX = Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_x + _time));
        //Adds another sinus calculation to the the sinOfX
        float doubleSinOfX = sinOfX + Mathf.Sin(s_multisinModulationFactor * pi * (_x + _time * s_multisinMorphingFactor)) / 2f;
        //Devide the value by 1.5 because the amplitude is 1.5 now
        doubleSinOfX *= 2f / 3f;
        //Returns the adjusted value
        result.y = doubleSinOfX;
        result.z = _z;
        return result;

    }

    static Vector3 MultiSine2DFunction(float _x, float _z, float _time) {

        Vector3 result;
        result.x = _x;
        float yValue = 4f * Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_x + _z + _time * 0.5f));
        yValue += Mathf.Sin(pi * (_x * s_multisinModulationFactor + _time));
        yValue += Mathf.Sin(2f * pi * (_z * s_multisinMorphingFactor + 2f * _time)) * 0.5f;
        yValue *= 1f / 5.5f;
        result.y = yValue;
        result.z = _z;
        return result;


    }

    static Vector3 Sine2DFunction(float _x, float _z, float _time) {

        Vector3 result;
        result.x = _x;
        result.y = Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_x * s_multisinModulationFactor + _z * s_multisinMorphingFactor + _time));
        result.z = _z;
        return result;

    }

    static Vector3 Sine2DAlternateFunction(float _x, float _z, float _time) {

        Vector3 result;
        result.x = _x;
        float yValue = Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_x * s_multisinModulationFactor + _time));
        yValue += Mathf.Sin(pi * s_sinFrequenzyMultiplier * (_z * s_multisinMorphingFactor + _time));
        yValue *= 0.5f;
        result.y = yValue;
        result.z = _z;
        return result;

    }

    static Vector3 Ripple (float _x, float _z, float _time) {

        Vector3 result;
        result.x = _x;
        //Getting the distance of the object to the originpoint
        float distance = Mathf.Sqrt(_x * _x + _z * _z);
        //Taking sin of distance to create the ripple and substracting time because the ripple should move outwards
        float yValue = Mathf.Sin(pi * s_sinFrequenzyMultiplier * (distance - _time/2));
        //Reducing the amplitude of the ripple because it was far to extreme and including the distance
        //into the function so the ripples get less amplitude the farhter they are away from the origin
        yValue /= 1f + 10f * distance;
        result.y = yValue;
        result.z = _z;
        return result;

    }

    static Vector3 Cylinder(float _u, float _v, float _time) {

        Vector3 result;
        //float r = 0.8f + Mathf.Sin(pi * (6f * _u + 2f * _v + _time)) * 0.2f;
        float r = 1f;
        result.x = r * Mathf.Sin(pi * _u);
        result.y = _v;
        result.z = r * Mathf.Cos(pi * _u);
        return result;

    }

    static Vector3 Sphere(float _u, float _v, float _time) {

        Vector3 result;
        //Adding another wave as r to reduce its radius at its top and bottom to zero
        float r = Mathf.Cos(pi * _v * 0.5f);
        result.x = r * Mathf.Sin(pi * _u);
        result.y = Mathf.Sin(pi * 0.5f * _v);
        result.z = r * Mathf.Cos(pi * _u);
        return result;

        #region Pulsating Sphere

        //Vector3 result;
        //float r = 0.8f + Mathf.Sin(pi * (6f * _u + _time)) * 0.1f;
        //r += Mathf.Sin(pi * (4f * _v + _time)) * 0.1f;
        //float s = r * Mathf.Cos(pi * 0.5f * _v);
        //result.x = s * Mathf.Sin(pi * _u);
        //result.y = r * Mathf.Sin(pi * 0.5f * _v);
        //result.z = s * Mathf.Cos(pi * _u);
        //return result;

        #endregion

    }

    static Vector3 Torus(float _u, float _v, float _time) {

        Vector3 result;
        //float r1 = 0.65f + Mathf.Sin(pi * (6f * _u + _time)) * 0.1f;
        //float r2 = 0.2f + Mathf.Sin(pi * (4f * _v + _time)) * 0.05f;
        float r1 = 1f;
        float r2 = 0.5f;
        float s = r2 * Mathf.Cos(pi * _v) + r1;
        result.x = s * Mathf.Sin(pi * _u);
        result.y = r2 * Mathf.Sin(pi * _v);
        result.z = s * Mathf.Cos(pi * _u);
        return result;

    }

}