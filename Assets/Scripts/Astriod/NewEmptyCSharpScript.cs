using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float fallSpeed = 5f; // Speed at which the object falls.
    public float maxFallHeight = -5f; // Maximum Y position (the object will be destroyed when it reaches this point).

    // Encapsulation: This ensures that the object's speed and position are controlled through methods.
    public void Fall()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        if (transform.position.y < maxFallHeight)
        {
            Destroy(gameObject); // Destroy the object when it falls off-screen.
        }
    }
}

public class Asteroid : FallingObject
{
    // Additional properties specific to asteroids (e.g., size, damage)
    public float damage = 10f;

    void Start()
    {
        // Optional: Give the asteroid a random rotation when it falls
        float randomRotation = Random.Range(0f, 360f);
        transform.Rotate(Vector3.forward, randomRotation);
    }

    void Update()
    {
        Fall(); // Call the base Fall method to handle falling logic.
    }
}

public class FallingObjectSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab; // The prefab for the falling object (asteroid).
    public float spawnInterval = 2f; // Time interval between spawns.
    public Vector2 spawnRange = new Vector2(-5f, 5f); // Range within which objects will spawn.

    void Start()
    {
        InvokeRepeating("SpawnObject", 0f, spawnInterval); // Repeatedly call SpawnObject method at intervals.
    }

    void SpawnObject()
    {
        float spawnX = Random.Range(spawnRange.x, spawnRange.y);
        Vector3 spawnPosition = new Vector3(spawnX, transform.position.y, 0f); // Set spawn position above the screen.
        Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity); // Instantiate the asteroid prefab at spawn position.
    }
}