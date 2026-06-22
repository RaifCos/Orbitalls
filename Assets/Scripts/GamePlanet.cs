using UnityEngine;

public class GamePlanet : MonoBehaviour {

    [Header("Planet Properties")]
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
    }

    public void ApplyTraits(int heat, int humidity, int atmosphere) {
        CurrentHeat = heat;
        CurrentHumidity = humidity;
        CurrentAtmosphere = atmosphere;
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
}