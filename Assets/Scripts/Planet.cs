using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
    
    [Header("Planet Properties")]
    [SerializeField] private int heat;
    [SerializeField] private int humidity;
    [SerializeField] private int atmosphere;

    [Header("Emission Properties")]
    [SerializeField] private bool hasEmissions;
    [SerializeField] private int heatChange;
    [SerializeField] private int humidityChange;
    [SerializeField] private int atmosphereChange;

    [Header("Collision Information")]
    [SerializeField] private bool changesState;
    private LayerMask planetMask;
    private readonly HashSet<Planet> candidates = new();
    private readonly HashSet<Planet> activeTargets = new();
    private readonly Dictionary<object, (int heat, int humidity, int atmosphere)> activeInfluences = new();

    [Header("Orbit Properties")]
    [SerializeField] private GameObject levelSun;
    [SerializeField] private GameObject parentPlanet;
    [SerializeField] private float orbitRadius;
    private float orbitAngle;
    public bool OrbitsPlanet => parentPlanet != null;

    private void FixedUpdate() { foreach (var planet in candidates) { ValidateView(planet); }}

    private void Update() {
        if (!OrbitsPlanet) return;

        Vector2 parentPos = parentPlanet.transform.position;
        float rad = orbitAngle * Mathf.Deg2Rad;
        transform.position = new Vector2(
            parentPos.x + orbitRadius * Mathf.Cos(rad),
            parentPos.y + orbitRadius * Mathf.Sin(rad)
        );
    }

    public void ApplySpin(float degrees) => transform.Rotate(Vector3.forward, degrees);

    public void DriveOrbit(float speed, float deltaTime) {
        orbitAngle += speed * deltaTime;
        if (orbitAngle >= 360f) orbitAngle -= 360f;
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
        activeInfluences[source] = (heat, humidity, atmosphere);
        RecalculateTraits();
    }

    public void RemoveInfluence(object source) { if (activeInfluences.Remove(source)) { RecalculateTraits(); } }

    private void RecalculateTraits() {
        if (changesState == false) { return; }
        int he = heat, hu = humidity, at = atmosphere;
        foreach (var inf in activeInfluences.Values) {
            he += inf.heat;
            hu += inf.humidity;
            at += inf.atmosphere;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.TryGetComponent<Planet>(out var planet)) { candidates.Add(planet); }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Planet>(out var planet)) { candidates.Add(planet); }
    }
}