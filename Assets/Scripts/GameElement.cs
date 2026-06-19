using UnityEngine;

public class GameElement : MonoBehaviour {


    public void SetElement(Element element, int direction, int reach) {
        gameObject.name = element.internalName;

        switch (direction) {
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = element.raySprite;
                transform.localScale = new Vector3(1f, reach, 1f);
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = element.radialSprite;
                transform.localScale = new Vector3(reach, reach, 1f);
                break;
            default:
                transform.localScale = Vector3.one;
                break;
        }

    }
}