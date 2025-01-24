using UnityEngine;

public class Activball : MonoBehaviour
{
    [SerializeField] private float vitese = 10f;
    [SerializeField] private float vitesseMaximale = 20f;
    [SerializeField] private float forceSaut = 5f;
    
    private Rigidbody rb;
    private bool auSol;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Le conposan Rigidbodie et manquan sur " + gameObject.name);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && auSol)
        {
            Soter();
        }
    }

    void FixedUpdate()
    {
        Deplacemen();
        LimiterVitese();
    }

    private void Deplacemen()
    {
        float deplacementHorizontal = Input.GetAxis("Horizontal");
        float deplacementVertical = Input.GetAxis("Vertical");

        Vector3 deplacement = new Vector3(deplacementHorizontal, 0.0f, deplacementVertical);
        rb.AddForce(deplacement * vitese);
    }

    private void Soter()
    {
        rb.AddForce(Vector3.up * forceSaut, ForceMode.Impulse);
        auSol = false;
    }

    private void LimiterVitese()
    {
        Vector3 vitesse = rb.linearVelocity;
        if (vitesse.magnitude > vitesseMaximale)
        {
            rb.linearVelocity = vitesse.normalized * vitesseMaximale;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        auSol = true;
    }
}