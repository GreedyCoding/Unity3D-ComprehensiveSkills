using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CubeVertices : MonoBehaviour
{

    [SerializeField] int xSize;
    [SerializeField] int ySize;
    [SerializeField] int zSize;
    [SerializeField] int roundness;

    private Mesh mesh;

    private Vector3[] vertices;
    private Vector3[] normals;

    private void Awake()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";
        CreateVertices();
        CreateTriangles();

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
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }

    void CreateVertices()
    {
		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;
		int faceVertices = (
			(xSize - 1) * (ySize - 1) +
			(xSize - 1) * (zSize - 1) +
			(ySize - 1) * (zSize - 1)) * 2;
		vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        normals = new Vector3[vertices.Length];

        int v = 0;

        //Looping through the layers of the cube
        for (int y = 0; y <= ySize; y++)
        {
            //Creating a loop around the cube at the current y value(layer) 
            for (int x = 0; x <= xSize; x++)
            {
                SetVertex(v++, x, y, 0);
            }
            for (int z = 1; z <= zSize; z++)
            {
                SetVertex(v++, xSize, y, z);
            }
            for (int x = xSize - 1; x >= 0; x--)
            {
                SetVertex(v++, x, y, zSize);
            }
            for (int z = zSize - 1; z > 0; z--)
            {
                SetVertex(v++, 0, y, z);
            }
        }

        //Filling the holes at the top and bottom of the cube
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                SetVertex(v++, x, ySize, z);
            }
        }
        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                SetVertex(v++, x, 0, z);
            }
        }
        mesh.vertices = vertices;
        mesh.normals = normals;
    }

    void CreateTriangles()
    {
        //Calculating how many quads we have
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        //Creating int array with quads times 6 because there are 6 vertices needed for 1 quad
        int[] triangles = new int[quads * 6];
        //Calculating the size of the ring
        int ring = (xSize + zSize) * 2;

        int triangleIndex = 0, vertexIndex = 0;

        //For every layer(ySize)
        for (int y = 0; y < ySize; y++, vertexIndex++)
        {
            for (int q = 0; q < ring - 1; q++, vertexIndex++)
            {
                //Set quads around the current ring
                triangleIndex = SetQuad(triangles, triangleIndex, vertexIndex, vertexIndex + 1, vertexIndex + ring, vertexIndex + ring + 1);
            }
            //an set last quad out of the loop beacuse it loops back to the beginning
            triangleIndex = SetQuad(triangles, triangleIndex, vertexIndex, vertexIndex - ring + 1, vertexIndex + ring, vertexIndex + 1);
        }
        triangleIndex = CreateTopFace(triangles, triangleIndex, ring);
        triangleIndex = CreateBottomFace(triangles, triangleIndex, ring);
        mesh.triangles = triangles;
    }

    int CreateTopFace(int[] triangles, int triangleIndex, int ring)
    {
        int v = ring * ySize;
        for (int x = 0; x < xSize - 1; x++, v++)
        {
            triangleIndex = SetQuad(triangles, triangleIndex, v, v + 1, v + ring - 1, v + ring);
        }
        triangleIndex = SetQuad(triangles, triangleIndex, v, v + 1, v + ring - 1, v + 2);

        int vMin = ring * (ySize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;

        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
        {
            triangleIndex = SetQuad(triangles, triangleIndex, vMin, vMid, vMin - 1, vMid + xSize - 1);
            for (int x = 1; x < xSize - 1; x++, vMid++)
            {
                triangleIndex = SetQuad(
                    triangles, triangleIndex,
                    vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
            }
            triangleIndex = SetQuad(triangles, triangleIndex, vMid, vMax, vMid + xSize - 1, vMax + 1);
        }

        int vTop = vMin - 2;
        triangleIndex = SetQuad(triangles, triangleIndex, vMin, vMid, vMin - 1, vMin - 2);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++)
        {
            triangleIndex = SetQuad(triangles, triangleIndex, vMid, vMid + 1, vTop, vTop - 1);
        }
        triangleIndex = SetQuad(triangles, triangleIndex, vMid, vTop - 2, vTop, vTop - 1);

        return triangleIndex;
    }

    int CreateBottomFace(int[] triangles, int t, int ring)
    {
        int v = 1;
        int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < xSize - 1; x++, v++, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }
        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

        int vMin = ring - 2;
        vMid -= xSize - 2;
        int vMax = v + 2;

        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
            for (int x = 1; x < xSize - 1; x++, vMid++)
            {
                t = SetQuad(
                    triangles, t,
                    vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
            }
            t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
        }

        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }
        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

        return t;
    }

    static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    void SetVertex(int i, int x, int y, int z)
    {
        Vector3 inner = vertices[i] = new Vector3(x, y, z);

        if (x < roundness)
        {
            inner.x = roundness;
        }
        else if (x > xSize - roundness)
        {
            inner.x = xSize - roundness;
        }
        if (y < roundness)
        {
            inner.y = roundness;
        }
        else if (y > ySize - roundness)
        {
            inner.y = ySize - roundness;
        }
        if (z < roundness)
        {
            inner.z = roundness;
        }
        else if (z > zSize - roundness)
        {
            inner.z = zSize - roundness;
        }

        normals[i] = (vertices[i] - inner).normalized;
        vertices[i] = inner + normals[i] * roundness;
    }
}
