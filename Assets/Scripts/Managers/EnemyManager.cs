using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // A reference to the player health
    public PlayerHealth playerHealth;

    // A reference to the enemy prefab
    public GameObject enemy;

    // How long between each spawn
    public float spawnTime = 3f;

    // Define where are spawn points
    public Transform[] spawnPoints;

    // Game start
    void Start ()
    {
        // Wait spawnTime first, then call Spawn() in number of spawnTime times
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
        // Do nothing if player die
        if (playerHealth.currentHealth <= 0f) {
            return;
        }

        // Define where will be the spawn point
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        // Instantiate a kind of enemy at spawnPoints
        Instantiate (
            enemy,
            spawnPoints [spawnPointIndex].position,
            spawnPoints [spawnPointIndex].rotation);
    }
}
