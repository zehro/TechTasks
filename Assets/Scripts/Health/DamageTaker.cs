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

    private SkinnedMeshRenderer[] renderers;

    private Coroutine routine;

    private void Start() {
        this.renderers = model.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Enemy" && routine == null) {
            routine = StartCoroutine(TakeDamage());
        }
    }

    private IEnumerator TakeDamage() {
        float invincible = 0;
        health.LoseHealth();
        while ((invincible += flickerRate) < invincibleDuration) {
            SetRenderers(false);
            yield return new WaitForSeconds(flickerRate);
            SetRenderers(true);
            yield return null;
        }
        model.SetActive(true);
        routine = null;
    }

    private void SetRenderers(bool isActive) {
        foreach (SkinnedMeshRenderer skin in renderers) {
            skin.enabled = isActive;
        }
    }
}