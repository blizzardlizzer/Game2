using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    public float speed = 3f; // Movement speed
    private int directionX; // 1 = Right, -1 = Left
    private int directionY; // 1 = Up, -1 = Down
    private float screenLeft,
        screenRight,
        screenTop,
        screenBottom;
    private float spriteWidth,
        spriteHeight;

    public int spawnDelay = 30;

    void Start()
    {
        // Get screen edges in world coordinates
        screenLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).x;
        screenRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0)).x;
        screenTop = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1, 0)).y;
        screenBottom = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0, 0)).y;


        // Get sprite width and height
        spriteWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        spriteHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        // Start with a random direction on both axes
        directionX = Random.Range(0, 2) == 0 ? 1 : -1; // Randomly choose 1 (Right) or -1 (Left)
        directionY = Random.Range(0, 2) == 0 ? 1 : -1; // Randomly choose 1 (Up) or -1 (Down)
    }

    void Update()
    {
        // Move left and right (X axis)
        transform.position += Vector3.right * speed * directionX * Time.deltaTime;

        // Move up and down (Y axis)
        transform.position += Vector3.up * speed * directionY * Time.deltaTime;

        // Change directions and move slightly so the sprite does not get stuck on the edge
        if (transform.position.x + spriteWidth > screenRight)
        {
            transform.position = new Vector3(screenRight - spriteWidth, transform.position.y, transform.position.z);
            directionX *= -1;
        }
        else if (transform.position.x - spriteWidth < screenLeft)
        {
            transform.position = new Vector3(screenLeft + spriteWidth, transform.position.y, transform.position.z);
            directionX *= -1;
        }

        if (transform.position.y + spriteHeight > screenTop)
        {
            transform.position = new Vector3(transform.position.x, screenTop - spriteHeight, transform.position.z);
            directionY *= -1;
        }
        else if (transform.position.y - spriteHeight < screenBottom)
        {
            transform.position = new Vector3(transform.position.x, screenBottom + spriteHeight, transform.position.z);
            directionY *= -1;
        }

    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the "Player" tag
        {
            
            PlayerMovement player = other.GetComponent<PlayerMovement>(); // Get the PlayerController script
            if (player != null)
            {
                player.acceleration *= 2; // Increase player speed by 2
            }

            Debug.Log("Power-up collected! Speed boosted.");
            Destroy(gameObject); // Destroy the power-up
        }
    }
}
