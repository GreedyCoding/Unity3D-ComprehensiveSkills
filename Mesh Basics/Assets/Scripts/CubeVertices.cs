using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeVertices : MonoBehaviour
{

    [SerializeField] int xSize;
    [SerializeField] int ySize;
    [SerializeField] int zSize;

    private Mesh mesh;

    private Vector3[] vertices;

    private void Awake()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";
        StartCoroutine(CreateVertices());
        StartCoroutine(CreateTriangles());
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    IEnumerator CreateVertices()
    {
		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;
		int faceVertices = (
			(xSize - 1) * (ySize - 1) +
			(xSize - 1) * (zSize - 1) +
			(ySize - 1) * (zSize - 1)) * 2;
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

        int v = 0;

        //Looping through the layers of the cube
        for (int y = 0; y <= ySize; y++)
        {
            //Creating a loop around the cube at the current y value(layer) 
            for (int x = 0; x <= xSize; x++)
            {
                vertices[v++] = new Vector3(x, y, 0);
                yield return new WaitForSeconds(0.05f);

            }
            for (int z = 1; z <= zSize; z++)
            {
                vertices[v++] = new Vector3(xSize, y, z);
                yield return new WaitForSeconds(0.05f);

            }
            for (int x = xSize - 1; x >= 0; x--)
            {
                vertices[v++] = new Vector3(x, y, zSize);
                yield return new WaitForSeconds(0.05f);

            }
            for (int z = zSize - 1; z > 0; z--)
            {
                vertices[v++] = new Vector3(0, y, z);
                yield return new WaitForSeconds(0.05f);

            }
        }

        //Filling the holes at the top and bottom of the cube
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                vertices[v++] = new Vector3(x, ySize, z);
                yield return new WaitForSeconds(0.05f);

            }
        }
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                vertices[v++] = new Vector3(x, 0, z);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    IEnumerator CreateTriangles()
    {
        //Calculating how many quads we have
        int quads = ((xSize * ySize) + (xSize * zSize) + (ySize * zSize)) * 2;
        //Creating int array with quads times 6 because there are 6 vertices needed for 1 quad
        int[] triangles = new int[quads * 6];
        //Calculating the size of the ring
        int ring = (xSize + zSize) * 2;

        int t = 0, v = 0;

        for (int y = 0; y < ySize; y++, v++)
        {
            for (int q = 0; q < ring - 1; q++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            }
            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
            yield return new WaitForSeconds(0.05f);
        }

        mesh.triangles = triangles;
    }

    static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }
}
