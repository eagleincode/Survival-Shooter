using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    // Initial health point
    public int startingHealth = 100;

    // Current health point
    public int currentHealth;

    // A reference to the slider view
    public Slider healthSlider;

    // A reference to the damage view
    public Image damageImage;

    // A clip will be played when player dies
    public AudioClip deathClip;

    // Speed the damageImage will fade at
    public float flashSpeed = 5f;

    // Colour the damageImage is set to
    public Color flashColour = new Color (1f, 0f, 0f, 0.1f);

    // A reference to the Animator component
    Animator anim;

    // A reference to the AudioSource component
    AudioSource playerAudio;

    // A reference to the player's movement which defined on PlayerMovement.cs
    PlayerMovement playerMovement;

    PlayerShooting playerShooting;

    // Whether the player is dead
    bool isDead;

    // True when the player get damaged
    bool damaged;

    // Setup reference for this instance
    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
        // Change damageImage colour
        if (damaged) {
            // Change to flashColour if player had been hitted
            damageImage.color = flashColour;
        } else {
            // Clear the flashColour in a fading manner
            damageImage.color = Color.Lerp (
                damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        // Reset the damaged flag
        damaged = false;
    }

    // Handler for receving damage from Enemy
    public void TakeDamage (int amount)
    {
        // Set damaged flag so that Update() can change colour accordingly
        damaged = true;

        // Deduct the health point by damage amount
        currentHealth -= amount;

        // Update value in healthSlider to reflect the current health
        healthSlider.value = currentHealth;

        // Play the hurt sound effect
        playerAudio.Play ();

        // Let player die if it's current health belows zero
        // flag *isDead* ensures player dies one only
        if (currentHealth <= 0 && !isDead) {
            // Call Death() to handle what should do after player die
            Death ();
        }
    }

    // Handler if player die
    void Death ()
    {
        // Set flag *isDead* so that it won't be called twiced
        isDead = true;

        playerShooting.DisableEffects ();

        // Instruct the animator that player is dead
        anim.SetTrigger ("Die");

        // Play the death clip
        // NOTE1: reason for changing clip is because of replacing the hurt
        //        sound effect
        playerAudio.clip = deathClip;
        playerAudio.Play ();

        // Disable movment script
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }


    public void RestartLevel ()
    {
        Application.LoadLevel (Application.loadedLevel);
    }
}
