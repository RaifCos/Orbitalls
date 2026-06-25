using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class LevelTransition : MonoBehaviour {
    
    [SerializeField] private float slideSpeed;
    [SerializeField] private float endingPoint;
    private float currentTarget;
    private Transform t;

    private void OnEnable() {
        t = GetComponent<Transform>();
        currentTarget = 0f;
        StartCoroutine(SlideLevel());
    }

    public void ExitLevel() {
        currentTarget = endingPoint;
        StartCoroutine(SlideLevel());
    }

    IEnumerator SlideLevel() {
    GameManager.instance.isPlaying = false;
        while (t.position.x > currentTarget) {
            t.position += slideSpeed  * Time.deltaTime * Vector3.left;
            yield return null;
        } if (currentTarget == endingPoint) gameObject.SetActive(false);
        else { GameManager.instance.isPlaying = true; }
    }
}
