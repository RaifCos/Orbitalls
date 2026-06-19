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
        gameObject.GetComponent<CircleCollider2D>().radius = planetType.radius;
        
        GameObject elementGO = transform.GetChild(0).gameObject;
        if (planetType.elementType != null) {
            if (planetType.outputDirection == 1) { elementGO.transform.localPosition = new Vector3(0f, 2f * planetType.radius, 0f); }
            gameElement = elementGO.GetComponent<GameElement>();
            gameElement.SetElement(planetType.elementType, planetType.outputDirection, outputReach);
        } else elementGO.SetActive(false);
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