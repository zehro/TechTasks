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

    private CollectibleStar bigStar;

    private void Start() {
        this.dict = new Dictionary<int, Star>();

        foreach (CollectibleStar star in FindObjectsOfType<CollectibleStar>()) {
            if (!star.IsBigStar) {
                Star uiStar = Instantiate<Star>(uiPrefab);
                dict.Add(star.gameObject.GetInstanceID(), uiStar);
                uiStar.transform.SetParent(starHolder.transform);
            } else {
                if (bigStar != null) {
                    throw new UnityException("Only one big star is allowed!");
                }
                this.bigStar = star;
            }
        }
        if (bigStar == null) {
            throw new UnityException("No big star in scene!");
        }
    }

    public void CompleteStar(int instanceId) {
        if (bigStar.gameObject.GetInstanceID() == instanceId) {
            pause.DoGameOver(this.timer.GetTimeFormatted());
            return;
        }
        dict[instanceId].SetColor(false);
        dict.Remove(instanceId);

        if (dict.Count == 0) {
            bigStar.ActivateStar();
        }
    }
}