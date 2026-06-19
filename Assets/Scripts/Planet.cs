using UnityEngine;

public class Planet : MonoBehaviour {

    [SerializeField] private Planet parentPlanet;
    [SerializeField] private float orbitRadius;
    private float orbitAngle;

    public bool OrbitsPlanet => parentPlanet != null;

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