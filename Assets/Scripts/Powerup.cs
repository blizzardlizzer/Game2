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

    void Start()
    {
        // Get screen edges in world coordinates
        screenLeft = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        screenRight = Camera.main.ViewportToWorldPoint(Vector3.one).x;
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

        // Check if it hits the screen edges on the X axis
        if (
            transform.position.x + spriteWidth > screenRight
            || transform.position.x - spriteWidth < screenLeft
        )
        {
            directionX *= -1; // Reverse direction on X axis
        }

        // Check if it hits the screen edges on the Y axis
        if (
            transform.position.y + spriteHeight > screenTop
            || transform.position.y - spriteHeight < screenBottom
        )
        {
            directionY *= -1; // Reverse direction on Y axis
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Log the name of the object we collided with
        Debug.Log("Collided with: " + collision.gameObject.name);
    }
}
