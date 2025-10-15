using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    public float jumpForce = 10f;
    public float gravityModifier = 1f;
    public bool isOnGround = true;
    public bool gameOver = false;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    private int jumpCount = 0;       // how many jumps have been made
    public int maxJumps = 2;         // max jumps
    public ParticleSystem dirtParticle;
    public ParticleSystem explosionParticle;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        // make gravity more noticeable
        Physics.gravity = new Vector3(0, -9.81f * gravityModifier, 0);
    }

    void Update()
    {
        // only jump if game is not over
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps && !gameOver)
        { playerAudio.PlayOneShot(jumpSound, 1.0f);
            dirtParticle.Stop();

            // reset Y velocity before jump
            playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0, playerRb.linearVelocity.z);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;

            playerAnim.SetTrigger("Jump_trig");
            isOnGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // hitting the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            dirtParticle.Play();
            isOnGround = true;
            jumpCount = 0;
        }
        // hitting obstacle
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerAudio.PlayOneShot(crashSound, 1.0f);
            dirtParticle.Stop();
            gameOver = true;
            Debug.Log("Game Over!");

            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);

            // player falling backward when hit
            playerRb.AddForce(Vector3.back * 5f + Vector3.up * 3f, ForceMode.Impulse);
        }
    }
}
