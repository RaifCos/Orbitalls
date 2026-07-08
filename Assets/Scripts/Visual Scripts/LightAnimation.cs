using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

[RequireComponent(typeof(Light2D))]
public class LightAnimation : MonoBehaviour {
    private Light2D stageLight;


    void Start() {
        stageLight = GetComponent<Light2D>();
        StartCoroutine(AnimateLight());
    }

    private IEnumerator AnimateLight() {
        while (true) {
            stageLight.intensity = Mathf.PingPong(Time.time, 1f);
            yield return null;
        }
    }
}
