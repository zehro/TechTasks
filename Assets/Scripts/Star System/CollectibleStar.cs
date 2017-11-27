﻿using System.Collections;
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
    private new Collider collider;

    [SerializeField]
    private new MeshRenderer renderer;

    private void OnTriggerEnter(Collider col) {
        if (renderer.enabled && col.gameObject.tag == "Player") {
            Manager.CompleteStar(gameObject.GetInstanceID());
            renderer.enabled = false;
            collider.enabled = false;
        }
    }
}