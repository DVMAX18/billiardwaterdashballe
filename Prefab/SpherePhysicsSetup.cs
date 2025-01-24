using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class SpherePhysicsSetup : MonoBehaviour
{
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = 1.0f;
        rb.centerOfMass = new Vector3(0, 0.01875f, 0);
        
        PhysicsMaterial material = new PhysicsMaterial();
        material.staticFriction = 0.6f;
        material.dynamicFriction = 0.4f;
        material.frictionCombine = PhysicsMaterialCombine.Average;
        material.bounciness = 0.3f;
        material.bounceCombine = PhysicsMaterialCombine.Average;
        Collider collider = GetComponent<Collider>();
        collider.material = material;
    }
}
