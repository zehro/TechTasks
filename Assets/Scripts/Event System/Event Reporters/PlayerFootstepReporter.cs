using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepReporter : MonoBehaviour
{
    // An alternative to curves and animation events that allow more accurate footstep recording
    private Animator playerAnimator;
    private float lastLeftFoot = 0f;
    private float lastRightFoot = 0f;

    public float minFootStepSeparation = 0.3f;
    public GameObject parent;

    private void Awake()
    {
        // Throw errors into the console log for missing components
        playerAnimator = parent.GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.Log("Animator could not be found");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (this.name.Equals("LeftFoot"))
        {
            if (Time.timeSinceLevelLoad - lastLeftFoot > minFootStepSeparation)
            {
                // Set last time of event
                lastLeftFoot = Time.timeSinceLevelLoad;
                // Spawn footstep event
                EventManager.TriggerEvent<EventPlayerFootstep, Vector3, string>(transform.position, other.tag);
                // Set foot bool in animator
                playerAnimator.SetBool("isLeftFootAvailable", false);
            }
        }
        else if (this.name.Equals("RightFoot"))
        {
            if (Time.timeSinceLevelLoad - lastRightFoot > minFootStepSeparation)
            {
                // Set last time of event
                lastRightFoot = Time.timeSinceLevelLoad;
                // Spawn footstep event
                EventManager.TriggerEvent<EventPlayerFootstep, Vector3, string>(transform.position, other.tag);
                // Set foot bool in animator
                playerAnimator.SetBool("isRightFootAvailable", false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (this.name.Equals("LeftFoot") && !playerAnimator.GetBool("isLeftFootAvailable"))
        {
            playerAnimator.SetBool("isLeftFootAvailable", true);
        }
        else if (this.name.Equals("RightFoot") && !playerAnimator.GetBool("isRightFootAvailable"))
        {
            playerAnimator.SetBool("isRightFootAvailable", true);
        }
    }
}
