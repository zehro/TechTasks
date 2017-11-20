using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEffect : MonoBehaviour {

    private enum Direction {
        NEGATIVE,
        POSITIVE
    }

    private const float Z_INTERVAL = 5;
    private const float SECONDS_PER_INTERVAL = 5;

    private Direction current;
    private float currentZ;

    private void Start() {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate() {
        float t = Random.value;
        while (true) {
            if (current == Direction.POSITIVE) {
                t += Time.deltaTime / SECONDS_PER_INTERVAL;
                if (t > 1) {
                    current = Direction.NEGATIVE;
                }
            } else {
                t -= Time.deltaTime / SECONDS_PER_INTERVAL;
                if (t < 0) {
                    current = Direction.POSITIVE;
                }
            }
            currentZ = Mathf.Lerp(-Z_INTERVAL, Z_INTERVAL, t);
            SetZ(currentZ);
            yield return null;
        }
    }

    private void SetZ(float newZ) {
        Vector3 local = transform.localPosition;
        transform.Rotate(new Vector3(0, 0, Mathf.Deg2Rad * newZ));
    }
}