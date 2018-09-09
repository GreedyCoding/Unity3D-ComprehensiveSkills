using System.Collections.Generic;
using UnityEngine;

public class TransformationGrid : MonoBehaviour
{
    [SerializeField] private Transform prefab;

    [SerializeField] private int gridResolution = 10;

    private Transform[] grid;

    private List<Transformation> transformations;

    private void Awake()
    {
        //Initialiaze the grid array with the gridResolution cubed
        grid = new Transform[gridResolution * gridResolution * gridResolution];
        //Looping through the array 3 times for all 3 axis
        for (int i = 0, z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                //Increasing i on the deepest iteration
                for (int x = 0; x < gridResolution; x++, i++)
                {
                    //Creating a GridPoint on every spot for every coordinate
                    grid[i] = CreateGridPoint(x, y, z);
                }
            }
        }
        transformations = new List<Transformation>();
    }

    private void Update()
    {
        GetComponents<Transformation>(transformations);
        for (int i = 0, z = 0; z < gridResolution; z++)
        {
            for (int y = 0; y < gridResolution; y++)
            {
                for (int x = 0; x < gridResolution; x++, i++)
                {
                    grid[i].localPosition = TransformPoint(x, y, z);
                }
            }
        }
    }

    private Transform CreateGridPoint(int x, int y, int z)
    {
        //Instantiate a prefab and store it in point 
        Transform point = Instantiate<Transform>(prefab);
        //Set the localPosition to the corrected coordinates
        point.localPosition = GetCoordinates(x, y, z);
        //Get the meshrenderer to set the color
        point.GetComponent<MeshRenderer>().material.color = new Color(
            //and set the color relative to their position
            (float)x / gridResolution,
            (float)y / gridResolution,
            (float)z / gridResolution
        );
        //Then return the point
        point.SetParent(this.transform);
        return point;
    }

    private Vector3 GetCoordinates(int x, int y, int z)
    {
        //Get the real coordinates
        return new Vector3(
            //Substracting by gridResolution -1 so we offset the points and multiplying by 0.5f to center the points
            x - (gridResolution - 1) * 0.5f,
            y - (gridResolution - 1) * 0.5f,
            z - (gridResolution - 1) * 0.5f
        );
    }

    private Vector3 TransformPoint(int x, int y, int z)
    {
        //Get the current coordinates
        Vector3 coordinates = GetCoordinates(x, y, z);
        for (int i = 0; i < transformations.Count; i++)
        {
            //Apply all transformations to the coordinates
            coordinates = transformations[i].Apply(coordinates);
        }
        //And return them
        return coordinates;
    }
}
