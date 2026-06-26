using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class BackgroundAnimation : MonoBehaviour {

    private Transform t;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float startingPoint;

    void Awake() { t = GetComponent<Transform>(); }

    void FixedUpdate() {
        t.position += 1f  * Time.deltaTime * Vector3.down;
        if (t.position.y < -10f) { t.position += 10f * Vector3.up; }
    }

    public void StartSliding() {
        t.position = startingPoint * Vector3.right;
        StartCoroutine(SlideBackground());
    }

    IEnumerator SlideBackground() {
        while (t.position.x > 0f) {
            t.position += slideSpeed  * Time.deltaTime * Vector3.left;
            yield return null;
        }
    }
}
