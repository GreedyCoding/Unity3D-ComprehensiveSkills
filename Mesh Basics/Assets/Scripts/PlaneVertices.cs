using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlaneVertices : MonoBehaviour
{
    [SerializeField] int xSize;
    [SerializeField] int ySize;

    Vector3[] vertices;

    Mesh mesh;

    void Awake()
    {
        Generate();
    }

    void OnDrawGizmos()
    {
        //If there are no vertices we dont have to draw anything so we return
        if(vertices == null)
        {
            return;
        }

        //Otherwise set Gizmo Color to Blact
        Gizmos.color = Color.black;

        for (int i = 0; i < vertices.Length; i++)
        {
            //And draw a sphere at all positions of the vertices
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    void Generate()
    {
        //Getting the Meshfilter and setting its mesh to a new mesh
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        //Setting Name of the Mesh
        mesh.name = "Procedural Face";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
            }
        }

        //And an array with UV points for the UV map
        Vector2[] uv = new Vector2[vertices.Length];
        //Tangents for normal mapping
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
        //Loop through the columns
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            //And loop through all the points in this column
            for (int x = 0; x <= xSize; x++, i++)
            {
                //And create a point with the x and y at this spot in the array
                vertices[i] = new Vector3(x, y);
                //And set a point for the UV map at the
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                //Setting tangent
                tangents[i] = tangent;
            }
        }
        //Setting the vertices, tangents and the uv points of the mesh
        mesh.vertices = vertices;
        mesh.tangents = tangents;
        mesh.uv = uv;

        //Int array with all the points the meshrenderer uses to draw triangles
        int[] triangles = new int[xSize * ySize * 6]; //Multiplied by 6 because we need 6 points to draw 2 triangles (1 quad)
        //Setting the points to draw for every triangle
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        //Setting the triangles of the mesh to the calcualted points
        mesh.triangles = triangles;

        //And recalculate the normals of the mesh so lighting is rendered correctly
        mesh.RecalculateNormals();

    }
}
