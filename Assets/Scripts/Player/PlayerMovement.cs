using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Speed we will apply to the charactor
    public float speed = 6f;

    // Vector stores the direction of the player's movement
    Vector3 movement;

    // Reference to the animator component
    Animator anim;

    // Reference to the player rigibody
    Rigidbody playerRigidBody;

    // A layer mask so that a ray can be cast just at gameobjects on the floor
    // layers
    int floorMask;

    // The length of the ray from the camera into the scene
    float camRayLength = 100f;

    // Setup reference
    void Awake ()
    {
        // Create a layer mask for the floor layer
        floorMask = LayerMask.GetMask ("Floor");

        // Setup references
        anim = GetComponent<Animator> ();
        playerRigidBody = GetComponent<Rigidbody> ();
    }

    // A function will be called by Unity which update physics
    void FixedUpdate ()
    {
        // Store the input axes
        float h = Input.GetAxisRaw ("Horizontal");
        float v = Input.GetAxisRaw ("Vertical");

        // Move the player around the scene
        Move (h, v);

        // Turn the player to face the mouse cursor
        Turning ();

        // Animate the player
        Animating (h, v);
    }

    // Move a character
    // 
    // @param h float
    //   Horizontal value
    // @param v float
    //   Vertical value
    void Move (float h, float v)
    {
        // Set the movement vector based on the axis input
        // 0f means fixing on grond
        movement.Set (h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed
        // per second
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement
        playerRigidBody.MovePosition (transform.position + movement);
    }

    // Turn player to face the mouse cursor
    void Turning ()
    {
        // Create a ray from the mouse cursor on screen in the direction of the
        // camera
        Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit
        // by the ray
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer
        if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
            // Create a vector from the player to the point on the floor the
            // raycast from the mouse hit
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector
            // from the player to the mouse
            Quaternion newRotation = Quaternion.LookRotation (playerToMouse);

            // Set the player's rotation to this new rotation
            playerRigidBody.MoveRotation (newRotation);
        }
    }

    // Animate the player. For example, walking.
    //
    // @param h float
    // @param v float
    void Animating (float h, float v)
    {
        // Create a boolean that is true if either of the input axes is
        // non-zero
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking
        anim.SetBool ("IsWalking", walking);
    }
}
