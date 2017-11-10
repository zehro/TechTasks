using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TopmostButtonSelector : MonoBehaviour {

    private enum ControlMode {
        KEYBOARD,
        MOUSE
    }

    private ControlMode currentControl;
    private Vector3 lastMousePosition;

    private bool IsNothingSelected {
        get {
            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
            return (currentSelected == null
                || !currentSelected.activeInHierarchy
                || !currentSelected.GetComponent<Button>());
        }
    }

    private bool IsMouseMoved {
        get {
            return this.lastMousePosition != Input.mousePosition;
        }
    }

    // Update is called once per frame
    private void Update() {
        DetermineLastInput();
        if (currentControl == ControlMode.MOUSE) {
            EventSystem.current.SetSelectedGameObject(null);
        }
        if (currentControl == ControlMode.KEYBOARD
            && IsNothingSelected) {
            Button[] buttons = FindObjectsOfType<Button>();
            Button[] sorted = buttons.OrderByDescending(b => b.transform.position.y).ToArray();
            if (sorted.Length > 0) {
                EventSystem.current.SetSelectedGameObject(sorted[0].gameObject);
            }
        }
        lastMousePosition = Input.mousePosition;
    }

    private void DetermineLastInput() {
        if (Input.anyKeyDown) {
            currentControl = ControlMode.KEYBOARD;
        }
        if (IsMouseMoved) {
            currentControl = ControlMode.MOUSE;
        }
    }
}