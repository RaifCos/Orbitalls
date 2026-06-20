using System.Collections.Generic;
using UnityEngine;

public class Sunlight : MonoBehaviour {

    [SerializeField] private LayerMask planetMask;

    private readonly HashSet<GamePlanet> candidates   = new();
    private readonly HashSet<GamePlanet> litPlanets   = new();

    private void FixedUpdate() { foreach (var planet in candidates) ValidateSunlight(planet); }

    private void OnTriggerEnter2D(Collider2D other) { if (other.TryGetComponent<GamePlanet>(out var planet)) candidates.Add(planet); }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.TryGetComponent<GamePlanet>(out var planet)) {
            candidates.Remove(planet);
            SetLit(planet, false);
        }
    }

    private void ValidateSunlight(GamePlanet planet) {
        Vector2 origin = transform.position;
        Vector2 target = planet.transform.position;
        Vector2 dir = (target - origin).normalized;
        float dist = Vector2.Distance(origin, target);

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, dist, planetMask);

        bool blocked = false;
        foreach (var hit in hits) {
            if (hit.collider.gameObject == planet.gameObject) continue;
            blocked = true;
            break;
        } SetLit(planet, !blocked);
    }

    private void SetLit(GamePlanet planet, bool lit) {
        bool wasLit = litPlanets.Contains(planet);
        if (lit == wasLit) return;

        if (lit) {
            litPlanets.Add(planet);
            planet.AddInfluence(this,  1, 0, 0);
        } else {
            litPlanets.Remove(planet);
            planet.RemoveInfluence(this);
        }
    }
}