using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Damage amount for every bullet
    public int damagePerShot = 20;

    // How quick for a gun to fire
    public float timeBetweenBullets = 0.15f;

    // Effective range for bullets
    public float range = 100f;

    // A timer determine when to fire
    float timer;

    // A ray from the gun end forwards
    Ray shootRay;

    // A raycast hit to get information about what was hit
    RaycastHit shootHit;

    // A layer mask so the raycast only hit things on the shootable layer
    int shootableMask;

    // A reference to the particle system
    ParticleSystem gunParticles;

    // A reference to the line renderer
    LineRenderer gunLine;

    // A reference to the audio source
    AudioSource gunAudio;

    // A reference to the light component
    Light gunLight;

    // The proportion of the timeBetweenBullets that the effects will display
    // for
    float effectsDisplayTime = 0.2f;

    // Setup reference
    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        // Add the time since Update() was last called to the timer
        timer += Time.deltaTime;

        // If fire1 button is being press and it's time to fire
        if (
            Input.GetButton ("Fire1") &&
            timer >= timeBetweenBullets &&
            Time.timeScale != 0) {
            Shoot ();
        }

        // Always disable effects if timer has exceeded the proportion of
        // timeBetweenBullets that effect should be displayed for
        if (timer >= timeBetweenBullets * effectsDisplayTime) {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        // Reset timer
        timer = 0f;

        // Play gun shot audio clip
        gunAudio.Play ();

        // Turn on the light
        gunLight.enabled = true;

        // Stop particle system and restart it
        gunParticles.Stop ();
        gunParticles.Play ();

        // Show the line renderer
        gunLine.enabled = true;
        // Set one of the end points of this line to be the end of gun barrel
        gunLine.SetPosition (0, transform.position);

        // Define the start point of this shootRay sames with the end of gun
        // barrel
        shootRay.origin = transform.position;
        // Define the direction for this shootRay sames with where the gun
        // barrel pointing
        shootRay.direction = transform.forward;

        // Checkout whether the shootRay hit anything
        if (Physics.Raycast (shootRay, out shootHit, range, shootableMask)) {
            // It hit something
            EnemyHealth enemyHealth =
                shootHit.collider.GetComponent <EnemyHealth> ();

            // Checkout whether it hits enemy or not
            // if yes, deal damage on enemy if the enemy had been shooted
            // if no, do nothing
            if (enemyHealth != null) {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }

            // Setup another end points for our gun line
            // NOTE1: Stop at the GameObject which had been shooted
            gunLine.SetPosition (1, shootHit.point);
        } else {
            // Setup another end points for our gun line
            // NOTE1: Stop at the fullest extent of the gun's range
            gunLine.SetPosition (
                1, shootRay.origin + shootRay.direction * range);
        }
    }
}
