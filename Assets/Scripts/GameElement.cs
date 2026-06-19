using UnityEngine;

public class GameElement : MonoBehaviour {


    public void SetElement(Element element, int direction, int reach) {
        gameObject.name = element.internalName;
        
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = direction switch {
            1 => element.raySprite,
            2 => element.radialSprite,
            _ => null,
        };

        transform.localScale = direction switch {
            1 => new Vector3(1f, reach, 1f),   
            2 => new Vector3(reach, reach, 1f),
            _ => Vector3.one,
        };
    }
}