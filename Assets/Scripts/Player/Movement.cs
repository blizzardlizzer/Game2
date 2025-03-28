using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public int maxHealth = 3;
    private int health = 3;

    [Header("Movement")]
    public float acceleration = 2.0f;
    public float maxSpeed = 5.0f;
    public float drag = 0.98f;
    public float rotationFactor = 20f;
    public float rotationSmoothing = 5.0f;

    private float velocity = 0.0f;
    private float input;
    private bool hasStarted = false;
    private bool isWaiting = false;
    private bool isDead = false;
    private bool canRestart = false;
    private float currentAcceleration = 0.0f;
    private float targetRotation = 0.0f;
    private float smoothRotation = 0.0f;
    private bool isInvincible = false;

    [Header("References")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public TextMeshProUGUI centerText;
    public BackgroundMover gameBackground;
    public ObjectFallController objectController;
    public Transform cameraTransform;
    public Timer timer;
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
        centerText.text = "Press any key to LIFTOFF!!";

        acceleration = 2.0f;
        maxSpeed = 5.0f; // reset after powerup
    }

    void Update()
    {
        if (health <= 0 && !isDead)
        {
            velocity = 0;
            isDead = true;
            animator.Play("RSD");
            timer.StopStopwatch();
            StartCoroutine(EnableRestart());
        }

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
        centerText.text = "";
        timer.StartStopwatch();
        objectController.StartSpawning();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            health -= 9999999;
        }
        else if (collision.gameObject.CompareTag("Hazard") && !isInvincible)
        {
            health--;
            StartCoroutine(FlashDamage());
        }
    }

    private System.Collections.IEnumerator FlashDamage()
    {
        isInvincible = true;
        StartCoroutine(ScreenShake());

        for (int i = 0; i < 6; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    private System.Collections.IEnumerator ScreenShake()
    {
        Vector3 originalPos = cameraTransform.position;
        float duration = 0.3f;
        float magnitude = 0.15f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            cameraTransform.position = originalPos + new Vector3(offsetX, offsetY, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.position = originalPos;
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

    public int GetHealth(){
        return health;
    }
}
