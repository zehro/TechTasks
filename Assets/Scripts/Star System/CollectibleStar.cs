using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleStar : MonoBehaviour {
    private static StarManager _manager;

    private static StarManager Manager {
        get {
            if (_manager == null) {
                _manager = FindObjectOfType<StarManager>();
            }
            return _manager;
        }
    }

    [SerializeField]
    private float rotationSpeed = 10;

    [SerializeField]
    private new Collider collider;

    [SerializeField]
    private new MeshRenderer renderer;

    [SerializeField]
    private bool isBigStar;

    public bool IsBigStar {
        get {
            return isBigStar;
        }
    }

    public void ActivateStar() {
        if (!isBigStar) {
            throw new UnityException("Only big stars can be activated!");
        }
        renderer.enabled = true;
        collider.enabled = true;
    }

    private void Start() {
        if (isBigStar) {
            renderer.enabled = false;
            collider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (renderer.enabled && col.gameObject.tag == "Player") {
            Manager.CompleteStar(gameObject.GetInstanceID());
            renderer.enabled = false;
            collider.enabled = false;
        }
    }

    private void Update() {
        this.transform.Rotate(Vector3.up, rotationSpeed);
    }
}