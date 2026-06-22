using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class GameElement : MonoBehaviour {

    private Element element;
    private int reach;
    private LayerMask planetMask;
    private readonly HashSet<GamePlanet> candidates = new();
    private readonly HashSet<GamePlanet> activeTargets = new();

    private float parentRadius;
    private float fullHeight;
    private float width;
    private BoxCollider2D col;

    private readonly Dictionary<GamePlanet, ParticleSystem> _activeParticles = new();

    private void FixedUpdate() { foreach (var planet in candidates) { ValidateView(planet); }}

    public void SetElement(Element e, int r, float w) {
        element = e;
        reach = r;
        planetMask = LayerMask.GetMask("Planet");
        gameObject.name = element.internalName;

        width = w;
        parentRadius = transform.parent.GetComponent<CircleCollider2D>().radius;
        fullHeight = reach - parentRadius;

        transform.localScale    = Vector3.one;
        transform.localPosition = new Vector3(0f, parentRadius + fullHeight * 0.5f, 0f);

        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.offset = Vector2.zero;
        col.size = new Vector2(width, fullHeight);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject == transform.parent.gameObject) return;
        if (other.TryGetComponent<GamePlanet>(out var planet)) { candidates.Add(planet); }
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
            if (hit.collider.gameObject == planet.gameObject) continue;
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
            SpawnParticle(planet);
        } else {
            activeTargets.Remove(planet);
            planet.RemoveInfluence(this);
            ReleaseParticle(planet);
        }
    }

    private void SpawnParticle(GamePlanet planet) {
        var ps = GameManager.particleManager.Get(element.internalName);
        if (ps == null) return;
        var parent = transform.parent;
        ps.transform.SetParent(parent);
        ps.transform.position = parent.position;
        ps.Play();
        _activeParticles[planet] = ps;
    }

    private void ReleaseParticle(GamePlanet planet) {
        if (!_activeParticles.TryGetValue(planet, out var ps)) return;
        ps.transform.SetParent(null);
        GameManager.particleManager.Release(element.internalName, ps);
        _activeParticles.Remove(planet);
    }

    private void OnDisable() {
        foreach (var planet in activeTargets) { planet.RemoveInfluence(this); }
        activeTargets.Clear();
        candidates.Clear();

        foreach (var (planet, ps) in new Dictionary<GamePlanet, ParticleSystem>(_activeParticles)) {
            ps.transform.SetParent(null);
            GameManager.particleManager.Release(element.internalName, ps);
        }
        _activeParticles.Clear();
    }
}