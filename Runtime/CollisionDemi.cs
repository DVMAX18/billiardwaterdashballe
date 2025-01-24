using UnityEngine;

public class CollisionDemi : MonoBehaviour
{
    [SerializeField] private float rayonExterieur = 0.1f;
    [SerializeField] private float epaisseur = 0.02f;
    
    private MeshCollider meshCollider;
    private Rigidbody rb;
    
    void Start()
    {
        CreateFullSphereCollider();
        SetupRigidbody();
    }

    void SetupRigidbody()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 1.0f;
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.05f;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void CreateFullSphereCollider()
    {
        Mesh sphereMesh = new Mesh();
        int segments = 32;
        int rings = 16;
        Vector3[] vertices = new Vector3[(segments + 1) * (rings + 1) * 2];
        int vertexIndex = 0;
        
        for (int layer = 0; layer < 2; layer++)
        {
            float radius = layer == 0 ? rayonExterieur : rayonExterieur - epaisseur;
            for (int ring = 0; ring <= rings; ring++)
            {
                float phi = ring * Mathf.PI / rings;
                for (int segment = 0; segment <= segments; segment++)
                {
                    float theta = segment * 2 * Mathf.PI / segments;
                    float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                    float y = radius * Mathf.Cos(phi);
                    float z = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
                    vertices[vertexIndex++] = new Vector3(x, y, z);
                }
            }
        }
        
        int[] triangles = new int[segments * rings * 12];
        int triangleIndex = 0;
        
        for (int ring = 0; ring < rings; ring++)
        {
            for (int segment = 0; segment < segments; segment++)
            {
                int current = ring * (segments + 1) + segment;
                int next = current + segments + 1;
                
                triangles[triangleIndex++] = current;
                triangles[triangleIndex++] = current + 1;
                triangles[triangleIndex++] = next + 1;
                
                triangles[triangleIndex++] = current;
                triangles[triangleIndex++] = next + 1;
                triangles[triangleIndex++] = next;
                
                int innerCurrent = current + (segments + 1) * (rings + 1);
                int innerNext = next + (segments + 1) * (rings + 1);
                
                triangles[triangleIndex++] = innerCurrent;
                triangles[triangleIndex++] = innerNext + 1;
                triangles[triangleIndex++] = innerCurrent + 1;
                
                triangles[triangleIndex++] = innerCurrent;
                triangles[triangleIndex++] = innerNext;
                triangles[triangleIndex++] = innerNext + 1;
            }
        }
        
        sphereMesh.vertices = vertices;
        sphereMesh.triangles = triangles;
        sphereMesh.RecalculateNormals();
        
        meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = sphereMesh;
        meshCollider.convex = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Colision avec : " + collision.gameObject.name);
    }
}