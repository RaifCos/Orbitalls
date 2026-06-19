using UnityEngine;

public class GameElement : MonoBehaviour {

    private int outputDirection;
    private int outputReach;

    public void SetElement(Element element, int direction, int reach) {
        gameObject.name = element.internalName;
        outputDirection = direction;
        outputReach = reach;
        gameObject.GetComponent<SpriteRenderer>().sprite = direction switch {
            1 => element.raySprite,
            2 => element.radialSprite,
            _ => null,
        };
    }
}
