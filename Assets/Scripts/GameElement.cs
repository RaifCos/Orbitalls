using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteAnimation))]
public class GameElement : MonoBehaviour {

    private Element element;
    private int reach;
    private LayerMask planetMask;
    private readonly HashSet<GamePlanet> candidates = new();
    private readonly HashSet<GamePlanet> activeTargets = new();

    private float parentRadius;
    private float fullHeight;
    private float width;
    private SpriteRenderer sr;
    private BoxCollider2D col;

    private void FixedUpdate() {
        foreach (var planet in candidates) { ValidateView(planet); }
        if (sr != null) UpdateVisualClip();
    }

    public void SetElement(Element e, int r) {
        element = e;
        reach = r;
        planetMask = LayerMask.GetMask("Planet");
        gameObject.name = element.internalName;

        parentRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
        fullHeight = reach - parentRadius;
        width = 0.8f;

        transform.localScale    = Vector3.one;
        transform.localPosition = new Vector3(0f, parentRadius + fullHeight * 0.5f, 0f);

        GetComponent<SpriteAnimation>().UpdateSpriteList(element.sprites);

        sr = GetComponent<SpriteRenderer>();
        sr.drawMode = SpriteDrawMode.Tiled;
        sr.size = new Vector2(width, fullHeight);

        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.offset = Vector2.zero;
        col.size = new Vector2(width, fullHeight);

        Debug.Log($"SR size: {sr.size}, Col size: {col.size}, Position: {transform.localPosition}");
    }

    // Clips the sprite to stop at the nearest planet blocker (or closest active target surface)
    private void UpdateVisualClip() {
        Vector2 origin = transform.parent.position;
        Vector2 dir = ((Vector2)transform.position - (Vector2)transform.parent.position).normalized;
        
        // Find the nearest hit along the emission direction within reach
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, reach, planetMask);

        float nearestStop = reach; // default: draw to full reach

        foreach (var hit in hits) {
            if (hit.collider.gameObject == transform.parent.gameObject) continue;

            float hitDist = hit.distance;

            // If this planet is an active target, stop at its near surface
            if (hit.collider.TryGetComponent<GamePlanet>(out var gp) && activeTargets.Contains(gp)) {
                nearestStop = Mathf.Min(nearestStop, hitDist);
                break; // active target is our endpoint, no need to look further
            }

            // Otherwise it's a blocker — clip just before it
            nearestStop = Mathf.Min(nearestStop, hitDist);
            break;
        }

        // Convert world distance → local height (subtract parentRadius since sprite starts at surface)
        float clippedHeight = Mathf.Max(0f, nearestStop - parentRadius);

        // Reposition sprite so its base stays at the planet surface
        transform.localPosition = new Vector3(0f, parentRadius + clippedHeight * 0.5f, 0f);
        sr.size = new Vector2(width, clippedHeight);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == transform.parent.gameObject) return;
        if (other.TryGetComponent<GamePlanet>(out var planet)) { candidates.Add(planet); }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.TryGetComponent<GamePlanet>(out var planet)) {
            candidates.Remove(planet);
            SetActive(planet, false);
        }
    }

    private void ValidateView(GamePlanet planet) {
        Vector2 origin = transform.parent.position;
        Vector2 target = planet.transform.position;
        Vector2 dir    = (target - origin).normalized;
        float   dist   = Vector2.Distance(origin, target);

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, dist, planetMask);

        bool blocked = false;
        foreach (var hit in hits) {
            if (hit.collider.gameObject == transform.parent.gameObject) continue;
            if (hit.collider.gameObject == planet.gameObject)           continue;
            blocked = true;
            break;
        }
        SetActive(planet, !blocked);
    }

    private void SetActive(GamePlanet planet, bool active) {
        bool wasActive = activeTargets.Contains(planet);
        if (active == wasActive) return;
        if (active) {
            activeTargets.Add(planet);
            planet.AddInfluence(this, element.heatEffect, element.humidityEffect, element.atmosphereEffect);
        } else {
            activeTargets.Remove(planet);
            planet.RemoveInfluence(this);
        }
    }

    private void OnDisable() {
        foreach (var planet in activeTargets) { planet.RemoveInfluence(this); }
        activeTargets.Clear();
        candidates.Clear();
    }
}