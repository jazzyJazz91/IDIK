using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    public static float globalSpeedMultiplier = 1f; // 👈 ny rad
    public float baseSpeed = 30f;                   // byt namn så vi vet att detta är grundfarten
    private PlayerController playerControllerScript;
    private float leftBound = -15f;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerControllerScript.gameOver)
        {
            // fiendens hastighet = basfart * global multiplier
            float currentSpeed = baseSpeed * globalSpeedMultiplier;
            transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
        }

        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}
