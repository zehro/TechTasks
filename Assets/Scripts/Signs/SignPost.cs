using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignPost : MonoBehaviour {

    [SerializeField]
    private string message;

    [SerializeField]
    private SignUIManager manager;

    private void OnTriggerEnter(Collider col) {
        if (col.tag == "Player") {
            manager.DisplaySignContext(message);
        }
    }

    private void OnTriggerExit(Collider col) {
        if (col.tag == "Player") {
            manager.HideSignContext();
        }
    }
}