using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
public class GamePlanet : MonoBehaviour {

    [Header("Planet Properties")]
    private int currentPlanetIndex = -1;
    private GameObject currentPlanetObj;
    private ParticleSystem transitionParticle;
    private readonly Dictionary<object, (int heat, int humidity, int atmosphere)> persistedInfluences = new();


    [Header("Climate Properties")]
    [SerializeField] private int heat;
    [SerializeField] private int humidity;
    [SerializeField] private int atmosphere;

    [Header("Orbit Properties")]
    [SerializeField] private GameObject levelSun;
    [SerializeField] private GameObject parentPlanet;
    [SerializeField] private float orbitRadius;

    private float orbitAngle;
    public bool OrbitsPlanet => parentPlanet != null;

    public int BaseHeat => heat;
    public int BaseHumidity => humidity;
    public int BaseAtmosphere => atmosphere;

    public int CurrentHeat { get; private set; }
    public int CurrentHumidity { get; private set; }
    public int CurrentAtmosphere { get; private set; }

    private void Start() {
        CurrentHeat = heat;
        CurrentHumidity = humidity;
        CurrentAtmosphere = atmosphere;
        transitionParticle = GetComponent<ParticleSystem>();
        currentPlanetIndex = GameManager.instance.GetPlanetIndex(heat, humidity, atmosphere);
        SetPlanet(currentPlanetIndex);
    }

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

    public void ApplyTraits(int heat, int humidity, int atmosphere) {
        CurrentHeat = heat;
        CurrentHumidity = humidity;
        CurrentAtmosphere = atmosphere;
        int newPlanetIndex = GameManager.instance.GetPlanetIndex(heat, humidity, atmosphere);
        if (newPlanetIndex != currentPlanetIndex) {
            SetPlanet(newPlanetIndex);
        }
    }

    private void SetPlanet(int newIndex) {
        transitionParticle.Play();

        if (currentPlanetObj != null) {
            foreach (var planet in currentPlanetObj.GetComponentsInChildren<Planet>()) {
                planet.Teardown();
            }
            GameManager.poolManager.Release(currentPlanetIndex, currentPlanetObj);
        }

        currentPlanetIndex = newIndex;
        currentPlanetObj = GameManager.poolManager.Get(currentPlanetIndex);
        currentPlanetObj.transform.SetParent(transform);
        currentPlanetObj.transform.localPosition = Vector3.zero;
        currentPlanetObj.transform.localRotation = Quaternion.identity;

        foreach (var planet in currentPlanetObj.GetComponentsInChildren<Planet>()) {
            planet.Initialize(this);
        }
}

    public void ClearPlanet() {
        if (currentPlanetObj == null) return;
        GameManager.poolManager.Release(currentPlanetIndex, currentPlanetObj);
        currentPlanetObj = null;
        currentPlanetIndex = -1;
    }
    
    public void ApplyInfluence(object source, int heat, int humidity, int atmosphere) {
        persistedInfluences[source] = (heat, humidity, atmosphere);
        RecalculateAndApply();
    }

    public void RemoveInfluence(object source) {
        if (persistedInfluences.Remove(source))
            RecalculateAndApply();
    }

    private void RecalculateAndApply() {
        int he = heat, hu = humidity, at = atmosphere;
        foreach (var (h, hum, atm) in persistedInfluences.Values)
        {
            he += h;
            hu += hum;
            at += atm;
        }
        ApplyTraits(he, hu, at);
    }

}