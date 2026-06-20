using UnityEngine;

public class GamePlanet : MonoBehaviour {

    [SerializeField] private Planet planetType;
    private Planet currentPlanet;
    [SerializeField] private int outputReach;

    [Header("Orbit Properties")]
    [SerializeField] private GamePlanet parentPlanet;
    [SerializeField] private float orbitRadius;
    private float orbitAngle;

    private GameElement gameElement;

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

    private void LateUpdate() {
        if (gameElement == null) return;
        gameElement.Detect();
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
        planetType.heat += heatChange;
        planetType.humidity += humidityChange;
        planetType.atmosphere += atmosphereChange;
        currentPlanet = GameManager.dataManager.UpdatePlanetState(planetType.heat, planetType.humidity, planetType.atmosphere);
        Debug.Log($"Planet {gameObject.name} updated to {currentPlanet.externalName} with traits: Heat={currentPlanet.heat}, Humidity={currentPlanet.humidity}, Atmosphere={currentPlanet.atmosphere}");
        UpdatePlanet();
    }
    
    public void ResetPlanet() {
        currentPlanet = planetType;
        UpdatePlanet();
    }

    public void ApplySpin(float degrees) => transform.Rotate(Vector3.forward, degrees);

    public void DriveOrbit(float speed, float deltaTime) {
        orbitAngle += speed * deltaTime;
        if (orbitAngle >= 360f) orbitAngle -= 360f;
    }
}