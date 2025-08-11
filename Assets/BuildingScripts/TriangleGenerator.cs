using System.Collections.Generic;
using UnityEngine;

namespace BuildingScripts
{
    public class TriangleGenerator : MonoBehaviour
    {
        private void Start()
        {
            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
            if (meshFilter == null) meshFilter = gameObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            if (meshRenderer == null) meshRenderer = gameObject.AddComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.name = "Triangle";

            List<Vector3> verts = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            Vector3 A = new Vector3(-0.5f, -0.5f, -0.5f);
            Vector3 B = new Vector3(0.5f, -0.5f, -0.5f);
            Vector3 C = new Vector3(-0.5f, 0.5f, -0.5f);
            Vector3 D = new Vector3(-0.5f, -0.5f, 0.5f);
            Vector3 E = new Vector3(0.5f, -0.5f, 0.5f);
            Vector3 F = new Vector3(-0.5f, 0.5f, 0.5f);

            int off = verts.Count;
            verts.Add(A); verts.Add(B); verts.Add(C);
            uvs.Add(Vector2.zero); uvs.Add(Vector2.right); uvs.Add(Vector2.up);
            tris.Add(off + 0); tris.Add(off + 2); tris.Add(off + 1);

            off = verts.Count;
            verts.Add(D); verts.Add(E); verts.Add(F);
            uvs.Add(Vector2.zero); uvs.Add(Vector2.right); uvs.Add(Vector2.up);
            tris.Add(off + 0); tris.Add(off + 1); tris.Add(off + 2);

            off = verts.Count;
            verts.Add(A); verts.Add(B); verts.Add(D); verts.Add(E);
            uvs.Add(new Vector2(0, 0)); uvs.Add(new Vector2(1, 0)); uvs.Add(new Vector2(0, 1)); uvs.Add(new Vector2(1, 1));
            tris.Add(off + 0); tris.Add(off + 1); tris.Add(off + 2);
            tris.Add(off + 1); tris.Add(off + 3); tris.Add(off + 2);

            off = verts.Count;
            verts.Add(C); verts.Add(A); verts.Add(D); verts.Add(F);
            uvs.Add(Vector2.zero); uvs.Add(Vector2.right); uvs.Add(Vector2.up); uvs.Add(Vector2.one);
            tris.Add(off + 0); tris.Add(off + 2); tris.Add(off + 3);
            tris.Add(off + 0); tris.Add(off + 1); tris.Add(off + 2);

            off = verts.Count;
            verts.Add(B); verts.Add(C); verts.Add(F); verts.Add(E);
            uvs.Add(Vector2.zero); uvs.Add(Vector2.right); uvs.Add(Vector2.up); uvs.Add(Vector2.one);
            tris.Add(off + 0); tris.Add(off + 2); tris.Add(off + 3);
            tris.Add(off + 0); tris.Add(off + 1); tris.Add(off + 2);

            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.SetUVs(0, uvs);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            meshFilter.mesh = mesh;

            meshRenderer.material = new Material(Shader.Find("Standard"));
            meshRenderer.material.color = Color.white;
        }
    }
}
