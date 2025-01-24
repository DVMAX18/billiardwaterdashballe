using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class WaterMovement : MonoBehaviour
{
    private Rigidbody rb;
    
    [Header("Parametres physiques")]
    [SerializeField] private float masse = 1f;
    [SerializeField] private float contenuEau = 0.5f;
    [SerializeField] private float coefficientFrottement = 0.1f;
    [SerializeField] private float vitesseMaximale = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.localScale = Vector3.one * 0.2f;
        rb.mass = masse;
        rb.linearDamping = coefficientFrottement;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    
        rb.centerOfMass = Vector3.down * (contenuEau * 0.1f);
    }

    private void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > vitesseMaximale)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * vitesseMaximale;
        }
        Vector3 groundNormal = Vector3.up;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.2f))
        {
            groundNormal = hit.normal;
            float pente = Vector3.Angle(groundNormal, Vector3.up);
            
            if (pente > 0)
            {
                Vector3 directionGlissement = Vector3.ProjectOnPlane(Physics.gravity, groundNormal).normalized;
                float forceGlissement = Mathf.Sin(pente * Mathf.Deg2Rad) * masse * (1 + contenuEau);
                rb.AddForce(directionGlissement * forceGlissement, ForceMode.Force);
            }
        }
    }
}