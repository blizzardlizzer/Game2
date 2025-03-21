using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    public float waitTime = 0.25f;
    public GameObject fallingObject;
    public float fallSpeed = 2f;
    public Sprite[] asteroidSprites;
    public Sprite fastAsteroidSprite;

    // Start is called before the first frame update
    public void StartFall()
    {
            InvokeRepeating("Fall", waitTime, waitTime);
    }


    void Fall()
    {
        GameObject obj = Instantiate(fallingObject, new Vector3(Random.Range(-10, 10), 10, 0), Quaternion.identity);

        SpriteRenderer spriteRenderer = fallingObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && asteroidSprites.Length > 0)
        {
            Sprite selectedSprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
            spriteRenderer.sprite = selectedSprite;

            // Access the FallingObject script and set the fall speed
            FallingObject fallingScript = fallingObject.GetComponent<FallingObject>();
            if (fallingScript != null)
            {
                // Check if it's the fast asteroid and set speed accordingly
                if (selectedSprite == fastAsteroidSprite)
                {
                    fallingScript.fallSpeed = 10f;  // Set faster speed
                }
                else
                {
                    fallingScript.fallSpeed = fallSpeed;  // Set normal speed
                }
            }
        }
    }
    }


