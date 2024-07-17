using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTerrainGenerator : MonoBehaviour
{
    //vertices and triangles
    //mesh
    //set renderer and filter

    public Material material;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

    private void Awake()
    {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter = gameObject.AddComponent<MeshFilter>();

        //tileSize and height
        var verts = Vertices(ElevationData(), 3, 0.2f);
        var tris = Triangles(ElevationData());
        //var normals = Normals(verts.ToArray(), tris.ToArray());

        meshRenderer.material = material;
        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        meshFilter.mesh = mesh;
        //meshFilter.sharedMesh = mesh;


        //meshFilter.mesh.normals = normals;
        //meshFilter.sharedMesh = mesh;
        //meshRenderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
    }

    IEnumerator Start()
    { 
        Debug.Log("Recalculating!");
        yield return new WaitForSeconds(2f);
        Debug.Log("Recalculated!");
        var mesh = meshFilter.mesh;
        
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    Vector3[] Normals(Vector3[] verts, int[] tris)
    {
        Vector3[] normals = new Vector3[verts.Length];
        for (int i = 0; i < tris.Length; i += 3)
        {
            int tri0 = tris[i];
            int tri1 = tris[i + 1];
            int tri2 = tris[i + 2];
            Vector3 vert0 = verts[tri0];
            Vector3 vert1 = verts[tri1];
            Vector3 vert2 = verts[tri2];
            // Vector3 normal = Vector3.Cross(vert1 - vert0, vert2 - vert0);
            Vector3 normal = new Vector3()
            {
                x = vert0.y * vert1.z - vert0.y * vert2.z - vert1.y * vert0.z + vert1.y * vert2.z + vert2.y * vert0.z - vert2.y * vert1.z,
                y = -vert0.x * vert1.z + vert0.x * vert2.z + vert1.x * vert0.z - vert1.x * vert2.z - vert2.x * vert0.z + vert2.x * vert1.z,
                z = vert0.x * vert1.y - vert0.x * vert2.y - vert1.x * vert0.y + vert1.x * vert2.y + vert2.x * vert0.y - vert2.x * vert1.y
            };
            normals[tri0] += normal;
            normals[tri1] += normal;
            normals[tri2] += normal;
        }

        for (int i = 0; i < normals.Length; i++)
        {
            // normals [i] = Vector3.Normalize (normals [i]);
            Vector3 norm = normals[i];
            float invlength = 1.0f / (float)System.Math.Sqrt(norm.x * norm.x + norm.y * norm.y + norm.z * norm.z);
            normals[i].x = norm.x * invlength;
            normals[i].y = norm.y * invlength;
            normals[i].z = norm.z * invlength;
        }
        return normals;
    }

    List<List<int>> ElevationData()
    {
        var a = new List<int> { 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0 };
        var b = new List<int> { 0, 0, 0, 0, 0, 0, 2, 3, 2, 0, 0 };
        var c = new List<int> { 0, 0, 0, 0, 1, 0, 1, 2, 2, 1, 0 };
        var d = new List<int> { 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0 };
        var e = new List<int> { 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0 };
        var list = new List<List<int>> { a, b, c, d, e };
        return list;
    }

    List<Vector3> Vertices(List<List<int>> elevationData, float tileSize, float heightSize)
    {
        int length = elevationData.Count;
        int width = elevationData[0].Count;
        var verts = new List<Vector3>();

        for(int i = 0; i < length; i++)
        {
            for(int j = 0; j < width; j++)
            {
                float x = i * tileSize;
                float z = j * tileSize;
                float y = elevationData[i][j] * heightSize;
                verts.Add(new Vector3(x, y, z));
            }
        }
        return verts;
    }

    List<int> Triangles(List<List<int>> elevationData)
    {
        var list = new List<int>();
        for(int i = 0; i < elevationData.Count - 1; i++)
        {
            for(int j = 0; j < elevationData[0].Count - 1; j++)
            {
                //0, 1, 5
                var first = elevationData[0].Count * i + j;
                var second = first + 1;
                var third = first + elevationData[0].Count;
                //5, 1, 6
                var fourth = first + elevationData[0].Count;
                var fifth = first + 1;
                var sixth = first + 1 + elevationData[0].Count;
                list.AddRange(new List<int> { first, second, third, fourth, fifth, sixth });
            }
        }
        return list;
    }
}
