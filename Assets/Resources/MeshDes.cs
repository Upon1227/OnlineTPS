using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MeshDes : MonoBehaviour
{

    class MeshData
    {
        internal string name;
        internal Color c;
        internal Vector3 offset;
    };

    // メッシュを2個作成する
    readonly MeshData[] meshDataSet =
    {
        new MeshData { name = "red",    c = new Color(1, 0, 0), offset = new Vector3(0, 0, 0) },
        new MeshData { name = "white",  c = new Color(1, 1, 1), offset = new Vector3(0, 0, 0) },
    };

    static readonly int NUM_POINTS = 5000;      // 点の個数

    /// <summary>
    /// メッシュを表示するだけのGameObjectを作成する
    /// </summary>
    /// <returns></returns>
    private GameObject CreateGameObjectWithMeshRenderer()
    {
        var o = new GameObject();
        o.transform.parent = GetComponent<Transform>();
        o.AddComponent<MeshFilter>();
        o.AddComponent<MeshRenderer>();

        // 組み込みのSprite-Defaultマテリアルを取得
#if UNITY_EDITOR
        var spriteMat = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
#else
        var spriteMat = Resources.GetBuiltinResource<Material>("Sprites-Default.mat");
#endif
        o.GetComponent<MeshRenderer>().material = spriteMat;

        return o;
    }

    /// <summary>
    /// 立方体の空間内のランダムな位置に点を生成したメッシュを生成する
    /// </summary>
    /// <param name="numPoints">点の数</param>
    /// <param name="c">色</param>
    /// <param name="name">メッシュ名</param>
    /// <param name="offset">中心位置</param>
    /// <returns>生成したメッシュ</returns>
    Mesh CreateSimpleSurfacePointMesh(int numPoints, Color c, string name, Vector3 offset)
    {
        Vector3[] points = new Vector3[numPoints];
        int[] indecies = new int[numPoints];
        Color[] colors = new Color[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            points[i] = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) + offset;  // 頂点座標
            indecies[i] = i;                                                                                        // 配列番号をそのままインデックス番号に流用
            colors[i] = c;
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = points;
        mesh.SetIndices(indecies, MeshTopology.Points, 0);  // 1頂点が1インデックスの関係
        mesh.colors = colors;
        mesh.name = name;

        return mesh;
    }

    void Start()
    {
        var meshes = new List<Mesh>();

        // メッシュを作成
        foreach (var item in meshDataSet)
        {
            Mesh meshSurface = CreateSimpleSurfacePointMesh(NUM_POINTS, item.c, item.name, item.offset);
            meshes.Add(meshSurface);
            var o = CreateGameObjectWithMeshRenderer();
            o.name = item.name;
            o.GetComponent<MeshFilter>().mesh = meshSurface;
        }

        // 合成したメッシュを作成
        CombineInstance[] combineInstanceAry = new CombineInstance[meshes.Count];
        for (int i = 0; i < meshes.Count; i++)
        {
            // エラー：Failed getting triangles. Submesh topology is lines or points.
            combineInstanceAry[i].mesh = meshes[i];
        }
        var combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combineInstanceAry);
    }

}
