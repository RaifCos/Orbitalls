using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class GameElement : MonoBehaviour {

    private Element element;
    private int reach;
    private LayerMask planetMask;
    private readonly HashSet<GamePlanet> candidates = new();    // Possible Targets (need to confirm view)
    private readonly HashSet<GamePlanet> activeTargets = new(); // Confirmed Targets (have clear view)

    private void FixedUpdate() { foreach (var planet in candidates) { ValidateView(planet); } }

    public void SetElement(Element e, int r) {
        element = e;
        reach = r;
        planetMask = LayerMask.GetMask("Planet");
        gameObject.name = element.internalName;

        // Set Elememt Transform
        float parentRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
        float height = reach - parentRadius;
        float width = 0.8f;
        transform.localScale    = Vector3.one;
        transform.localPosition = new Vector3(0f, parentRadius + height * 0.5f, 0f);

        // Set Element Sprite
        var sr = GetComponent<SpriteRenderer>();
        sr.sprite   = element.sprite;
        sr.drawMode = SpriteDrawMode.Sliced;
        sr.size = new Vector2(width, height);

        // Set Collider
        var col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.offset = Vector2.zero;
        col.size = new Vector2(width, height);
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
        Vector2 dir = (target - origin).normalized;
        float dist = Vector2.Distance(origin, target);

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, dist, planetMask);

        bool blocked = false;
        foreach (var hit in hits) {
            if (hit.collider.gameObject == transform.parent.gameObject) continue;
            if (hit.collider.gameObject == planet.gameObject)           continue;
            blocked = true;
            break;
        } SetActive(planet, !blocked);
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