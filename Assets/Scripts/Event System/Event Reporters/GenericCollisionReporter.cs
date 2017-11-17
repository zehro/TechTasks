using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCollisionReporter : MonoBehaviour
{
    void OnCollisionEnter(Collision c)
    {
        if (c.impulse.magnitude > 0.5f)
        {
            // we'll just use the first contact point for simplicity
            //EventManager.TriggerEvent<EventCollisionGeneric, Vector3, GameObject, Collision>(c.contacts[0].point, this.gameObject, c);
        }

        // foreach (ContactPoint contact in c.contacts)
        // {
        //    if (c.impulse.magnitude > 0.5f)
        //    {
        //        EventManager.TriggerEvent<AudioEventManager.BoxAudioEvent, Vector3>(contact.point);
        //    }
        // }
    }
}
