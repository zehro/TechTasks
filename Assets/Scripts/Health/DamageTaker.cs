using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour {

    [SerializeField]
    private HealthManager health;

    [SerializeField]
    private float invincibleDuration = 2;

    [SerializeField]
    private float flickerRate = 0.10f;

    [SerializeField]
    private GameObject model;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private float minPitch = 0.8f;

    [SerializeField]
    private float maxPitch = 1.2f;

    [SerializeField]
    private float knockbackFromHit;

    [SerializeField]
    private Rigidbody body;

    private SkinnedMeshRenderer[] renderers;

    private Coroutine routine;

    private void Start() {
        this.renderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void Hurt(Vector3 position) {
        if (routine == null) {
            routine = StartCoroutine(TakeDamage(position));
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy") {
            Hurt(collision.transform.position);
        }
    }

    private IEnumerator TakeDamage(Vector3 enemyPosition) {
        Vector3 knockbackDirection = (enemyPosition - this.transform.position).normalized;
        knockbackDirection *= knockbackFromHit;
        body.AddForce(knockbackDirection, ForceMode.Force);

        float invincible = 0;
        health.LoseHealth();
        source.pitch = Random.Range(minPitch, maxPitch);
        source.Play();
        while ((invincible += flickerRate) < invincibleDuration) {
            SetRenderers(false);
            yield return new WaitForSeconds(flickerRate);
            SetRenderers(true);
            yield return null;
        }
        SetRenderers(true);
        routine = null;
    }

    private void SetRenderers(bool isActive) {
        foreach (SkinnedMeshRenderer skin in renderers) {
            skin.enabled = isActive;
        }
    }
}