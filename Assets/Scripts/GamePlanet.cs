using UnityEngine;

public class GamePlanet : MonoBehaviour {

    [SerializeField] private Planet planetType;
    private Planet currentPlanet;
    private int heat;
    private int humidity;
    private int atmosphere;

    [Header("Element Properties")]
    private GameElement gameElement;
    [SerializeField] private int outputReach;

    [Header("Orbit Properties")]
    [SerializeField] private GameObject levelSun;
    [SerializeField] private GamePlanet parentPlanet;
    [SerializeField] private float orbitRadius;
    private float orbitAngle;

    [Header("Sunlight Properties")]
    [SerializeField] private LayerMask planetLayer;
    private bool isInSunlight;

    public bool OrbitsPlanet => parentPlanet != null;

    void Awake() => ResetPlanet();

    private void Update() {
        if (!OrbitsPlanet) return;

        Vector2 parentPos = parentPlanet.transform.position;
        float rad = orbitAngle * Mathf.Deg2Rad;

        transform.position = new Vector2 (
            parentPos.x + orbitRadius * Mathf.Cos(rad),
            parentPos.y + orbitRadius * Mathf.Sin(rad)
        );
    }

    private void FixedUpdate() {
        if (levelSun == null) return;
        CheckSunlight();
    }

    private void CheckSunlight() {
        Vector2 origin = transform.position;
        Vector2 target = levelSun.transform.position;
        Vector2 direction = (target - origin).normalized;
        float distance = Vector2.Distance(origin, target);

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, planetLayer);

        if (hit.collider != null && hit.collider.gameObject == gameObject) { hit = default; }

        bool wasInSunlight = isInSunlight;
        isInSunlight = hit.collider == null;

        Debug.DrawLine(origin, target, isInSunlight ? Color.yellow : Color.red);

        if (isInSunlight == wasInSunlight) return;

        if (isInSunlight) {
            Debug.Log($"{gameObject.name} entered sunlight, heat +1");
            UpdatePlanetTraits(1, 0, 0);
        } else {
            Debug.Log($"{gameObject.name} blocked by {hit.collider.gameObject.name}, heat -1");
            UpdatePlanetTraits(-1, 0, 0);
        }
    }

    private void UpdatePlanet() {
        gameObject.name = currentPlanet.externalName;
        gameObject.GetComponent<SpriteRenderer>().sprite = currentPlanet.sprite;

        if (currentPlanet.element != null) {
            GameObject elementGO = transform.GetChild(0).gameObject;
            gameElement = elementGO.GetComponent<GameElement>();
            gameElement.SetElement(currentPlanet.element, outputReach);
        }
    }

    public void UpdatePlanetTraits(int heatChange, int humidityChange, int atmosphereChange) {
        heat += heatChange;
        humidity += humidityChange;
        atmosphere += atmosphereChange;
        currentPlanet = GameManager.dataManager.UpdatePlanetState(heat, humidity, atmosphere);
        Debug.Log($"Planet {gameObject.name} updated to {currentPlanet.externalName} with traits: Heat={heat}, Humidity={humidity}, Atmosphere={atmosphere}");
        UpdatePlanet();
    }

    public void ResetPlanet() {
        currentPlanet = planetType;
        heat = currentPlanet.heat;
        humidity = currentPlanet.humidity;
        atmosphere = currentPlanet.atmosphere;
        UpdatePlanet();
    }

    public void ApplySpin(float degrees) => transform.Rotate(Vector3.forward, degrees);

    public void DriveOrbit(float speed, float deltaTime) {
        orbitAngle += speed * deltaTime;
        if (orbitAngle >= 360f) orbitAngle -= 360f;
    }
}