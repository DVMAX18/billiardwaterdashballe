using UnityEngine;
using System.Collections;

public class CreateSphere : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField, Range(1, 3)]
    private int numberOfBallsToSpawn = 3;
    [SerializeField]
    private float explosionForce = 10f;
    [SerializeField]
    private float explosionRadius = 2f;
    [SerializeField]
    private float ballLifetime = 3f;

    private readonly float collisionResetTime = 0.1f;
    private int collisionCount = 0;
    private float lastCollisionTime = 0f;
    private float lastSpawnTime = 0f;

    void Start()
    {
        numberOfBallsToSpawn = Mathf.Min(numberOfBallsToSpawn, 3);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Time.time - lastCollisionTime < collisionResetTime)
        {
            collisionCount++;
            if (collisionCount >= 2)
            {
                StartCoroutine(SpawnBalls(collision.contacts[0].point));
                collisionCount = 0;
            }
        }
        else
        {
            collisionCount = 1;
        }
        lastCollisionTime = Time.time;
    }

    IEnumerator SpawnBalls(Vector3 explosionPoint)
    {
        for (int i = 0; i < numberOfBallsToSpawn; i++)
        {
            GameObject ball = Instantiate(ballPrefab, explosionPoint, Random.rotation);
            ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            if (!ball.TryGetComponent<Rigidbody>(out var rb))
            {
                rb = ball.AddComponent<Rigidbody>();
            }
            
            Vector3 randomDir = Random.insideUnitSphere.normalized;
            rb.AddForce(randomDir * explosionForce, ForceMode.Impulse);
            
            Destroy(ball, ballLifetime);
        }
        yield return new WaitForSeconds(2f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(SpawnBalls(transform.position));
        }

        if (Time.time - lastSpawnTime > 1f)
        {
            StartCoroutine(SpawnBalls(transform.position));
            lastSpawnTime = Time.time;
        }
    }
}