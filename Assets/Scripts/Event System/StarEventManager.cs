using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StarEventManager : MonoBehaviour {
    private UnityAction<Vector3> starEventListener;

    [SerializeField]
    private AudioClip starGetSound;

    private void Start() {
        this.starEventListener = new UnityAction<Vector3>(StarEventHandler);
    }

    private void OnEnable() {
        EventManager.StartListening<StarGetEvent, Vector3>(starEventListener);
    }

    private void OnDisable() {
        EventManager.StopListening<StarGetEvent, Vector3>(starEventListener);
    }

    private void StarEventHandler(Vector3 position) {
        AudioSource.PlayClipAtPoint(starGetSound, position);
    }
}