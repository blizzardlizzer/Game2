using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFallController : MonoBehaviour
{
    public float waitTime = 1.5f;
    public GameObject fallingObject;
    public float fallSpeed = 2f;
    public Sprite[] asteroidSprites;
    public Sprite fastAsteroidSprite;

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

            FallingObject fallingScript = fallingObject.GetComponent<FallingObject>();
            if (fallingScript != null)
            {
                if (selectedSprite == fastAsteroidSprite)
                {
                    fallingScript.fallSpeed = 10f;
                }
                else
                {
                    fallingScript.fallSpeed = fallSpeed;
                }
            }
        }
    }
    }


