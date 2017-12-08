using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SignUIManager : MonoBehaviour {

    [SerializeField]
    private string readSignButton;

    [SerializeField]
    private GameObject signContext;

    [SerializeField]
    private Text signContextText;

    [SerializeField]
    private PauseMenuManager pause;

    [SerializeField]
    private Text exitInstructions;

    [SerializeField]
    private SignTextDisplay signTextDisplay;

    private string signMessage;

    private void Start() {
        this.exitInstructions.text = string.Format("Press {0} to continue.", readSignButton);
        this.signContextText.text = string.Format("Press {0} to read sign.", readSignButton);
    }

    public void DisplaySignContext(string signMessage) {
        signContext.SetActive(true);
        this.signMessage = signMessage;
    }

    public void HideSignContext() {
        signContext.SetActive(false);
    }

    private void ShowSignText() {
        pause.Pause = PauseMenuManager.PauseType.SIGN;
        pause.TogglePause(() => signTextDisplay.ShowSignDisplay(signMessage));
    }

    private void HideSignText() {
        pause.Pause = PauseMenuManager.PauseType.NOT_PAUSED;
        pause.TogglePause(() => signTextDisplay.HideSignDisplay());
    }

    private void Update() {
        if ((signContext.activeInHierarchy || pause.Pause == PauseMenuManager.PauseType.SIGN) && Input.GetButtonDown(readSignButton)) {
            if (pause.Pause == PauseMenuManager.PauseType.SIGN) { // Unpause
                DisplaySignContext(this.signMessage);
                HideSignText();
            } else if (pause.Pause == PauseMenuManager.PauseType.NOT_PAUSED) { // Pause
                HideSignContext();
                ShowSignText();
            }
        }
    }
}