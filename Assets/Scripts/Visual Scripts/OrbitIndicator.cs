using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitIndicator : MonoBehaviour {

    [SerializeField] private int segments = 64;
    [SerializeField] private float lineWidth = 0.04f;
    [SerializeField] private Color color = new Color(1f, 1f, 1f, 0.15f);

    private LineRenderer lr;
    private Transform parentPlanet;
    private float radius;

    private Vector3 lastParentPos;

    private void Awake() {
        lr = GetComponent<LineRenderer>();
        lr.loop = true;
        lr.useWorldSpace = true;
        lr.startWidth = lineWidth;
        lr.endWidth   = lineWidth;
        lr.positionCount = segments;

        var mat = new Material(Shader.Find("Sprites/Default")) { color = color };
        lr.material = mat;
    }

    public void Initialise(Transform orbitParent, float orbitRadius) {
        parentPlanet = orbitParent;
        radius = orbitRadius;
        lastParentPos = parentPlanet.position;
        DrawCircle();
    }

    private void LateUpdate() {
        if (parentPlanet == null) return;
        if (parentPlanet.position != lastParentPos) {
            lastParentPos = parentPlanet.position;
            DrawCircle();
        }
    }

    private void DrawCircle() {
        Vector3 center = parentPlanet.position;
        for (int i = 0; i < segments; i++) {
            float angle = i * Mathf.PI * 2f / segments;
            lr.SetPosition(i, new Vector3(
                center.x + Mathf.Cos(angle) * radius,
                center.y + Mathf.Sin(angle) * radius,
                center.z
            ));
        }
    }
}