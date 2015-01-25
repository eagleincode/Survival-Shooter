using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    // Who will be traced by enemy
    Transform player;
    //PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    NavMeshAgent nav;

    // Setup references for this instance
    void Awake ()
    {
        // Get reference of the player's transform
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        //playerHealth = player.GetComponent <PlayerHealth> ();
        //enemyHealth = GetComponent <EnemyHealth> ();
        // Get reference for nav
        nav = GetComponent <NavMeshAgent> ();
    }


    void Update ()
    {
        //if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
        //{
        // Instrcut enemy heads to where player is
        // 
        // NOTE1: Nav ensure bunny won't crashed with obstacles like other
        // bunnys or environment obstacles
        nav.SetDestination (player.position);
        //}
        //else
        //{
        //    nav.enabled = false;
        //}
    }
}
