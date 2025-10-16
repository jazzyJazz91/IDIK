using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Obstacle settings")]
    public GameObject[] obstacles;   // flera obstacles
    public Vector3 spawnPos = new Vector3(10, 0, 0);

    [Header("Timing settings")]
    public float minSpawnDelay = 1.0f;
    public float maxSpawnDelay = 3.0f;
    public float startDelay = 2.0f;

    private PlayerController playerControllerScript;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        // Vänta lite innan första spawnen
        yield return new WaitForSeconds(startDelay);

        while (!playerControllerScript.gameOver)
        {
            // välj ett random obstacle ur listan
            int index = Random.Range(0, obstacles.Length);

            // skapa det
            Instantiate(obstacles[index], spawnPos, obstacles[index].transform.rotation);

            // vänta en slumpmässig tid innan nästa spawn
            float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(randomDelay);
        }
    }
}
