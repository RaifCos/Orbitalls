using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class LevelTransition : MonoBehaviour {

    [SerializeField] private float slideSpeed;
    [SerializeField] private float edge;
    private float currentTarget;
    private Transform t;
    private Transform[] levelObjects;
    private TransformData[] initTransforms; 

    private struct TransformData {
        public Vector3 position;
        public Quaternion rotation;
        public TransformData(Transform tf) {
            position = tf.position;
            rotation = tf.rotation;
        }
    }

    private void Awake() {
        levelObjects = GetComponentsInChildren<Transform>();
        initTransforms = new TransformData[levelObjects.Length];
        t = GetComponent<Transform>();
        t.position = new(edge, 0f, 0f);
        currentTarget = 0f;
    }

    private void OnEnable() { StartCoroutine(SlideLevel()); }

    public void ExitLevel() {
        currentTarget = -edge;
        StartCoroutine(SlideLevel());
    }

    IEnumerator SlideLevel() {
        GameManager.instance.isPlaying = false;
        while (t.position.x > currentTarget) {
            t.position += slideSpeed * Time.deltaTime * Vector3.left;
            yield return null;
        } if (currentTarget == -edge) gameObject.SetActive(false);
        else {
            for (int i = 0; i < levelObjects.Length; i++) { initTransforms[i] = new TransformData(levelObjects[i]); }
            GameManager.instance.isPlaying = true;
        }
    }

    public void ResetLevel() {
        Debug.Log("reset");
        for (int i = 0; i < initTransforms.Length; i++) {
            levelObjects[i].SetPositionAndRotation(initTransforms[i].position, initTransforms[i].rotation);
        }
    }
}