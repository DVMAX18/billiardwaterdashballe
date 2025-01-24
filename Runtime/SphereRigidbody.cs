using UnityEngine;

public class SphereRigidbody : MonoBehaviour
{
    public Rigidbody sphereRigidbody;
    public float coefficientFrottement = 0.1f;
    public float coefficientTraînée = 0.05f;
    public float restitution = 0.8f;

    private Vector3 gravité = new Vector3(0, -9.81f, 0);

    void Start()

    {
        if (sphereRigidbody == null)
        {
            sphereRigidbody = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        Vector3 forceGravité = sphereRigidbody.mass * gravité;
        sphereRigidbody.AddForce(forceGravité);

        Vector3 forceFrottement = -coefficientFrottement * sphereRigidbody.linearVelocity;
        sphereRigidbody.AddForce(forceFrottement);

        Vector3 forceTraînée = -coefficientTraînée * sphereRigidbody.linearVelocity.sqrMagnitude * sphereRigidbody.linearVelocity.normalized;
        sphereRigidbody.AddForce(forceTraînée);
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Rigidbody autreRigidbody = contact.otherCollider.attachedRigidbody;
            if (autreRigidbody != null)
            {
                Vector3 relativeVelocity = sphereRigidbody.linearVelocity - autreRigidbody.linearVelocity;
                Vector3 impulse = (1 + restitution) * relativeVelocity / (1 / sphereRigidbody.mass + 1 / autreRigidbody.mass);
                sphereRigidbody.linearVelocity -= impulse / sphereRigidbody.mass;
                autreRigidbody.linearVelocity += impulse / autreRigidbody.mass;
            }
        }
    }
}