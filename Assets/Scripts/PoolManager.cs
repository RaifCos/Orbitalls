using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour {
    [SerializeField] private GameObject[] planetPrefabs;

    private Dictionary<int, ObjectPool<GameObject>> pools;

    void Awake() {
        GameManager.poolManager = this;
        pools = new Dictionary<int, ObjectPool<GameObject>>();

        for (int i = 0; i < planetPrefabs.Length; i++) {
            int index = i;
            pools[index] = new ObjectPool<GameObject>(
                createFunc:       () => CreatePlanet(index),
                actionOnGet:      obj => obj.SetActive(true),
                actionOnRelease:  obj => ResetAndDeactivate(obj),
                actionOnDestroy:  obj => Destroy(obj),
                collectionCheck: true,
                defaultCapacity:  2, 
                maxSize:          10
            );
        }
    }

    private GameObject CreatePlanet(int index) {
        GameObject obj = Instantiate(planetPrefabs[index]);
        obj.SetActive(false);
        return obj;
    }

    private void ResetAndDeactivate(GameObject obj) {
        obj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        obj.transform.localScale = Vector3.one;

        if (obj.TryGetComponent<Rigidbody2D>(out var rb)) {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (obj.TryGetComponent<SpriteAnimation>(out var anim)) { anim.ResetAnimation(); }
        obj.SetActive(false);
    }

    public GameObject Get(int planetIndex) { return pools[planetIndex].Get(); }

    public void Release(int planetIndex, GameObject obj) { pools[planetIndex].Release(obj); }
}