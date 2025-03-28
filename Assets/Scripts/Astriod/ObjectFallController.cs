using System.Collections;
using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject[] fallingPrefabs; 
    public float initialMinWait = 1.5f;
    public float initialMaxWait = 3.0f;
    public float difficultyRampRate = 0.05f; 
    public float minWaitLimit = 0.5f; 

    private float currentMinWait;
    private float currentMaxWait;
    private bool isSpawning = false;

    void Start()
    {
        currentMinWait = initialMinWait;
        currentMaxWait = initialMaxWait;
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            float waitTime = Random.Range(currentMinWait, currentMaxWait);
            yield return new WaitForSeconds(waitTime);

            SpawnObject();

            currentMinWait = Mathf.Max(minWaitLimit, currentMinWait - difficultyRampRate);
            currentMaxWait = Mathf.Max(minWaitLimit + 0.1f, currentMaxWait - difficultyRampRate);
        }
    }

    void SpawnObject()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 10f, 0f);
        int randomIndex = Random.Range(0, fallingPrefabs.Length);

        GameObject obj = Instantiate(fallingPrefabs[randomIndex], spawnPosition, Quaternion.identity);

        if (obj.TryGetComponent<FallingObject>(out FallingObject falling))
        {
            falling.fallSpeed += Random.Range(-2f, 2f);
        }
    }
}
