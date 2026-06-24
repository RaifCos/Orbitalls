using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [Header("Emission Properties")]
    [SerializeField] private int heatChange;
    [SerializeField] private int humidityChange;
    [SerializeField] private int atmosphereChange;

    [Header("Collision Information")]
    private LayerMask planetMask;
    private readonly HashSet<Planet> candidates = new();
    private readonly HashSet<Planet> activeTargets = new();

    private GamePlanet gamePlanet;
    public void Initialize(GamePlanet owner) => gamePlanet = owner;

    private void OnEnable() {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) {
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(planetMask);
            filter.useTriggers = true;

            List<Collider2D> overlapping = new List<Collider2D>();
            Physics2D.OverlapCollider(col, filter, overlapping);

            foreach (var other in overlapping) {
                if (other.TryGetComponent<Planet>(out var planet) && planet != this) {
                    candidates.Add(planet);
                }
            }
        }

        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
    }

    private void OnDisable() {
        foreach (var target in activeTargets) {
            target.RemoveInfluence(this);
        }
        activeTargets.Clear();
        candidates.Clear();
    }

   private void FixedUpdate() {
        foreach (var planet in new List<Planet>(candidates)) { 
            ValidateView(planet); 
        }
    }

    private void ValidateView(Planet planet) {
        Vector2 origin = transform.position;
        Vector2 target = planet.transform.position;
        Vector2 dir = (target - origin).normalized;
        float dist = Vector2.Distance(origin, target);

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, dist, planetMask);

        bool blocked = false;
        foreach (var hit in hits) {
            if (hit.collider.gameObject == transform.parent.gameObject) continue;
            if (hit.collider.gameObject == planet.gameObject) continue;
            blocked = true;
            break;
        } SetActive(planet, !blocked);
    }

    private void SetActive(Planet planet, bool active) {
        bool wasActive = activeTargets.Contains(planet);
        if (active == wasActive) return;
        if (active) {
            activeTargets.Add(planet);
            planet.AddInfluence(this, heatChange, humidityChange, atmosphereChange);
        } else {
            activeTargets.Remove(planet);
            planet.RemoveInfluence(this);
        }
    }

    public void AddInfluence(object source, int heat, int humidity, int atmosphere) {
        if (gamePlanet == null) gamePlanet = GetComponentInParent<GamePlanet>();
        if (gamePlanet == null) return;
        gamePlanet.ApplyInfluence(source, heat, humidity, atmosphere);
    }

    public void RemoveInfluence(object source) {
        if (gamePlanet == null) gamePlanet = GetComponentInParent<GamePlanet>();
        if (gamePlanet == null) return;
        gamePlanet.RemoveInfluence(source);
    }

    public void Teardown() {
        foreach (var target in activeTargets) {
            target.RemoveInfluence(this);
        }
        activeTargets.Clear();
        candidates.Clear();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.TryGetComponent<Planet>(out var planet)) { candidates.Add(planet); }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Planet>(out var planet)) { candidates.Add(planet); }
    }

    private void OnTriggerExit2D(Collider2D other) {
    if (other.TryGetComponent<Planet>(out var planet)) {
        candidates.Remove(planet);
        SetActive(planet, false);
    }
}
}