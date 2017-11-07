using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Component variables
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    private Collider playerCollider;
    // Input variables
    private float inputH = 0f;
    private float inputV = 0f;
    // Animator variables
    private float speedFwd = 0f;
    private float speedTurn = 0f;
    // Player variables
    private float playerVelX = 0f;
    private float playerVelY = 0f;
    private float playerVelZ = 0f;

    // Public attributes
    public Transform thing;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // GetAxisRaw() so we can do filtering here instead of the InputManager
        //float h = 4f * Input.GetAxis("Mouse X");    // setup h variable as our horizontal input axis
        //float v = 4f * -Input.GetAxis("Mouse Y");	    // setup v variable as our vertical input axis

        //thing.Rotate(v, h, 0);
    }
}
