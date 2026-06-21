using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]

[RequireComponent(typeof(SpriteAnimation))]
public class GamePlanet : MonoBehaviour {

    [SerializeField] private Planet planetType;
    private Planet currentPlanet;

    [Header("Element Properties")]
    private GameElement gameElement;
    [SerializeField] private int elementRange;

    [Header("Orbit Properties")]
    [SerializeField] private GameObject levelSun;
    [SerializeField] private GameObject parentPlanet;
    [SerializeField] private float orbitRadius;
    private float orbitAngle;
    public bool OrbitsPlanet => parentPlanet != null;

    [Header("Influence Properties")]
    private readonly Dictionary<object, (int heat, int humidity, int atmosphere)> activeInfluences = new();
    private int baseHeat;
    private int baseHumidity;
    private int baseAtmosphere;

    void Awake() { ResetPlanet(); }

    void Start() { InitOrbitIndicator(); }

    private void Update() {
        if (!OrbitsPlanet) return;

        Vector2 parentPos = parentPlanet.transform.position;
        float rad = orbitAngle * Mathf.Deg2Rad;
        transform.position = new Vector2(
            parentPos.x + orbitRadius * Mathf.Cos(rad),
            parentPos.y + orbitRadius * Mathf.Sin(rad)
        );
    }

    public void AddInfluence(object source, int heat, int humidity, int atmosphere) {
        activeInfluences[source] = (heat, humidity, atmosphere);
        RecalculateTraits();
    }

    public void RemoveInfluence(object source) { if (activeInfluences.Remove(source)) { RecalculateTraits(); } }

    private void RecalculateTraits() {
        if (planetType.changesState == false) { return; }
        int heat = baseHeat, humidity = baseHumidity, atmosphere = baseAtmosphere;
        foreach (var inf in activeInfluences.Values) {
            heat += inf.heat;
            humidity += inf.humidity;
            atmosphere += inf.atmosphere;
        } currentPlanet = GameManager.dataManager.UpdatePlanetState(heat, humidity, atmosphere);
        UpdatePlanetVisuals();
    }

    private void UpdatePlanetVisuals() {
        gameObject.name = currentPlanet.externalName;
        GetComponent<SpriteAnimation>().UpdateSpriteList(currentPlanet.sprites);

        // Error Fallbacks
        if (currentPlanet.element == null) return;
        if (transform.childCount == 0) return;
        GameObject elementGO = transform.GetChild(0).gameObject;
        if (!elementGO.TryGetComponent(out gameElement)) return;
    
        gameElement.SetElement(currentPlanet.element, elementRange);
    }

    public void ResetPlanet() {
        activeInfluences.Clear();
        currentPlanet = planetType;
        baseHeat = currentPlanet.heat;
        baseHumidity = currentPlanet.humidity;
        baseAtmosphere = currentPlanet.atmosphere;
        UpdatePlanetVisuals();
    }

    public void ApplySpin(float degrees) => transform.Rotate(Vector3.forward, degrees);

    public void DriveOrbit(float speed, float deltaTime) {
        orbitAngle += speed * deltaTime;
        if (orbitAngle >= 360f) orbitAngle -= 360f;
    }

    private void InitOrbitIndicator() {
        if (!OrbitsPlanet) return; 
        OrbitIndicator indicator = GetComponentInChildren<OrbitIndicator>();
        if (indicator == null) {
            var go = new GameObject("OrbitIndicator");
            go.transform.SetParent(transform);
            indicator = go.AddComponent<OrbitIndicator>();
        } indicator.Initialise(parentPlanet.transform, orbitRadius);
    }
}