using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour {

    [SerializeField]
    private Star uiPrefab;

    [SerializeField]
    private GameObject starHolder;

    [SerializeField]
    private PauseMenuManager pause;

    [SerializeField]
    private Timer timer;

    private IDictionary<int, Star> dict;

    private void Start() {
        this.dict = new Dictionary<int, Star>();

        foreach (CollectibleStar star in FindObjectsOfType<CollectibleStar>()) {
            Star uiStar = Instantiate<Star>(uiPrefab);
            dict.Add(star.gameObject.GetInstanceID(), uiStar);
            uiStar.transform.SetParent(starHolder.transform);
        }
    }

    public void CompleteStar(int instanceId) {
        dict[instanceId].SetColor(false);
        dict.Remove(instanceId);

        if (dict.Count == 0) {
            pause.DoGameOver(timer.GetTimeFormatted());
        }
    }
}