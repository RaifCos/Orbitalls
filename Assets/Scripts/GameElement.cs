using UnityEngine;

public class GameElement : MonoBehaviour {
    private int reach;
    private int planetLayer;
    private Element element;
    private float parentRadius;
    private GamePlanet currentHit;
    private int missFrameCount = 0;
    private const int MissFrameThreshold = 20;

    public void SetElement(Element e, int r) {
        element = e;
        reach = r;
        planetLayer = LayerMask.GetMask("Planet");
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

    private void Update() { Detect(); }

    public void Detect() {
        Vector2 origin = transform.parent.position;
        Vector2 rayDir = transform.parent.up;

        parentRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
        Vector2 offsetOrigin = origin + rayDir * parentRadius;
        float adjustedReach = reach - parentRadius;

        Vector2 perpendicular = new(-rayDir.y, rayDir.x);
        Vector2 left  = offsetOrigin - perpendicular * 0.4f;
        Vector2 right = offsetOrigin + perpendicular * 0.4f;

        RaycastHit2D hitCenter = Physics2D.Raycast(offsetOrigin, rayDir, adjustedReach, planetLayer);
        RaycastHit2D hitLeft   = Physics2D.Raycast(left,         rayDir, adjustedReach, planetLayer);
        RaycastHit2D hitRight  = Physics2D.Raycast(right,        rayDir, adjustedReach, planetLayer);

        Debug.DrawRay(left,         rayDir * adjustedReach, hitLeft.collider   != null ? Color.red : Color.green);
        Debug.DrawRay(offsetOrigin, rayDir * adjustedReach, hitCenter.collider != null ? Color.red : Color.green);
        Debug.DrawRay(right,        rayDir * adjustedReach, hitRight.collider  != null ? Color.red : Color.green);

        RaycastHit2D hit = hitCenter.collider != null ? hitCenter : hitLeft.collider != null ? hitLeft : hitRight;

        GamePlanet hitPlanet = null;
        if (hit.collider != null
            && hit.collider.gameObject != transform.parent.gameObject
            && hit.collider.TryGetComponent<GamePlanet>(out var found)) {
            hitPlanet = found;
        }

        if (hitPlanet != null) {
            missFrameCount = 0;
            if (hitPlanet != currentHit) {
                ElementHit(hitPlanet);
                currentHit = hitPlanet;
            }
        } else {
            missFrameCount++;
            if (missFrameCount >= MissFrameThreshold && currentHit != null) {
                ElementExit(currentHit);
                currentHit = null;
                missFrameCount = 0;
            }
        }
    }

    private void ElementHit(GamePlanet target) {
        Debug.Log($"Element {element.internalName} hit planet {target.name}");
        target.UpdatePlanetTraits(element.heatEffect, element.humidityEffect, element.atmosphereEffect);
    }

        private void ElementExit(GamePlanet target) {
        Debug.Log($"Element {element.internalName} left planet {target.name}");
        target.UpdatePlanetTraits(-element.heatEffect, -element.humidityEffect, -element.atmosphereEffect);
    }
}