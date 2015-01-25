using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    // A reference to player health
    public PlayerHealth playerHealth;

    // How long we will restart the game after player die
    public float restartDelay = 5f;

    // A reference to animation
    Animator anim;

    // A timer count up to restart the level
    float restartTimer;

    void Awake ()
    {
        anim = GetComponent<Animator> ();
    }


    void Update ()
    {
        // Gameover if player health point is zero
        if (playerHealth.currentHealth <= 0) {
            anim.SetTrigger ("GameOver");

            restartTimer += Time.deltaTime;

            if (restartTimer >= restartDelay) {
                Application.LoadLevel (Application.loadedLevel);
            }
        }
    }
}
