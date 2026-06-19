using UnityEngine;
using System.Collections.Generic;

public class GameElement : MonoBehaviour {

    private int direction;
    private int reach;
    private Element element;

    [Header("Raycast Detection")]
    private GamePlanet currenRaytHit;
    private List<GamePlanet> currentRadialHits = new();


    public void SetElement(Element e, int d, int r) {
        element = e;
        direction = d;
        reach = r;
        gameObject.name = element.internalName;

        switch (direction) {
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = element.raySprite;
                transform.localScale = new Vector3(1f, reach, 1f);
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = element.radialSprite;
                transform.localScale = new Vector3(reach * 2f, reach * 2f, 1f);
                break;
            default:
                transform.localScale = Vector3.one;
                break;
        }
    }

    public void Detect() {
        int planetLayer = LayerMask.GetMask("Planet");
        switch (direction) {
            case 1:
                RayDetect(planetLayer);
                break;
            case 2:
                RadialDetect(planetLayer);
                break;
        }
    }

    
    private void RayDetect(int planetLayer) {
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

        if (hitPlanet != null && hitPlanet != currenRaytHit) ElementHit(hitPlanet);

        currenRaytHit = hitPlanet;
    }


    private void RadialDetect(int planetLayer) {
        Vector2 origin = transform.parent.position;
        float parentRadius = transform.parent.GetComponent<CircleCollider2D>().radius;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, reach, planetLayer);

        List<GamePlanet> frameHits = new();
        foreach (var collider in colliders) {
            if (collider.gameObject == transform.parent.gameObject) continue;
            if (!collider.TryGetComponent<GamePlanet>(out var hitPlanet)) continue;

            Vector2 targetPos = collider.transform.position;
            Vector2 toTarget = targetPos - origin;
            Vector2 dir = toTarget.normalized;
            float distance = toTarget.magnitude;

            Vector2 offsetOrigin = origin + dir * parentRadius;
            float adjustedDistance = distance - parentRadius;

            RaycastHit2D hit = Physics2D.Raycast(offsetOrigin, dir, adjustedDistance, planetLayer);
            Debug.DrawRay(offsetOrigin, dir * adjustedDistance, hit.collider != null && hit.collider.gameObject == collider.gameObject ? Color.red : Color.yellow);

            if (hit.collider != null && hit.collider.gameObject == collider.gameObject) frameHits.Add(hitPlanet);
        }

        foreach (var planet in frameHits) {
            if (!currentRadialHits.Contains(planet)) ElementHit(planet);
        }

        currentRadialHits = frameHits;
    }

    private void ElementHit(GamePlanet target) {
        Debug.Log($"Element {element.internalName} hit planet {target.name}");
        //TODO: Add Logic for when an element hits a planet (planet state should change)
    }
}