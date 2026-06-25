using UnityEngine;
using System.Collections.Generic;

public class GameSun : MonoBehaviour {

    [Header("Sun Influence")]
    [SerializeField] private float influenceRadius = 20f;
    [SerializeField] private int heatBonus = 1;
    [SerializeField] private LayerMask planetMask;

    private readonly HashSet<GamePlanet> activeTargets = new();

    private void Update() {
        ScanForPlanets();
    }

    private void ScanForPlanets() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, influenceRadius, planetMask);

        HashSet<GamePlanet> visibleThisFrame = new();

        foreach (var col in hits) {
            if (col is not CircleCollider2D) continue;
            if (!col.TryGetComponent<GamePlanet>(out var planet)) continue;
            if (HasLineOfSight(planet)) visibleThisFrame.Add(planet);
        }

        foreach (var planet in new List<GamePlanet>(activeTargets)) {
            if (!visibleThisFrame.Contains(planet)) {
                planet.RemoveInfluence(this);
                activeTargets.Remove(planet);
            }
        }

        foreach (var planet in visibleThisFrame) {
            if (!activeTargets.Contains(planet)) {
                planet.ApplyInfluence(this, heatBonus, 0, 0);
                activeTargets.Add(planet);
            }
        }
    }

    private bool HasLineOfSight(GamePlanet planet) {
        Vector2 origin = transform.position;
        Vector2 target = planet.transform.position;
        Vector2 dir = (target - origin).normalized;
        float dist = Vector2.Distance(origin, target);

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, dist, planetMask);

        foreach (var hit in hits) {
            if (hit.collider.gameObject == gameObject) continue;
            if (hit.collider.gameObject == planet.gameObject) continue;
            return false;
        }
        return true;
    }

    private void OnDestroy() {
        foreach (var planet in activeTargets)
            if (planet != null) planet.RemoveInfluence(this);
        activeTargets.Clear();
    }
}