using UnityEngine;
using UnityEngine.Pool;

public class ParticlePool : MonoBehaviour {
    [SerializeField] private ParticleSystem prefab;

    private ObjectPool<ParticleSystem> _pool;

    void Awake() {
        _pool = new ObjectPool<ParticleSystem>(
            createFunc:    () => Instantiate(prefab),
            actionOnGet:   ps => ps.gameObject.SetActive(true),
            actionOnRelease: ps => {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ps.gameObject.SetActive(false);
            },
            actionOnDestroy: ps => Destroy(ps.gameObject),
            defaultCapacity: 10
        );
    }

    public ParticleSystem Get()    => _pool.Get();
    public void Release(ParticleSystem ps) => _pool.Release(ps);
}