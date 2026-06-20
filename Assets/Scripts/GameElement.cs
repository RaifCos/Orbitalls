using UnityEngine;

public class GameElement : MonoBehaviour {
    private int reach;
    private int planetLayer;
    private Element element;
    private float parentRadius;
    private GamePlanet currentHit;
    private int missFrameCount = 0;
    private const int MissFrameThreshold = 20;

    void Start() => planetLayer = LayerMask.GetMask("Planet");

    public void SetElement(Element e, int r) {
        element = e;
        reach = r;
        gameObject.name = element.internalName;
        gameObject.GetComponent<SpriteRenderer>().sprite = element.sprite;
        AlignVisualToRay();
        parentRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
    }
        
    private void AlignVisualToRay() {
        float adjustedReach = reach - parentRadius;
        transform.localScale = new Vector3(1f, adjustedReach, 1f);
        transform.localPosition = new Vector3(0f, parentRadius + adjustedReach * 0.5f, 0f);
    }

    public void Detect() {
        Vector2 origin = transform.parent.position;
        Vector2 rayDir = transform.parent.up;

        parentRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
        Vector2 offsetOrigin = origin + rayDir * parentRadius;
        float adjustedReach = reach - parentRadius;

        Vector2 boxSize = new(1f, 0.1f);
        RaycastHit2D hit = Physics2D.BoxCast(offsetOrigin, boxSize, transform.parent.eulerAngles.z, rayDir, adjustedReach, planetLayer);
        
        Vector2 perpendicular = new(-rayDir.y * 0.5f, rayDir.x * 0.5f); // half-width offset
        Vector2 endPoint = offsetOrigin + rayDir * adjustedReach;
        Debug.DrawLine(offsetOrigin - perpendicular, endPoint - perpendicular, hit.collider != null ? Color.red : Color.green);
        Debug.DrawLine(offsetOrigin + perpendicular, endPoint + perpendicular, hit.collider != null ? Color.red : Color.green);

        GamePlanet hitPlanet = null;

        if (hit.collider != null
            && hit.collider.gameObject != transform.parent.gameObject
            && hit.collider.TryGetComponent<GamePlanet>(out var found)) {
            hitPlanet = found;
        }

        if (hitPlanet != null) {
            missFrameCount = 0; // reset miss counter whenever we hit something
            if (hitPlanet != currentHit) {
                ElementHit(hitPlanet);
                currentHit = hitPlanet;
            }
        } else {
            missFrameCount++;
            if (missFrameCount >= MissFrameThreshold && currentHit != null) {
                ElementExited(currentHit);
                currentHit = null;
                missFrameCount = 0;
            }
        }
    }

    private void ElementExited(GamePlanet target) {
        Debug.Log($"Element {element.internalName} left planet {target.name}");
    }

    private void ElementHit(GamePlanet target) {
        Debug.Log($"Element {element.internalName} hit planet {target.name}");
    }
}