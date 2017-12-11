using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootManager : MonoBehaviour {

    [SerializeField]
    private Transform playerBase;

    [SerializeField]
    private Rigidbody body;

    [SerializeField]
    private ParticleSystem dustPrefab;

    [SerializeField]
    private float epsilon = 0.05f;

    [SerializeField]
    private float dustTrailCooldown = 0.10f;

    [SerializeField]
    private float maxDustTrails = 10;

    private float dustTrailTimer;

    private bool isContactingGround;

    private Queue<ParticleSystem> dustTrails;

    private Vector3 lastPosition;

    private void Start() {
        dustTrails = new Queue<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision col) {
        if (col.gameObject.layer == 9) {
            isContactingGround = true;
        }
    }

    private void OnCollisionExit(Collision col) {
        if (col.gameObject.layer == 9) {
            isContactingGround = false;
        }
    }

    private void Update() {
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        if (isContactingGround
            && velocity.magnitude > epsilon
            && (dustTrailTimer += Time.deltaTime) > dustTrailCooldown) {
            Debug.Log(velocity.magnitude);
            StartCoroutine(AddDustTrail());
        }
        lastPosition = transform.position;
    }

    private IEnumerator AddDustTrail() {
        ParticleSystem ps = Instantiate<ParticleSystem>(dustPrefab);
        dustTrails.Enqueue(ps);
        while (dustTrails.Count > maxDustTrails) {
            Destroy(dustTrails.Dequeue());
        }
        ps.gameObject.transform.position = playerBase.position;
        ps.Play();
        yield return new WaitWhile(() => ps.isPlaying);
        Destroy(ps);
    }
}