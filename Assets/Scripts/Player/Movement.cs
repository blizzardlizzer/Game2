using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the scene

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float acceleration = 2.0f;
    public float maxSpeed = 5.0f;
    public float drag = 0.98f; // Friction for drift
    public float rotationFactor = 20f; // Rotation based on acceleration
    public float rotationSmoothing = 5.0f; // Smoothing factor for rotation lerp
    public float waitTime = 5.0f;

    private float velocity = 0.0f;
    private float input;
    private bool hasStarted = false;
    private bool isWaiting = false;
    private bool isDead = false;
    private bool canRestart = false;
    private float currentAcceleration = 0.0f;
    private float targetRotation = 0.0f;
    private float smoothRotation = 0.0f;

    [Header("References")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Sprite noBoosterSprite;
    public TextMeshProUGUI centerText;
    private Rigidbody2D rb;

    void Start()
    {
        spriteRenderer.sprite = noBoosterSprite; // Show og sprite at start
        animator.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        centerText.text = "Press any key to LIFTOFF!!";

        Debug.Log("Player initialized.");
    }

    void Update()
    {
        if (isDead)
        {
            if (canRestart && Input.anyKeyDown)
            {
                RestartGame();
            }
            centerText.text = "You went BLAM!";
            return;
        }
        
        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                hasStarted = true;
                StartCoroutine(StartDelay());
                Debug.Log($"Game started, waiting for {waitTime} seconds.");
            }
            return;
        }

        if (!isWaiting)
        {
            input = Input.GetAxis("Horizontal");

            if (input != 0)
                Debug.Log("Player input detected: " + input);
        }
    }

    void FixedUpdate()
    {
        if (isDead || !hasStarted || isWaiting) return;

        float prevVelocity = velocity;

        if (input != 0)
        {
            velocity += input * acceleration * Time.fixedDeltaTime;
            velocity = Mathf.Clamp(velocity, -maxSpeed, maxSpeed);
        }
        else
        {
            velocity *= drag;
        }

        currentAcceleration = velocity - prevVelocity;

        transform.position += Vector3.right * velocity * Time.fixedDeltaTime;

        targetRotation = -currentAcceleration * rotationFactor;
        smoothRotation = Mathf.Lerp(smoothRotation, targetRotation, Time.fixedDeltaTime * rotationSmoothing);
        transform.rotation = Quaternion.Euler(0, 0, smoothRotation);

        Debug.Log("Velocity: " + velocity);
    }

    private System.Collections.IEnumerator StartDelay()
    {
        isWaiting = true;
        centerText.text = "3";
        yield return new WaitForSeconds(1f);
        centerText.text = "2";
        yield return new WaitForSeconds(1f);
        centerText.text = "1";
        yield return new WaitForSeconds(1f);
        centerText.text = "LIFTOFF!!!";
        animator.enabled = true; // Start animation
        yield return new WaitForSeconds(1.2f);
        centerText.text = "";
        isWaiting = false;
        Debug.Log("Player can now move.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            velocity = 0;
            isDead = true;
            animator.Play("Death"); // Placeholder animation
            Debug.Log($"Player hit [{collision.gameObject.name}] border and died!");
            StartCoroutine(EnableRestart());
        }
    }

    private System.Collections.IEnumerator EnableRestart()
    {
        yield return new WaitForSeconds(0.5f);
        canRestart = true;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}