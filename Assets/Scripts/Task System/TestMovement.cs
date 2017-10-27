using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour {

    private static readonly IDictionary<KeyCode, Vector3> KEYCODE_TO_DIRECTION = new Dictionary<KeyCode, Vector3>() {
        { KeyCode.W, Vector3.forward },
        { KeyCode.D, Vector3.right },
        { KeyCode.S, Vector3.back },
        { KeyCode.A, Vector3.left }
    };

    [SerializeField]
    private float speedMultiplier;

    // Update is called once per frame
    private void Update() {
        DoMovement(GetDirection());
    }

    private IList<Vector3> GetDirection() {
        List<Vector3> directions = new List<Vector3>();
        foreach (KeyValuePair<KeyCode, Vector3> pair in KEYCODE_TO_DIRECTION) {
            if (Input.GetKey(pair.Key)) {
                directions.Add(pair.Value);
            }
        }
        return directions;
    }

    private void DoMovement(IList<Vector3> directions) {
        foreach (Vector3 direction in directions) {
            transform.position += (direction * speedMultiplier);
        }
    }
}