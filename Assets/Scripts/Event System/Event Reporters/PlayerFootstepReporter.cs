using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepReporter : MonoBehaviour
{
    // An alternative to curves and animation events that allow more accurate footstep recording
    private Animator playerAnimator;
    private Transform leftFoot;
    private Transform rightFoot;

    private float lastLeftFoot = 0f;
    private float lastRightFoot = 0f;

    public float minFootStepSeparation = 0.3f;

    void Start()
    {
        // Example of how to get access to certain limbs
        leftFoot = this.transform.Find("mixamorig:Hips/mixamorig:LeftUpLeg/mixamorig:LeftLeg/mixamorig:LeftFoot");
        rightFoot = this.transform.Find("mixamorig:Hips/mixamorig:RightUpLeg/mixamorig:RightLeg/mixamorig:RightFoot");

        if (leftFoot == null || rightFoot == null)
        {
            Debug.Log("One of the feet could not be found");
        }
    }

    public void LeftFoot()
    {
        if (Time.timeSinceLevelLoad - lastLeftFoot > minFootStepSeparation)
        {
            // Set last time of event
            lastLeftFoot = Time.timeSinceLevelLoad;
            // Spawn footstep event
            //EventManager.TriggerEvent<EventPlayerFootstep, Vector3, string>(transform.position, other.tag);
        }
    }

    public void RightFoot()
    {
        if (Time.timeSinceLevelLoad - lastLeftFoot > minFootStepSeparation)
        {
            // Set last time of event
            lastLeftFoot = Time.timeSinceLevelLoad;
            // Spawn footstep event
            //EventManager.TriggerEvent<EventPlayerFootstep, Vector3, string>(transform.position, other.tag);
        }
    }

    //public void leftFootStep()
    //{
    //    if (leftFoot == null)
    //    {
    //        return;
    //    }

    //    //see if it's been long enough since the last footstep
    //    if (Time.timeSinceLevelLoad - lastLeftFoot > minFootStepSeparation && Time.timeSinceLevelLoad - lastRightFoot > minFootStepSeparation)
    //    {
    //        lastLeftFoot = Time.timeSinceLevelLoad;

    //        //TODO spawn event for footstep sound
    //        EventManager.TriggerEvent<EventPlayerFootstep, Vector3>(leftFoot.position);
    //    }

    //    //Otherwise, just fall through and ignore the footstep callback that wanted to play
    //}

    //public void rightFootStep()
    //{
    //    if (rightFoot == null)
    //    {
    //        return;
    //    }

    //    //see if it's been long enough since the last footstep
    //    if (Time.timeSinceLevelLoad - lastLeftFoot > minFootStepSeparation && Time.timeSinceLevelLoad - lastRightFoot > minFootStepSeparation)
    //    {
    //        lastRightFoot = Time.timeSinceLevelLoad;

    //        //TODO spawn event for footstep sound
    //        EventManager.TriggerEvent<EventPlayerFootstep, Vector3>(rightFoot.position);
    //    }

    //    //Otherwise, just fall through and ignore the footstep callback that wanted to play
    //}
}
