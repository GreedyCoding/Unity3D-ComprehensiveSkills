using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    private float scaleOffset = 1f;
    private float pointOffset = 0.5f;

    [Range(10,100)]
    public int resolution = 50;
    public Transform pointPrefab;

    Transform[] points;

    private void Awake() {

        //The size of each step on the scale is 2/resolution
        float step = 2f / resolution;
        //Calculation the scale out of the loop cause it is only required once
        Vector3 scale = Vector3.one * step;
        //Initializing an position vector and setting y and z value to 0 initially
        Vector3 position;    position.y = 0f;    position.z = 0f;

        //Creating a new Transform array for our points
        points = new Transform[resolution];

        //Looping through the points array
        for (int index = 0; index < points.Length; index++) {

            //Instantiating a point from the pointPrefab
            Transform point = Instantiate(pointPrefab);

            //Setting x value along the scale
            //Adding pointoffset to index to set it in the middle of the scale
            //Multiplying by step to set it at the right position
            //And substracting scaleOffset (in this case 1 so we have a scale from -1 to 1)
            position.x = (index + pointOffset) * step - scaleOffset;
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

    private void Update() {

        //Looping through all the points and maniplutaing their y positon based on some functions
        for (int index = 0; index < points.Length; index++) {

            Transform point = points[index];
            Vector3 position = point.localPosition;
            position.y = SineFunction(position.x, Time.time, 2f);
            point.localPosition = position;

        }

    }

    float SineFunction(float x, float time, float multipliyer = 1f) {

        float pi = Mathf.PI;

        return Mathf.Sin(pi * multipliyer * (x + time));

    }

}
