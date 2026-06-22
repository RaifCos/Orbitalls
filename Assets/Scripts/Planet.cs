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
    private readonly Dictionary<object, (int heat, int humidity, int atmosphere)> activeInfluences = new();

    private GamePlanet gamePlanet;

    private void Awake() { gamePlanet = GetComponentInParent<GamePlanet>(); }

    private void FixedUpdate() { foreach (var planet in candidates) { ValidateView(planet); } }

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
        activeInfluences[source] = (heat, humidity, atmosphere);
        RecalculateTraits();
    }

    public void RemoveInfluence(object source) { if (activeInfluences.Remove(source)) { RecalculateTraits(); } }

    private void RecalculateTraits() {
        int he = heatChange, hu = humidityChange, at = atmosphereChange;
        foreach (var inf in activeInfluences.Values) {
            he += inf.heat;
            hu += inf.humidity;
            at += inf.atmosphere;
        } gamePlanet?.ApplyTraits(he, hu, at);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.TryGetComponent<Planet>(out var planet)) { candidates.Add(planet); }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Planet>(out var planet)) { candidates.Add(planet); }
    }
}