using UnityEngine;

public class GameElement : MonoBehaviour {
    private int reach;
    private int planetLayer;
    private Element element;

    [Header("Raycast Detection")]
    private GamePlanet currentHit;

    void Start() => planetLayer = LayerMask.GetMask("Planet");

    public void SetElement(Element e, int r) {
        element = e;
        reach = r;
        gameObject.name = element.internalName;

        gameObject.GetComponent<SpriteRenderer>().sprite = element.raySprite;
        transform.localScale = new Vector3(1f, reach, 1f);
    }
    
    public void Detect() {
        Vector2 origin = transform.parent.position;
        Vector2 rayDir = transform.parent.up;

        float parentRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
        Vector2 offsetOrigin = origin + rayDir * parentRadius;
        float adjustedReach = reach - parentRadius;

        RaycastHit2D hit = Physics2D.Raycast(offsetOrigin, rayDir, adjustedReach, planetLayer);
        Debug.DrawRay(offsetOrigin, rayDir * adjustedReach, hit.collider != null ? Color.red : Color.green);
        GamePlanet hitPlanet = null;

        if (hit.collider != null
            && hit.collider.gameObject != transform.parent.gameObject
            && hit.collider.TryGetComponent<GamePlanet>(out var found)) {
            hitPlanet = found;
        }

        if (hitPlanet != null && hitPlanet != currentHit) ElementHit(hitPlanet);

        currentHit = hitPlanet;
    }

    private void ElementHit(GamePlanet target) {
        Debug.Log($"Element {element.internalName} hit planet {target.name}");
    }
}