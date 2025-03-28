using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public float baseFallSpeed = 8f;
    public float accelerationOverTime = 0.2f;
    public List<Sprite> possibleSprites;
    public Sprite fastAsteroidSprite;
    public Color fastAsteroidTrailColor = Color.yellow;

    private SpriteRenderer spriteRenderer;
    private Vector3 fallDirection;
    private float spinSpeed;
    public ParticleSystem particles;
    private TrailRenderer trail;
    private float spawnTime;
    private float fallSpeed;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
        spawnTime = Time.time;
        fallSpeed += Random.Range(2f, -2f);

        if (particles != null)
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particles.gameObject.SetActive(true);
        }

        if (possibleSprites != null && possibleSprites.Count > 0 && spriteRenderer != null)
        {
            List<Sprite> options = new List<Sprite>(possibleSprites);
            if (!options.Contains(fastAsteroidSprite))
            {
                options.Add(fastAsteroidSprite);
            }

            Sprite chosen = options[Random.Range(0, options.Count)];
            spriteRenderer.sprite = chosen;

            fallSpeed = baseFallSpeed + (Time.timeSinceLevelLoad * accelerationOverTime);

            if (chosen == fastAsteroidSprite)
            {
                fallSpeed += 7f;
                if (trail != null)
                {
                    trail.startColor = fastAsteroidTrailColor;
                    trail.endColor = fastAsteroidTrailColor;
                }
            }
        }
        else
        {
            fallSpeed = baseFallSpeed + (Time.timeSinceLevelLoad * accelerationOverTime);
        }

        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        spinSpeed = Random.Range(-180f, 180f);

        float angleOffset = Random.Range(-0.3f, 0.3f);
        fallDirection = (Vector2.down + Vector2.right * angleOffset).normalized;
    }

    protected virtual void Update()
    {
        transform.Translate(fallDirection * fallSpeed * Time.deltaTime, Space.World);
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (particles != null && !collision.CompareTag("Border"))
        {
            particles.transform.SetParent(null);
            particles.Play();
            Destroy(particles.gameObject, particles.main.duration);
        }

        Destroy(gameObject);
    }
    public float GetFallSpeed(){
        return fallSpeed;
    }
}
