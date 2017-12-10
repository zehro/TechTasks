using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Component variables
    private Animator playerAnimator;
    private Rigidbody playerRigidbody;
    
    // Input variables
    private Vector3 input;
    private float inputH = 0f;
    private float inputV = 0f;

    // Animator variables
    private float velocityZ = 0f;
    private float velocityX = 0f;

    // Motion variables
    public float fwdInterpolation = 0.05f;
    public float turnInterpolation = 0.05f;
    public float speedScale = 2f;
    public float gravityScale = 3f;
    public float slopeAngleDiff = 50f;
    private Vector3 relativeVector;
    private Vector3 animRootDifference;
    private Vector3 lastAnimVelocity;
    private Vector3 normalVector;

    // Falling variables
    private bool isGrounded
    {
        get { return groundContacts > 0; }
    }
    public int groundContacts = 0;
    private float rayOriginOffset = 0.5f;
    private float rayDepth = 1f;
    private RaycastHit hit;
    private int layerMask = (1 << 9); //~(1 << 8);

    //// Sliding variables
    //private bool isSliding
    //{
    //    get { return Vector3.Angle(Vector3.up, normalVector) > slopeAngleDiff; }
    //}

    // Jumping variables
    public bool jumpButtonPressed
    {
        get { return Input.GetKey(KeyCode.Space); }
    }
    public float jumpTimeInterval = 1f;
    public float jumpForce = 1000f;

    // Public attributes
    public Transform cameraProxy;
    public string[] groundTags;

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

        // Get the relative vector
        relativeVector = cameraProxy.InverseTransformDirection(input);

        // Pass in the raw input values into the animator
        playerAnimator.SetFloat("Input: Left-Right", relativeVector.x);
        playerAnimator.SetFloat("Input: Fwd-Bwd", relativeVector.z);

        velocityX = Mathf.Lerp(velocityX, relativeVector.x, turnInterpolation);
        velocityZ = Mathf.Lerp(velocityZ, Mathf.Abs(relativeVector.z), fwdInterpolation);

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
        playerAnimator.SetFloat("VelocityZ", velocityZ);    // set our animator's float parameter 'Speed' equal to the vertical input axis
        playerAnimator.SetFloat("VelocityX", velocityX);    // set our animator's float parameter 'Direction' equal to the horizontal input axis

        // Handle falling
        // Set our animator's float parameter 'Vertical Velocity' equal to the rigidbody's current y velocity
        float velocityY = playerRigidbody.velocity.y;
        if (velocityY > -0.01 && velocityY < 0.01)
        {
            playerAnimator.SetFloat("VelocityY", 0);
        }
        else
        {
            playerAnimator.SetFloat("VelocityY", velocityY);
        }

        // Handle sliding & falling
        if (!isGrounded)
        {
            // Toggles the fall trigger, then disables future triggers
            if (playerAnimator.GetBool("isFalling") == false)
            {
                playerAnimator.SetTrigger("Fall");
                playerAnimator.SetBool("isFalling", true);
            }
        }

        Ray ray = new Ray(this.transform.position + Vector3.up * rayOriginOffset, Vector3.down);

        //Cast ray and look for ground
        if (Physics.Raycast(ray, out hit, rayOriginOffset + rayDepth + (Mathf.Pow(-velocityY, 2) * Time.deltaTime), layerMask))
        {
            normalVector = hit.normal;

            if (!isGrounded)
            {
                // Check to see if raycast hits the ground
                if (IsGround(hit.collider.gameObject))
                {
                    // Turning falling back off because we are close to the ground
                    playerAnimator.SetBool("isFalling", false);
                }
            }

            //if (isSliding)
            //{
            //    if (playerAnimator.GetBool("isSliding") == false)
            //    {
            //        playerAnimator.SetTrigger("Slide");
            //        playerAnimator.SetBool("isSliding", true);
            //    }
            //}
            //else
            //{
            //    playerAnimator.SetBool("isSliding", false);
            //}
        }

        playerRigidbody.angularVelocity = Vector3.zero;
    }

    void Update()
    {
        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            StartCoroutine(MakeJump());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isGrounded && collision.impulse.magnitude > 0.1f)
        {
            lastAnimVelocity = (collision.impulse.normalized) + (Vector3.down * 2f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsGround(other.transform.gameObject))
        {
            ++groundContacts;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsGround(other.transform.gameObject))
        {
            --groundContacts;
        }
    }

    // Called when the animator moves
    void OnAnimatorMove()
    {
        if (!isGrounded)
        {
            // Moves the character slightly with input (for falling, jumping)
            playerRigidbody.velocity = lastAnimVelocity;
            lastAnimVelocity += (Vector3.down * gravityScale) * Time.deltaTime;
        }
        else
        {
            // Moves the character with some enhancements to root motion
            animRootDifference = playerAnimator.rootPosition - transform.position;
            transform.position = new Vector3(
                                    transform.position.x + (animRootDifference.x * speedScale),
                                    playerAnimator.rootPosition.y,
                                    transform.position.z + (animRootDifference.z * speedScale));

            transform.rotation = playerAnimator.rootRotation;

            // Keep track of the current animator velocity
            lastAnimVelocity = playerAnimator.velocity * speedScale;
        }
    }

    public bool IsGround(GameObject collidedObject)
    {
        foreach (string tag in groundTags)
        {
            if (collidedObject.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator MakeJump()
    {
        playerAnimator.SetTrigger("Jump");
        //playerAnimator.SetBool("isFalling", true);
        float timer = 1.0f;

        while (jumpButtonPressed && timer < jumpTimeInterval)
        {
            // Calculate how far through the jump we are as a percentage
            // apply the full jump force on the first frame, then apply less force
            // each consecutive frame
            float proportionCompleted = timer / jumpTimeInterval;
            Vector3 thisFrameJumpVector = Vector3.Lerp(Vector3.zero, Vector3.up * jumpForce, proportionCompleted);
            playerRigidbody.AddForce(thisFrameJumpVector);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
