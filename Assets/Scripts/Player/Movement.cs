using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public TextMeshProUGUI centerText;
    public BackgroundMover gameBackground;
    public ObjectFallController objectController;
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

    void Start()
    {
        animator.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        // Technically theres no sound in space right?
        // loopingSource.enabled = false;
        // loopingSource.loop = true;
        // loopingSource.volume = 0.5f;

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
        centerText.text = "LIFTOFF!";
        yield return StartCoroutine(MoveDownOverTime(-3f, 3f));
    }

    private System.Collections.IEnumerator MoveDownOverTime(float targetY, float duration)
    {
        audioSource.PlayOneShot(rocketLaunch);
        animator.enabled = true;
        yield return new WaitForSeconds(0.75f);
        gameBackground.StartMoving();
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(startPos.x, targetY, startPos.z);
        
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        yield return new WaitForSeconds(2f);
        isWaiting = false;
        audioSource.PlayOneShot(weHaveLiftoff);
        // Again, no sound in space right? (It sounds like crap)
        // loopingSource.clip = rocketLoop;
        // loopingSource.enabled = true;
        // loopingSource.Play();
        centerText.text = "";
        objectController.StartFall();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            velocity = 0;
            isDead = true;
            animator.Play("Death"); // Placeholder animation
            Debug.Log("Player hit border and died: " + collision.gameObject.name);
            StartCoroutine(EnableRestart());
        } else if (collision.gameObject.CompareTag("Hazard")){
            velocity = 0;
            isDead = true;
            animator.Play("Death"); // Placeholder animation
            Debug.Log("Player hit object and died: " + collision.gameObject.name);
            StartCoroutine(EnableRestart());
        }
    }

    private System.Collections.IEnumerator EnableRestart()
    {
        yield return new WaitForSeconds(.5f);
        canRestart = true;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}