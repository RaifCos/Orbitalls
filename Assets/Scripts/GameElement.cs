using UnityEngine;

public class GameElement : MonoBehaviour {

    public void SetElement(Element element, int direction, int reach) {
        gameObject.name = element.internalName;

        switch (direction) {
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = element.raySprite;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                transform.localScale = new Vector3(1f, reach, 1f);
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = element.radialSprite;
                gameObject.GetComponent<CircleCollider2D>().enabled = true;
                transform.localScale = new Vector3(reach * 2f, reach * 2f, 1f);
                break;
            default:
                transform.localScale = Vector3.one;
                break;
        }

    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Planet")) {
            if (collision.gameObject.TryGetComponent<GamePlanet>(out var otherPlanet)) {
                Debug.Log($"Element {gameObject.name} collided with Planet {otherPlanet.name}");
            }
        }
    }
}