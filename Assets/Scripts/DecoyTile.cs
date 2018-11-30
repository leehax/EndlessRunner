using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyTile : MonoBehaviour
{
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;
    private Mesh m_mesh;

    private Vector3[] m_vertices;
    private Vector3[] m_normals;
    private Vector2[] m_uvs;

   
    void OnEnable()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_mesh = m_meshFilter.mesh;


        m_vertices = m_mesh.vertices;
        m_normals = m_mesh.normals;
        m_uvs = m_mesh.uv;

    }

    public void ExplodeMesh()
    {

        for (int i = 0; i < m_mesh.subMeshCount; i++)
        {
            int[] indices = m_mesh.GetTriangles(i);

            for (int j = 0; j < indices.Length; j += 3) //three indices at a time, for a triangle
            {
                Vector3[] newVertices = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];

                for (int k = 0; k < 3; k++)
                {
                    int index = indices[i + k];
                    newVertices[k] = m_vertices[index];
                    newNormals[k] = m_normals[index];
                    newUvs[k] = m_uvs[index];
                }

                Mesh triangleMesh = new Mesh
                {
                    vertices = newVertices,
                    normals = newNormals,
                    uv = newUvs,
                    triangles = new[] {0, 1, 2, 2, 1, 0}
                };


                GameObject triangle = new GameObject("Triangle" + (j / 3));
                triangle.transform.position = transform.position;
                triangle.transform.rotation = Random.rotation;
                triangle.AddComponent<MeshRenderer>().material = m_meshRenderer.materials[i];
                triangle.AddComponent<MeshFilter>().mesh = triangleMesh;
                Vector3 explosionPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),
                    transform.position.y + Random.Range(-0.5f, 0.5f), transform.position.z + Random.Range(-0.5f, 0.5f));
                triangle.AddComponent<Rigidbody>().AddExplosionForce(1000f, explosionPos, 30);
                Destroy(triangle, 2f);
            }

        }

        gameObject.SetActive(false);
    }
}
