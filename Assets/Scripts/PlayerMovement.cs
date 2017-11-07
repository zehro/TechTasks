using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // Component variables
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    private Collider playerCollider;
    // Input variables
    private Vector3 input;
    private float inputH = 0f;
    private float inputV = 0f;
    // Animator variables
    private float velocityZ = 0f;
    private float velocityX = 0f;

    // Public attributes
    public float fwdInterpolation = 0.05f;
    public float turnInterpolation = 0.05f;

    private void Awake()
    {
        // Throw errors into the console log for missing components
        playerAnimator = GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.Log("Animator could not be found");
        }

        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.Log("Rigid body could not be found");
        }

        playerCollider = GetComponentInChildren<Collider>();
        if (playerCollider == null)
        {
            Debug.Log("Collider could not be found");
        }
    }

    // Use this for initialization
    void Start ()
    {
    }

    // Update whenever physics updates with FixedUpdate()
    // The animator update mode should coincide with "Animate Physics" in the Inspector
    void FixedUpdate()
    {
        // GetAxisRaw() so we can do filtering here instead of the InputManager
        inputH = Input.GetAxisRaw("Horizontal");    // setup h variable as our horizontal input axis
        inputV = Input.GetAxisRaw("Vertical");	    // setup v variable as our vertical input axis

        // Enforce circular joystick mapping which should coincide with circular blendtree positions
        input = Vector3.ClampMagnitude(new Vector3(inputH, 0, inputV), 1.0f);

        // Get normalized input values
        inputH = input.x;
        inputV = input.z;
        // Pass in the raw input values into the animator
        playerAnimator.SetFloat("Input: Left-Right", inputH);
        playerAnimator.SetFloat("Input: Fwd-Bwd", inputV);

        //Debug.Log(transform.forward);

        //// BEGIN ANALOG ON KEYBOARD DEMO CODE
        //if (Input.GetKey(KeyCode.Q))
        //    h = -0.5f;
        //else if (Input.GetKey(KeyCode.E))
        //    h = 0.5f;

        //if (Input.GetKeyUp(KeyCode.Alpha1))
        //    forwardSpeedLimit = 0.1f;
        //else if (Input.GetKeyUp(KeyCode.Alpha2))
        //    forwardSpeedLimit = 0.2f;
        //else if (Input.GetKeyUp(KeyCode.Alpha3))
        //    forwardSpeedLimit = 0.3f;
        //else if (Input.GetKeyUp(KeyCode.Alpha4))
        //    forwardSpeedLimit = 0.4f;
        //else if (Input.GetKeyUp(KeyCode.Alpha5))
        //    forwardSpeedLimit = 0.5f;
        //else if (Input.GetKeyUp(KeyCode.Alpha6))
        //    forwardSpeedLimit = 0.6f;
        //else if (Input.GetKeyUp(KeyCode.Alpha7))
        //    forwardSpeedLimit = 0.7f;
        //else if (Input.GetKeyUp(KeyCode.Alpha8))
        //    forwardSpeedLimit = 0.8f;
        //else if (Input.GetKeyUp(KeyCode.Alpha9))
        //    forwardSpeedLimit = 0.9f;
        //else if (Input.GetKeyUp(KeyCode.Alpha0))
        //    forwardSpeedLimit = 1.0f;
        //// END ANALOG ON KEYBOARD DEMO CODE  

        //// Do some filtering of our input as well as clamp to a speed limit
        //filteredForwardInput = Mathf.Clamp(Mathf.Lerp(filteredForwardInput, v, Time.deltaTime * forwardInputFilter), -forwardSpeedLimit, forwardSpeedLimit);
        //filteredTurnInput = Mathf.Lerp(filteredTurnInput, h, Time.deltaTime * turnInputFilter);

        Vector3 relativeVector = transform.InverseTransformDirection(input);

        velocityZ = Mathf.Lerp(velocityZ, relativeVector.z, fwdInterpolation);
        velocityX = Mathf.Lerp(velocityX, relativeVector.x, turnInterpolation);
        //float angle = Vector3.Angle(input, transform.forward);

        // Filter out the weird small double values that Lerp returns
        if (velocityZ > -0.01 && velocityZ < 0.01)
        {
            velocityZ = 0;
        }
        if (velocityX > -0.01 && velocityX < 0.01)
        {
            velocityX = 0;
        }

        // Finally pass the processed input values to the animator
        playerAnimator.SetFloat("VelocityZ", velocityZ);	        // set our animator's float parameter 'Direction' equal to the horizontal input axis
        playerAnimator.SetFloat("VelocityX", velocityX);    // set our animator's float parameter 'Speed' equal to the vertical input axis

        

        //// Handle falling
        //bool isFalling = !isGrounded;
        //float verticalVel = playerRigidbody.velocity.y;

        //if (isFalling)
        //{
        //    // Toggles the fall trigger, then disables future triggers
        //    if (fallTriggered == false)
        //    {
        //        animator.SetTrigger("fall");
        //        fallTriggered = true;
        //    }

        //    const float rayOriginOffset = 1f; // the origin is near bottom of the collider, so need a fudge factor up away from there
        //    float rayDepth = 2 + (Mathf.Pow(-verticalVel, 2) * Time.deltaTime); //how far down will we look for ground?
        //    float totalRayLen = rayOriginOffset + rayDepth;

        //    Ray ray = new Ray(this.transform.position + Vector3.up * rayOriginOffset, Vector3.down);

        //    RaycastHit hit;

        //    //Cast ray and look for ground. If ground is close, then transition out of falling animation
        //    if (Physics.Raycast(ray, out hit, totalRayLen))
        //    {
        //        if (CheckGround(hit.collider.gameObject))
        //        {
        //            // Turning falling back off because we are close to the ground
        //            isFalling = false;

        //            // Re-enables the fall trigger
        //            fallTriggered = false;
        //        }
        //    }
        //}

        //animator.SetBool("isFalling", isFalling);
        //// Set our animator's float parameter 'Vertical Velocity' equal to the rigidbody's current y velocity
        //animator.SetFloat("velY", verticalVel);
    }

    // Update is called once per frame
    void Update ()
    {
    }

    // Called when the animator moves
    void OnAnimatorMove()
    {
        transform.rotation = playerAnimator.rootRotation;

        transform.position = playerAnimator.rootPosition;

        //if (!animator.GetBool("isJumping"))
        //{
        //    if (isGrounded)
        //    {
        //        // Use root motion as is if on the ground		
        //        this.transform.position = animator.rootPosition;
        //    }
        //    else
        //    {
        //        // Simple trick to keep model from climbing other rigidbodies that aren't the ground
        //        this.transform.position = new Vector3(animator.rootPosition.x, this.transform.position.y, animator.rootPosition.z);
        //    }

        //    // Use rotational root motion as is
        //    this.transform.rotation = animator.rootRotation;
        //}
    }
}
