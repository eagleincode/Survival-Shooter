using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Initial health point for enemys
    public int startingHealth = 100;

    // Current health point for this enemy
    public int currentHealth;

    // How fast for the corpse to sink inside the floor
    public float sinkSpeed = 2.5f;

    // Score for killing this enemy
    public int scoreValue = 10;

    // A reference to the death clip
    public AudioClip deathClip;

    // A reference to animator
    Animator anim;

    // A reference to audio source
    // By default, defined in unity IDE, it is a hurt sound
    AudioSource enemyAudio;

    // A reference to particle system
    ParticleSystem hitParticles;

    // A reference to the enemy's capsule collider
    CapsuleCollider capsuleCollider;

    // A flag indicate this enemy is dead or not
    bool isDead;

    // A flag indicate whether this corpse has started sinking through the
    // floor or not
    bool isSinking;

    // Setup references
    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        // Set current health to be starting health on spwan
        currentHealth = startingHealth;
    }


    void Update ()
    {
        // How fast will this corpse to be sinked (disappered)
        if (isSinking) {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    // Handler for receiving damage from player
    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        // Do nothing if this is a corpse
        if (isDead)
            return;

        // Play hurt sound
        enemyAudio.Play ();

        // Deduct the health point
        currentHealth -= amount;

        // Play HitParticles effect in a position where it got hurt
        hitParticles.transform.position = hitPoint;
        hitParticles.Play ();

        // Condition for this enemy to die
        if (currentHealth <= 0) {
            // Call Death() to handle how this enemy die
            Death ();
        }
    }


    void Death ()
    {
        // Set flag *isDead* to tell others this enemy die
        isDead = true;

        // Allows player to walk through the corpse
        capsuleCollider.isTrigger = true;

        // Ask animator to play dead clip
        anim.SetTrigger ("Dead");

        // Replace the hurt clip with dead clip and play it
        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }

    // Called by Animator
    public void StartSinking ()
    {
        // Turn off this one component only instead of all instances of this
        // components
        GetComponent <NavMeshAgent> ().enabled = false;

        // Find the rigidbody component and make it kinematic
        GetComponent <Rigidbody> ().isKinematic = true;

        // Set flag *isSinking* true
        isSinking = true;
        //ScoreManager.score += scoreValue;

        // Destroy this instance after it dies for 2 seconds
        Destroy (gameObject, 2f);
    }
}
