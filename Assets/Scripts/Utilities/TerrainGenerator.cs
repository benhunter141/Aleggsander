#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
#endif

public class TerrainGenerator
{
#if UNITY_EDITOR

    static Mesh CreateTerrainMesh()
    {
        var verts = Vertices(ElevationData(), 2, 0.3f);
        var tris = Triangles(ElevationData());
        //var normals = Normals(verts.ToArray(), tris.ToArray());

        Mesh mesh = new Mesh();
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        return mesh;
    }

    [MenuItem("GameObject/3D Object/Ben's Terrain", false, 0)]
    static GameObject CreateTerrain()
    {
        var obj = new GameObject("Terrain");
        Mesh mesh = CreateTerrainMesh();
        var filter = obj.AddComponent<MeshFilter>();
        var renderer = obj.AddComponent<MeshRenderer>();
        //var collider = obj.AddComponent<MeshCollider>();

        filter.sharedMesh = mesh;
        //collider.sharedMesh = mesh;
        renderer.sharedMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");

        return obj;
    }

    static List<List<int>> ElevationData()
    {
        var a = new List<int> { 0, 2, 2, 2, 3, 3, 2, 2, 1, 0, 0 };
        var b = new List<int> { 0, 2, 2, 2, 2, 3, 2, 3, 2, 0, 0 };
        var c = new List<int> { 0, 0, 0, 1, 1, 1, 1, 2, 2, 1, 0 };
        var d = new List<int> { 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0 };
        var e = new List<int> { 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0 };
        var list = new List<List<int>> { a, b, c, d, e };
        return list;
    }

    static List<Vector3> Vertices(List<List<int>> elevationData, float tileSize, float heightSize)
    {
        int length = elevationData.Count;
        int width = elevationData[0].Count;
        var verts = new List<Vector3>();

        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                float x = i * tileSize;
                float z = j * tileSize;
                float y = elevationData[i][j] * heightSize;
                verts.Add(new Vector3(x, y, z));
            }
        }
        return verts;
    }

    static List<int> Triangles(List<List<int>> elevationData)
    {
        var list = new List<int>();
        for (int i = 0; i < elevationData.Count - 1; i++)
        {
            for (int j = 0; j < elevationData[0].Count - 1; j++)
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
#endif
}
