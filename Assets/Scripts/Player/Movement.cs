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

    private float velocity = 0.0f;
    private float input;
    private bool hasStarted = false;
    private bool isWaiting = false;
    private bool isDead = false;
    private bool canRestart = false; // Allows restart after death
    private float currentAcceleration = 0.0f;
    private float targetRotation = 0.0f;
    private float smoothRotation = 0.0f;

    [Header("References")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Sprite noBoosterSprite;
    public TextMeshProUGUI centerText;
    private Rigidbody2D rb;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource loopingSource;
    public AudioClip countdownThree;
    public AudioClip countdownTwo;
    public AudioClip countdownOne;
    public AudioClip ignition;
    public AudioClip weHaveLiftoff;
    public AudioClip rocketLaunch;
    public AudioClip rocketLoop;
    public float rocketLoopVolume = 0.5f;

    void Start()
    {
        spriteRenderer.sprite = noBoosterSprite; // Show og sprite at start
        animator.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        loopingSource.enabled = false;
        loopingSource.loop = true;
        loopingSource.volume = rocketLoopVolume;

        Debug.Log("Player initialized.");
        centerText.text = "Press any key to LIFTOFF!!";
    }

    void Update()
    {
        if (isDead)
        {
            if (canRestart && Input.anyKeyDown)
            {
                RestartGame();
            }
            return;
        }
        
        if (!hasStarted)
        {
            if (Input.anyKeyDown)
            {
                hasStarted = true;
                StartCoroutine(StartCountdown());
                Debug.Log("Game started, waiting for countdown.");
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

    private System.Collections.IEnumerator StartCountdown()
    {
        isWaiting = true;
        audioSource.PlayOneShot(countdownThree);
        centerText.text = "THREE";
        yield return new WaitForSeconds(1f);

        audioSource.PlayOneShot(countdownTwo);
        centerText.text = "TWO";
        yield return new WaitForSeconds(1f);

        audioSource.PlayOneShot(countdownOne);
        centerText.text = "ONE";
        yield return new WaitForSeconds(1f);

        audioSource.PlayOneShot(ignition);
        audioSource.PlayOneShot(rocketLaunch);
        animator.enabled = true;
        centerText.text = "LIFTOFF!";
        yield return new WaitForSeconds(5f);

        isWaiting = false;
        audioSource.PlayOneShot(weHaveLiftoff);
        loopingSource.clip = rocketLoop;
        loopingSource.enabled = true;
        loopingSource.Play();
        centerText.text = "";
        Debug.Log("Player can now move.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            velocity = 0;
            isDead = true;
            animator.Play("Death"); // Placeholder animation
            loopingSource.enabled = false;
            Debug.Log("Player hit border and died: " + collision.gameObject.name);
            StartCoroutine(EnableRestart());
        }
    }

    private System.Collections.IEnumerator EnableRestart()
    {
        yield return new WaitForSeconds(.5f);
        canRestart = true;
        Debug.Log("Press any key to restart.");
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}