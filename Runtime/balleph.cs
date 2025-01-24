using UnityEngine;

public class AdvectRigidbody : MonoBehaviour
{
    public Rigidbody grandeBille;
    public Rigidbody[] billes;
    public float forceDetraction = 150f;
    public float forceRepulsion = 80f;
    public float rayonGrandeBille = 0.7f;
    public float viscosite = 0.8f;
    private LineRenderer[] lines;
    public Material lineMaterial;
    public Color lineColor = Color.blue;
    public Material grandeBilleMaterial;
    
    void Start()
    {
        billes = new Rigidbody[10];
        lines = new LineRenderer[10];

        grandeBille = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<Rigidbody>();
        grandeBille.transform.position = new Vector3(0, 1, 0);
        grandeBille.transform.localScale = Vector3.one * 1.4f;
        grandeBille.mass = 5.65f;
        grandeBille.useGravity = true;
        grandeBille.linearDamping = 1f;
        
        Renderer grandeBilleRenderer = grandeBille.GetComponent<Renderer>();
        if (grandeBilleMaterial != null)
            grandeBilleRenderer.material = grandeBilleMaterial;

        for (int i = 0; i < billes.Length; i++)
        {
            billes[i] = CreateBille();
            billes[i].GetComponent<Renderer>().enabled = false;
            
            lines[i] = billes[i].gameObject.AddComponent<LineRenderer>();
            lines[i].startWidth = 0.05f;
            lines[i].endWidth = 0.05f;
            lines[i].material = new Material(Shader.Find("Sprites/Default"));
            lines[i].material.color = lineColor;
            lines[i].positionCount = 2;
        }
    }

    private Rigidbody CreateBille()
    {
        GameObject bille = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Vector3 randomPos = Random.insideUnitSphere * (rayonGrandeBille * 0.8f);
        bille.transform.position = grandeBille.transform.position + randomPos;
        bille.transform.localScale = Vector3.one * 0.1f;
        
        Rigidbody rb = bille.AddComponent<Rigidbody>();
        rb.mass = 0.565f;
        rb.useGravity = true;
        rb.linearDamping = viscosite;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        return rb;
    }

    void FixedUpdate()
    {
        Vector3 grandeBilleVelocity = grandeBille.linearVelocity;

        for (int i = 0; i < billes.Length; i++)
        {
            Vector3 centreVersParticule = billes[i].position - grandeBille.position;
            float distance = centreVersParticule.magnitude;

            if (distance > rayonGrandeBille * 0.9f)
            {
                Vector3 directionVersInterieur = -centreVersParticule.normalized;
                billes[i].AddForce(directionVersInterieur * forceDetraction * distance);
            }

            billes[i].linearVelocity += grandeBilleVelocity * 0.1f;

            for (int j = 0; j < billes.Length; j++)
            {
                if (i != j)
                {
                    Vector3 repulsion = billes[i].position - billes[j].position;
                    float dist = repulsion.magnitude;
                    if (dist < 0.15f)
                    {
                        billes[i].AddForce(repulsion.normalized * forceRepulsion * (1f/dist));
                    }
                }
            }

            lines[i].SetPosition(0, billes[i].position);
            lines[i].SetPosition(1, grandeBille.position);
        }
    }

    void OnDestroy()
    {
        foreach (LineRenderer line in lines)
        {
            if (line != null && line.material != null)
            {
                Destroy(line.material);
            }
        }
    }
}