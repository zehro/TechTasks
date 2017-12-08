using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour {

    [SerializeField]
    private Image image;

    private Color Color {
        set {
            this.image.color = value;
        }
    }

    private CollectibleStar collectible;

    public CollectibleStar Collectible {
        get {
            return collectible;
        }
    }

    public void SetCollectible(CollectibleStar star) {
        this.collectible = star;
    }

    public void SetColor(bool isBlack) {
        Color = isBlack ? Color.black : Color.white;
    }
}