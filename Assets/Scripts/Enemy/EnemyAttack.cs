using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    // As named, attack speed of a bunny
    public float timeBetweenAttacks = 0.5f;

    // Damage can deal of a bunny
    public int attackDamage = 10;

    // A reference to the animator component
    Animator anim;

    // A reference to the player GameObject
    GameObject player;

    // A reference to the player's PlayerHealth component
    // NOTE1: Class::PlayerHealth is defined on PlayerHealth.cs
    PlayerHealth playerHealth;

    EnemyHealth enemyHealth;

    // Whether player is close enough for bunny to attack
    bool playerInRange;

    // Timer for couting up the next attack
    // NOTE1: work with variable timeBetweenAttacks
    float timer;

    // Setup references
    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth> ();
        anim = GetComponent <Animator> ();
    }

    // Callback for bunny's sphere collider if it collides with others
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player) {
            playerInRange = true;
        }
    }

    // Callback for bunny's sphere collider if the collision ends
    void OnTriggerExit (Collider other)
    {
        if (other.gameObject == player) {
            playerInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if (
            timer >= timeBetweenAttacks &&
            playerInRange &&
            enemyHealth.currentHealth > 0
        ) {
            Attack ();
        }

        if (playerHealth.currentHealth <= 0) {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void Attack ()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0) {
            playerHealth.TakeDamage (attackDamage);
        }
    }
}
