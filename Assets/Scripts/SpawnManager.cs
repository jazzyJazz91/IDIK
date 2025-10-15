using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject Obstacle;
    private Vector3 spawnPos = new Vector3(10, 0, 0);
    private float startDelay = 2;
    private float repeatRate = 2;
    private PlayerController playerControllerScript;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        if (playerControllerScript.gameOver == false)
        {
            Instantiate(Obstacle, spawnPos, Obstacle.transform.rotation);
        }
    }
}
