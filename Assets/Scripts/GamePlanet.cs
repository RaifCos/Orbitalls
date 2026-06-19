using UnityEngine;

public class GamePlanet : MonoBehaviour {

    [SerializeField] private Planet planetType;
    [SerializeField] private int outputReach;

    [Header("Orbit Properties")]
    [SerializeField] private GamePlanet parentPlanet;
    [SerializeField] private float orbitRadius;
    private float orbitAngle;

    private GameElement gameElement;

    public bool OrbitsPlanet => parentPlanet != null;

    void Awake() {
        gameObject.name = planetType.externalName;
        gameObject.GetComponent<SpriteRenderer>().sprite = planetType.sprite;
        gameObject.GetComponent<CircleCollider2D>().radius = outputReach;
        
        if (planetType.element != null) {
            GameObject elementGO = transform.GetChild(0).gameObject;
            gameElement = elementGO.GetComponent<GameElement>();
            gameElement.SetElement(planetType.element, outputReach);
        }
    }

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

    public void ApplySpin(float degrees) => transform.Rotate(Vector3.forward, degrees);

    public void DriveOrbit(float speed, float deltaTime) {
        orbitAngle += speed * deltaTime;
        if (orbitAngle >= 360f) orbitAngle -= 360f;
    }
}