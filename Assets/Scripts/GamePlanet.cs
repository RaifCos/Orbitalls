using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteAnimation))]
public class GamePlanet : MonoBehaviour {

    [Header("Default Climate")]
    [SerializeField] private int heat;
    [SerializeField] private int humidity;
    [SerializeField] private int atmosphere;

    [Header("Orbit Properties")]
    [SerializeField] private GameObject parentPlanet;
    [SerializeField] private float orbitRadius;
    private OrbitIndicator orbitIndicator;

    [Header("Game Object Properties")]
    private Planet currentPlanet, newPlanet;
    private ParticleSystem transitionParticle;
    private BoxCollider2D emissionTrigger;
    private SpriteAnimation spriteAnimation;
    private readonly Dictionary<Planet, GameObject> particleInstances = new();
    private GameObject activeParticleInstance;

    [Header("Collision Properties")]
    [SerializeField] private LayerMask planetMask;
    private readonly HashSet<GamePlanet> candidates = new();
    private readonly HashSet<GamePlanet> activeTargets = new();
    private readonly Dictionary<object, (int heat, int humidity, int atmosphere)> persistedInfluences = new();

    private float orbitAngle;
    public bool OrbitsPlanet => parentPlanet != null;
    public int BaseHeat => heat;
    public int BaseHumidity => humidity;
    public int BaseAtmosphere => atmosphere;

    private int currentHeat, currentHumidity, currentAtmosphere;

    private void Start() {
        transitionParticle = GetComponent<ParticleSystem>();
        emissionTrigger = GetComponent<BoxCollider2D>();
        spriteAnimation = GetComponent<SpriteAnimation>();
        currentHeat = heat;
        currentHumidity = humidity;
        currentAtmosphere = atmosphere;
        newPlanet = GameManager.instance.GetPlanet(heat, humidity, atmosphere);
        SetPlanet();

        orbitIndicator = GetComponent<OrbitIndicator>();
        if (OrbitsPlanet && orbitIndicator != null) orbitIndicator.Initialise(parentPlanet.transform, orbitRadius);
    }

    private void Update() {
        foreach (var planet in new List<GamePlanet>(candidates)) { ValidateView(planet); }
        if (!OrbitsPlanet) return;

        Vector2 parentPos = parentPlanet.transform.position;
        float rad = orbitAngle * Mathf.Deg2Rad;
        transform.position = new Vector2(
            parentPos.x + orbitRadius * Mathf.Cos(rad),
            parentPos.y + orbitRadius * Mathf.Sin(rad)
        );
    }

    private void OnDestroy() {
        foreach (var instance in particleInstances.Values)
            if (instance != null) Destroy(instance);
        particleInstances.Clear();
    }

    #region Player Movements

    public void ApplySpin(float degrees) => transform.Rotate(Vector3.forward, degrees);

    public void DriveOrbit(float speed, float deltaTime) {
        orbitAngle += speed * deltaTime;
        if (orbitAngle >= 360f) orbitAngle -= 360f;
    }

    #endregion
    #region Planet State Updates

    public void ApplyTraits(int heat, int humidity, int atmosphere) {
        currentHeat = heat;
        currentHumidity = humidity;
        currentAtmosphere = atmosphere;
        newPlanet = GameManager.instance.GetPlanet(currentHeat, currentHumidity, currentAtmosphere);
        if (currentPlanet != newPlanet) { SetPlanet(); }
    }

    private void SetPlanet() {
        if (newPlanet == null) {
            Debug.LogWarning($"No planet definition found for climate values ({heat}, {humidity}, {atmosphere}).");
            return;
        }

        if (activeParticleInstance != null) {
            activeParticleInstance.SetActive(false);
            activeParticleInstance = null;
        }

        transitionParticle.Play();
        currentPlanet = newPlanet;
        spriteAnimation.ChangeAnimation(currentPlanet);
        emissionTrigger.enabled = currentPlanet.hasEmissions;

        if (currentPlanet.hasEmissions && currentPlanet.particlePrefab != null) {
            if (!particleInstances.TryGetValue(currentPlanet, out var instance)) {
                instance = Instantiate(currentPlanet.particlePrefab, transform);
                particleInstances[currentPlanet] = instance;
            }
            instance.SetActive(true);
            activeParticleInstance = instance;
        }

        if (currentPlanet.isHome) { /* GameManager function to complete level */ }
    }

    #endregion
    #region Planet Emission Updates


    private void ValidateView(GamePlanet planet) {
        Vector2 origin = transform.position;
        Vector2 target = planet.transform.position;
        Vector2 dir = (target - origin).normalized;
        float dist = Vector2.Distance(origin, target);

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, dist, planetMask);

        bool blocked = false;
        foreach (var hit in hits) {
            if (hit.collider.gameObject == gameObject) continue;
            if (hit.collider.gameObject == planet.gameObject) continue;
            blocked = true;
            break;
        } SetActiveTarget(planet, !blocked);
    }

    private void SetActiveTarget(GamePlanet planet, bool active) {
        bool wasActive = activeTargets.Contains(planet);
        if (active == wasActive) return;
        if (active) {
            activeTargets.Add(planet);
            planet.ApplyInfluence(this, currentPlanet.heat, currentPlanet.humidity, currentPlanet.atmosphere);
        } else {
            activeTargets.Remove(planet);
            planet.RemoveInfluence(this);
        }
    }

    private void RecalculateAndApply() {
        int he = BaseHeat, hu = BaseHumidity, at = BaseAtmosphere;
        foreach (var (h, hum, atm) in persistedInfluences.Values) {
            he += h;
            hu += hum;
            at += atm;
        } ApplyTraits(he, hu, at);
    }

    public void ApplyInfluence(object source, int heat, int humidity, int atmosphere) {
        persistedInfluences[source] = (heat, humidity, atmosphere);
        RecalculateAndApply();
    }

    public void RemoveInfluence(object source) { if (persistedInfluences.Remove(source)) RecalculateAndApply(); }

    private void OnTriggerEnter2D(Collider2D other) {
    if (other is CircleCollider2D && other.TryGetComponent<GamePlanet>(out var planet))
        candidates.Add(planet);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other is CircleCollider2D && other.TryGetComponent<GamePlanet>(out var planet)) {
            candidates.Remove(planet);
            SetActiveTarget(planet, false);
        }
    }

    #endregion
}