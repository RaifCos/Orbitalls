using UnityEngine;

public class GamePlanet : MonoBehaviour {

    [SerializeField] private Planet planetType;
    [SerializeField] private int outputReach;

    [Header("Orbit Properties")]
    [SerializeField] private GamePlanet parentPlanet;
    [SerializeField] private float orbitRadius;
    private float orbitAngle;

    public bool OrbitsPlanet => parentPlanet != null;

    void Awake() {
        gameObject.name = planetType.externalName;
        gameObject.GetComponent<SpriteRenderer>().sprite = planetType.sprite;
        gameObject.GetComponent<CircleCollider2D>().radius = planetType.radius;
        if(planetType.elementType != null) {
            GameObject elementGO = transform.GetChild(0).gameObject;
            if (planetType.outputDirection == 1) elementGO.transform.localPosition = new Vector3(0f, 0.5f + planetType.radius, 0f);
            else elementGO.SetActive(false);
            elementGO.GetComponent<GameElement>().SetElement(planetType.elementType, planetType.outputDirection, outputReach);
        }
    }

    public void DriveOrbit(float speed, float deltaTime) {
        orbitAngle += speed * deltaTime;
        if (orbitAngle >= 360f) orbitAngle -= 360f;
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
}