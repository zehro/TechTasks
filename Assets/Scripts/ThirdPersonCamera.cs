using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public Transform target;
    public Vector3 offset;
    public float pitchSpeed = 1f;
    public float yawSpeed = 1f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float h = Input.GetAxis("Mouse X") * pitchSpeed;
        float v = Input.GetAxis("Mouse Y") * yawSpeed;

        // Rotates the camera proxy
        target.Rotate(v, -h, 0);

        // Lock "roll"/z-axis of camera proxy
        target.rotation = Quaternion.Euler(
            target.rotation.eulerAngles.x,
            target.rotation.eulerAngles.y,
            0
        );

        // Rotate camera according to mouse inputs
        transform.Rotate(-v, h, 0);

        // Lock "roll"/z-axis of camera
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            0
        );

        // Get the offset relative to the target pivot
        Vector3 relativeVector = transform.TransformDirection(offset);

        // Update position around the target + offset
        transform.position = target.position + relativeVector;
    }

    void OnMouseDown()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
