using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    // A target object which our camera will be followed
    public Transform target;

    // Speed with which the camera will be following
    public float smoothing = 5f;

    // Offset from the target
    //
    // Below is a 2D diagram illustrate what offset is.
    //
    //     C     <--- Camera
    //    /       |
    //   /       <|   offset
    //  /         |
    // P         <--- Player
    //
    Vector3 offset;

    void Start ()
    {
        // Calculate the initial offset from camera to the target
        offset = transform.position - target.position;
    }

    void FixedUpdate ()
    {
        // Create a position the camera is aiming for based on the offset
        // from the target
        // NOTE1: Camera will behave like First persion view if missing offset
        //        in updating camera position.
        Vector3 targetCamPos = target.position + offset;

        // Smoothly interpolate between the camera's current position and it's
        // target position
        transform.position = Vector3.Lerp (
            transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
