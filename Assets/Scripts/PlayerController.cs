using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;

    [Header("Movement & Jumping")]
    public float jumpForce = 10f;
    public float gravityModifier = 1f;
    public bool isOnGround = true;
    public bool gameOver = false;
    private int jumpCount = 0;
    public int maxJumps = 2;

    [Header("Particles & Sounds")]
    public ParticleSystem dirtParticle;
    public ParticleSystem explosionParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    [Header("Health System")]
    public int maxHealth = 3;
    private int currentHealth;
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image fillImage;

    [Header("UI")]
    public TextMeshProUGUI gameOverText;   // TMP för “Game Over”
    public Button restartButton;           // UI Button istället för TMP-text

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        Physics.gravity = new Vector3(0, -9.81f * gravityModifier, 0);

        // Initiera hälsa
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Dölj Game Over och knapp i början
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
            restartButton.onClick.AddListener(RestartGame); // ← kopplar knappen till funktionen
        }
    }

    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
            {
                playerAudio.PlayOneShot(jumpSound, 1.0f);
                dirtParticle.Stop();

                playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z);
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCount++;

                playerAnim.SetTrigger("Jump_trig");
                isOnGround = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            dirtParticle.Play();
            isOnGround = true;
            jumpCount = 0;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerAudio.PlayOneShot(crashSound, 1.0f);
            dirtParticle.Stop();
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FrogBody>())
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            TriggerGameOver();
        }
        else
        {
            playerAnim.SetTrigger("Hit");
        }
    }

    private void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;

            if (fillImage != null && healthGradient != null)
                fillImage.color = healthGradient.Evaluate((float)currentHealth / maxHealth);
        }
    }

    private void TriggerGameOver()
    {
        if (gameOver) return;

        gameOver = true;
        Debug.Log("GAME OVER – alla liv slut!");

        playerAnim.SetBool("Death_b", true);
        playerAnim.SetInteger("DeathType_int", 1);

        if (gameOverText != null) gameOverText.gameObject.SetActive(true);
        if (restartButton != null) restartButton.gameObject.SetActive(true);

        Time.timeScale = 0f; // stoppar spelet
    }

    // 👇 Den här körs när du klickar på restart-knappen
    private void RestartGame()
    {
        Time.timeScale = 1f; // starta tiden igen
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
